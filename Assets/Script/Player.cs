using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Hozuki;
using UnityEngine.SceneManagement;

namespace GameJam.Sho
{
    public class Player : MonoBehaviour
    {
        public enum Direction
        {
            Right = 1,
            Left = -1,
        }
        [SerializeField, Header("Move Speed")]
        private float speed = 10.0f;

        [SerializeField, Header("Jump Power")]
        private float jumpPower = 10.0f;

        [SerializeField, Header("Move Speed Max")]
        private /*readonly*/ float speedMax = 10.0f;

        [SerializeField]
        private /*readonly*/ KeyCode rightMove = KeyCode.RightArrow;
        [SerializeField]
        private /*readonly*/ KeyCode leftMove = KeyCode.LeftArrow;
        [SerializeField]
        private /*readonly*/ KeyCode jump = KeyCode.UpArrow;
        [SerializeField]
        private /*readonly*/ KeyCode attack = KeyCode.Space;
        [SerializeField]
        private /*readonly*/ KeyCode windAttack = KeyCode.Z;

        public Rigidbody2D Rigidbody { get; set; } = null;

        [SerializeField]
        private float gravityScaleWhenJumping = 10.0f;

        private float gravityScale { get; set; } = 1.0f;
        [SerializeField]
        private bool isOnGround = false;
        public bool IsOnGround
        {
            get { return isOnGround; }
            set
            {
                if (!value)
                {
                    Rigidbody.gravityScale = gravityScaleWhenJumping;
                }
                else
                {
                    Rigidbody.gravityScale = gravityScale;
                }
                isOnGround = value;
            }
        }

        [SerializeField]
        private GameObject shurikenPrefab = null;
        [SerializeField]
        private float shurikenThrowPower = 100.0f;

        [SerializeField]
        private GameObject windPrefab = null;

        [SerializeField]
        private Direction currentDirection = Direction.Right;
        public Direction CurrentDirection { get { return currentDirection; } set { currentDirection = value; } }

        private AudioSource[] Audios;
        private AudioSource WalkSound => Audios[0];
        private AudioSource AttackByShuriken => Audios[1];
        private AudioSource AttackByWind => Audios[2];
        //private AudioSource Jump => Audios[3];

        private bool IsHittedWithGround { get; set; } = false;

        private PlayerSpriteController MotionController { get; set; } = null;

        [SerializeField]
        private float attackDelay = 0.5f;

        public SpriteRenderer Renderer { get; set; } = null;

        [SerializeField]
        private GameObject husuma;

        // Use this for initialization
        void Start()
        {
            Rigidbody = this.GetComponent<Rigidbody2D>();
            gravityScale = Rigidbody.gravityScale;
            IsOnGround = false;
            Audios = this.GetComponents<AudioSource>();
            MotionController = this.GetComponentInChildren<PlayerSpriteController>();

            WalkSound.pitch *= 2.0f;
            Renderer = this.GetComponentInChildren<SpriteRenderer>();

            var status = this.GetComponent<PlayerStatus>();

            // 移動の処理/ For Move
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (Input.GetKey(rightMove))
                    {
                        Rigidbody.AddForce(Vector2.right * speed * Time.fixedDeltaTime);
                        CurrentDirection = Direction.Right;
                        if (!WalkSound.isPlaying && (IsOnGround && IsHittedWithGround)) WalkSound.PlayOneShot(WalkSound.clip);
                        MotionController.RunStart();

                        var scale = this.transform.localScale;
                        if (scale.x < 0) scale.x *= -1;
                        this.transform.localScale = scale;
                    }
                    else if (Input.GetKey(leftMove))
                    {
                        Rigidbody.AddForce(Vector2.left * speed * Time.fixedDeltaTime);
                        CurrentDirection = Direction.Left;
                        if (!WalkSound.isPlaying && (IsOnGround && IsHittedWithGround)) WalkSound.PlayOneShot(WalkSound.clip);
                        MotionController.RunStart();
                        var scale = this.transform.localScale;
                        if (scale.x > 0) scale.x *= -1;
                        this.transform.localScale = scale;
                    }
                    else
                    {
                        MotionController.RunStop();
                    }
                    if (Rigidbody.velocity.magnitude >= speedMax)
                    {
                        Rigidbody.velocity = Rigidbody.velocity.normalized * speedMax;
                    }

                    Rigidbody.velocity *= 0.8f;
                }).AddTo(this);

            // Jump
            this.FixedUpdateAsObservable()
                .Where(n => IsOnGround && Input.GetKeyDown(jump))
                .Subscribe(_ =>
                {
                    Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0.0f);
                    Rigidbody.AddForce(Vector2.up * jumpPower);
                    MotionController.JumpStart();
                }).AddTo(this);

            // 接地判定 / Check is on Ground
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var past = IsOnGround;
                    IsOnGround = Rigidbody.velocity.y <= 0.1f && Rigidbody.velocity.y >= -0.1f;
                    if (!MotionController.IsJumping() && !IsOnGround)
                    {
                        MotionController.ForceJumpStart();
                    }
                }).AddTo(this);

            // throwing Shuriken
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(attack))
                .Subscribe(_ =>
                {
                    this.DelayMethod(attackDelay, () =>
                    {
                        var shuriken = CreateNewItem<Shuriken>(shurikenPrefab);

                        var d = Vector2.right;
                        d.x = (int)CurrentDirection;
                        shuriken.Rigidbody.AddForce(d * shurikenThrowPower);

                        // temp
                        GameObject.Destroy(shuriken, 5.0f);

                        AttackByShuriken.Play();
                    });
                    MotionController.Attack();
                }).AddTo(this);

            // Creating Wind
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(windAttack))
                .Subscribe(_ =>
                {
                    if (status.WindMax <= status.CurrentWindCount) return;
                    CreateNewItem<Wind>(windPrefab);
                    AttackByWind.Play();
                    MotionController.Attack();
                    status.CurrentWindCount++;
                }).AddTo(this);

            // temp 当たったオブジェクトが地面かどうかを監視する Check Hitting Object is Ground
            this.OnCollisionStay2DAsObservable()
                .Subscribe(hit =>
                {
                    IsHittedWithGround = hit.gameObject.tag == "Ground";
                    if (IsHittedWithGround)
                    {
                        MotionController.JumpStop();
                    }
                }).AddTo(this);

            // test
            this.UpdateAsObservable()
                .Where(n => Input.GetKeyDown(KeyCode.D))
                .Subscribe(_ =>
                {
                    status.HP--;
                }).AddTo(this);
            // test
            this.UpdateAsObservable()
                .Where(n => Input.GetKeyDown(KeyCode.H))
                .Subscribe(_ =>
                {
                    var h = GameObject.Instantiate(husuma).GetComponent<Husuma>();
                    h.transform.SetParent(GameObject.Find("UI").transform, false);
                    h.IsClose = true;
                }).AddTo(this);
            this.UpdateAsObservable()
                .Where(n => Input.GetKeyDown(KeyCode.G))
                .Subscribe(_ =>
                {
                    var h = GameObject.Instantiate(husuma).GetComponent<Husuma>();
                    h.transform.SetParent(GameObject.Find("UI").transform, false);
                    h.IsClose = false;
                }).AddTo(this);

            status.DeadEvent
                .Subscribe(_ =>
                {
                    Time.timeScale = 0.5f;

                    this.DelayMethodInRealTime(1.0f, () =>
                    {
                        Time.timeScale = 1.0f;
                        var h = GameObject.Instantiate(husuma).GetComponent<Husuma>();
                        h.transform.SetParent(GameObject.Find("UI").transform, false);
                        h.IsClose = true;
                        h.HusumaCompleteEvent
                        .Subscribe(__ =>
                        {
                            Result.State = ClearState.Lose;
                            SceneManager.LoadScene("Result");
                        });
                    });
                }).AddTo(this);
        }

        T CreateNewItem<T>(GameObject prefab) where T : ISpawnedByPlayer
        {
            var newObj = GameObject.Instantiate(prefab).GetComponent<T>();
            newObj.transform.position = this.transform.position;
            var offsetWithDir = newObj.Offset;
            offsetWithDir.x *= (int)CurrentDirection;
            newObj.transform.Translate(offsetWithDir);
            return newObj;
        }
    }
}
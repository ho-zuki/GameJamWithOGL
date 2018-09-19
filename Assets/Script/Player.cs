using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameJam.Sho
{
    public class Player : MonoBehaviour
    {
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

        private Rigidbody2D Rigidbody { get; set; } = null;

        [SerializeField]
        private float gravityScaleWhenJumping = 10.0f;

        private float gravityScale { get; set; } = 1.0f;
        [SerializeField]
        private bool isOnGround = true;
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

        // Use this for initialization
        void Start()
        {
            Rigidbody = this.GetComponent<Rigidbody2D>();
            gravityScale = Rigidbody.gravityScale;
            IsOnGround = false;

            // 移動の処理/ For Move
            this.FixedUpdateAsObservable().Subscribe(_ =>
            {
                if (Input.GetKey(rightMove))
                {
                    Rigidbody.AddForce(Vector2.right * speed * Time.fixedDeltaTime);
                }
                if (Input.GetKey(leftMove))
                {
                    Rigidbody.AddForce(Vector2.left * speed * Time.fixedDeltaTime);
                }
                if (Rigidbody.velocity.magnitude >= speedMax)
                {
                    Rigidbody.velocity = Rigidbody.velocity.normalized * speedMax;
                }

                Rigidbody.velocity *= 0.8f;
            }).AddTo(this);

            // Jump
            this.FixedUpdateAsObservable().Subscribe(_ =>
            {
                if (IsOnGround && Input.GetKeyDown(jump))
                {
                    Rigidbody.AddForce(Vector2.up * jumpPower);
                }
            }).AddTo(this);

            this.OnCollisionEnter2DAsObservable().Where(n => n.gameObject.tag == "Ground").Subscribe(_ => IsOnGround = true);
            this.OnCollisionExit2DAsObservable().Where(n => n.gameObject.tag == "Ground").Subscribe(_ => IsOnGround = false);
        }
    }
}
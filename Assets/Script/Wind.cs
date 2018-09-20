using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameJam.Sho
{
    public class Wind : MonoBehaviour, ISpawnedByPlayer
    {
        [SerializeField]
        private float lifeTimer = 5.0f;
        [SerializeField]
        private int power = 1;
        [SerializeField]
        private Vector2 offset = new Vector2(2, 0);
        public Vector2 Offset => offset;

        private PlayerStatus PlayerStatus { get; set; } = null;

        void Start()
        {
            this.OnTriggerStay2DAsObservable()
                .Select(n => n.GetComponent<StandObjectOnWind>())
                .Where(n => n != null)
                .Subscribe(n =>
                {
                    var v = new Vector2(0.0f, 1.0f);
                    n.Rigidbody.AddForce(v.normalized * power);
                }).AddTo(this);

            PlayerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
            var se = this.GetComponent<AudioSource>();
            se.Play();
            this.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    se.Stop();
                }).AddTo(this);

            var t = lifeTimer * 0.3f;
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    t -= Time.deltaTime;
                    if(t<=0.0f)
                    {
                        this.transform.localScale *= 0.9999f;
                    }
                }).AddTo(this);
            GameObject.Destroy(this.gameObject, lifeTimer);
        }

        void OnDestroy()
        {
            PlayerStatus.CurrentWindCount--;
        }
    }
}
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

        void Start()
        {
            GameObject.Destroy(this.gameObject, lifeTimer);

            this.OnTriggerStay2DAsObservable()
                .Select(n => n.GetComponent<StandObjectOnWind>())
                .Where(n => n != null)
                .Subscribe(n =>
                {
                    var v = new Vector2(0.0f, 1.0f);
                    n.Rigidbody.AddForce(v.normalized * power);
                }).AddTo(this);

            var se = this.GetComponent<AudioSource>();
            se.Play();
            this.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    se.Stop();
                }).AddTo(this);
        }
    }
}
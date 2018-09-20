using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameJam.Sho
{
    public class Shuriken : MonoBehaviour, ISpawnedByPlayer
    {
        public Rigidbody2D Rigidbody { get; set; } = null;

        [SerializeField]
        private Vector2 offset = new Vector2();
        public Vector2 Offset => offset;

        [SerializeField]
        private GameObject effect = null;

        // Use this for initialization
        void Awake()
        {
            Rigidbody = this.GetComponent<Rigidbody2D>();

            this.OnCollisionEnter2DAsObservable()
                .Select(n => n.gameObject.GetComponent<Status>())
                .Where(n => n != null)
                .Subscribe(n =>
                {
                    n.HP--;

                    var e = GameObject.Instantiate(effect);
                    e.transform.position = this.transform.position;
                    GameObject.Destroy(e.gameObject, 1.0f);
                    GameObject.Destroy(this.gameObject);
                }).AddTo(this);

            this.OnCollisionEnter2DAsObservable()
                .Where(n => n.gameObject.tag == "ObjectDead")
                .Subscribe(_ =>
                {
                    GameObject.Destroy(this.gameObject);
                }).AddTo(this);
        }
    }
}
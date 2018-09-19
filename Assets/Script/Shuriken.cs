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

        private AudioSource hitSound { get; set; } = null;

        // Use this for initialization
        void Awake()
        {
            Rigidbody = this.GetComponent<Rigidbody2D>();
            hitSound = this.GetComponent<AudioSource>();

            this.OnCollisionEnter2DAsObservable()
                .Select(n => n.gameObject.GetComponent<Status>())
                .Where(n => n != null)
                .Subscribe(n =>
                {
                    n.HP--;
                    hitSound.Play();
                    GameObject.Destroy(this.transform.GetChild(0).gameObject);
                    GameObject.Destroy(this.GetComponent<BoxCollider2D>());
                    GameObject.Destroy(this.gameObject, 3.0f);
                }).AddTo(this);

        }
    }
}
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

        // Use this for initialization
        void Awake()
        {
            Rigidbody = this.GetComponent<Rigidbody2D>();
        }
    }
}
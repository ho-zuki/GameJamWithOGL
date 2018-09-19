using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameJam.Sho
{
    public class StandObjectOnWind : MonoBehaviour
    {
        public Rigidbody2D Rigidbody { get; set; } = null;
        [SerializeField]
        private Vector2 jumpPower = new Vector2(0, 100);
        public Vector2 JumpPower => jumpPower;

        void Awake()
        {
            Rigidbody = this.GetComponentInParent<Rigidbody2D>();
        }

        public void OnWindDestroyed()
        {
            this.GetComponentInParent<Player>().IsOnGround = false;
        }
    }
}

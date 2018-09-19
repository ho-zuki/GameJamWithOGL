using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameJam
{
    public class PlayerSpriteController : MonoBehaviour
    {
        private Animator Animator { get; set; } = null;

        [SerializeField]
        private float speed = 1.0f;

        // Use this for initialization
        void Start()
        {
            Animator = this.GetComponent<Animator>();
            Animator.speed = speed;
        }

        public void RunStart()
        {
            Animator.SetBool("Running", true);
        }
        public void RunStop()
        {
            Animator.SetBool("Running", false);
        }
    }
}
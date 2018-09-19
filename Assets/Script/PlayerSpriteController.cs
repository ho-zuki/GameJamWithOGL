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

        private AudioSource[] Audios;

        private AudioSource JumpStartSound => Audios[0];
        private AudioSource JumpEndSound => Audios[1];

        // Use this for initialization
        void Start()
        {
            Animator = this.GetComponent<Animator>();
            Animator.speed = speed;
            Audios = this.GetComponents<AudioSource>();
        }

        public void RunStart()
        {
            Animator.SetBool("Running", true);
        }
        public void RunStop()
        {
            Animator.SetBool("Running", false);
        }
        public void JumpStart()
        {
            if (!IsJumping()) JumpStartSound.Play();
            Animator.SetBool("Jumping", true);
        }
        public void JumpStop()
        {
            if (IsJumping()) JumpEndSound.Play();
            Animator.SetBool("Jumping", false);
        }
        public void ForceJumpStart()
        {
            if (!IsJumping())
            {
                Animator.SetTrigger("PushUp");
                Animator.SetBool("Jumping", true);
            }
        }
        public bool IsJumping()
        {
            return Animator.GetBool("Jumping");
        }

        public void Attack()
        {
            Animator.SetTrigger("Attack");
        }
    }
}
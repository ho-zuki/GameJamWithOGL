﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


namespace GameJam.Sho
{
    public class Enemy : MonoBehaviour
    {
        private enum Attack_Type
        {
            E_WAIT, E_LEFT, E_RIGHT, E_ATTACK_LEFT, E_ATTACK_RIGHT,
        };

        [SerializeField, Header("Rush speed")]
        private float movespeed = 0.01f;

        [SerializeField, Header("Enemy state")]
        private Attack_Type state;

        [SerializeField, Header("Tackle speed")]
        private float tackle_speed = 0.1f;

        private Vector2 pos;
        private bool attack, spawn;
        private float timer;
        private float speed;

        // 大きさ / scale
        private const float e_scale = 0.5f;
        private Vector3 scale;

        // アニメーションフラグ
        ///private bool a_taikiflag, a_run_flag, a_attack_flag;

        // 音声読み込み / Load from sound data.
        private AudioSource[] audios;
        private AudioSource tackle_sound => audios[0];
        private AudioSource destroy_sound => audios[1];

        private Animator animator { get; set; } = null;


        // 初期化 / Initialize
        void Start()
        {
            state = Attack_Type.E_WAIT;
            pos = new Vector2(0, 0);
            transform.Translate(pos.x, pos.y, 0.0f);
            timer = 0;
            attack = false;
            spawn = true;
            scale = new Vector3(e_scale, e_scale, e_scale);
            animator = GetComponent<Animator>();
        }


        void Update()
        {
            switch (state)
            {
                // 待機 / Wait
                case Attack_Type.E_WAIT:
                    animator.SetBool("taiki_flag", true);
                    timer += (Time.deltaTime) * 1.0f;

                    if (timer >= 1.0f)
                    {
                        animator.SetBool("taiki_flag", false);
                        timer = 0;
                        int rnd_dir = Random.Range(0, 1);
                        spawn = false;
                        switch (rnd_dir)
                        {
                            case 0:
                                state = Attack_Type.E_LEFT;
                                break;
                            case 1:
                                state = Attack_Type.E_RIGHT;
                                break;
                        }
                    }
                    break;

                // 左右移動 / For Move
                case Attack_Type.E_LEFT:
                    animator.SetBool("run_flag", true);
                    animator.SetBool("attack_flag", false);
                   
                    scale.x = -e_scale;
                    timer += (Time.deltaTime) * 1.0f;
                    speed = -movespeed;
                    break;

                case Attack_Type.E_RIGHT:
                    animator.SetBool("run_flag", true);
                    animator.SetBool("attack_flag", false);
                    
                    scale.x = e_scale;
                    timer += (Time.deltaTime) * 1.0f;
                    speed = movespeed;
                    break;

                // 攻撃（突進） / For attack（tackle attack）
                case Attack_Type.E_ATTACK_LEFT:
                    animator.SetBool("attack_flag", true);
                    animator.SetBool("run_flag", false);
                    scale.x = -e_scale;
                    timer += (Time.deltaTime) * 1.0f;
                    speed = -tackle_speed;
                    break;

                case Attack_Type.E_ATTACK_RIGHT:
                    animator.SetBool("attack_flag", true);
                    animator.SetBool("run_flag", false);
                    scale.x = e_scale;
                    timer += (Time.deltaTime) * 1.0f;
                    speed = tackle_speed;
                    break;
            }


            // 突進攻撃終了 / Tackle Attack End
            if (attack && timer >= 2.0f)
            {
                // animator.SetBool("attack_flag", false);
                // animator.SetBool("run_flag", true);
                animator.SetBool("attack_flag", false);
                attack = false;
                timer = 0.0f;
                if (state == Attack_Type.E_ATTACK_LEFT) state = Attack_Type.E_LEFT;
                else if (state == Attack_Type.E_ATTACK_RIGHT) state = Attack_Type.E_RIGHT;
            }
            // 移動向き反転
            else if (!attack && (timer >= 3.0f + Random.Range(0, 0.05f)) && !spawn)
            {
                // animator.SetBool("attack_flag", false);
                // animator.SetBool("run_flag", true);

                if (state == Attack_Type.E_LEFT) state = Attack_Type.E_RIGHT;
                else if (state == Attack_Type.E_RIGHT) state = Attack_Type.E_LEFT;
                timer = 0;
                if (attack) attack = false;
            }

            // 画面端反転
            if ((int)transform.position.x <= -10) { timer = 0; state = Attack_Type.E_RIGHT; }
            else if ((int)transform.position.x >= 10) { timer = 0; state = Attack_Type.E_LEFT; }


            // 移動速度更新
            transform.Translate(new Vector2(speed, 0));

            ///if (++timer == 300)
            ///{
            ///    transform.Rotate(0, 0, 0);
            ///    GameObject.Instantiate(this);
            ///    pos.x = pos.y = 0;
            ///    transform.Translate(Random.Range(-5, 5), Random.Range(0, 10), 0.0f);
            ///}

            transform.localScale = scale;
            transform.Translate(pos.x, pos.y, 0.0f);
        }

        //　一定距離近づいたら攻撃
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!attack && !spawn)
            {
                if (collision.gameObject.tag == "Player")
                {
                    if ((this.transform.position.x > collision.gameObject.transform.position.x))
                    {
                        state = Attack_Type.E_ATTACK_LEFT;
                    }
                    else if ((this.transform.position.x < collision.gameObject.transform.position.x))
                    {
                        state = Attack_Type.E_ATTACK_RIGHT;
                    }

                    attack = true;
                    //timer = 0;
                }
                //GameObject.Find("Player");
            }
        }
    }
}

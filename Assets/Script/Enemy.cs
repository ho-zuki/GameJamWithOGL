using System.Collections;
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
            E_LEFT, E_RIGHT, E_TAG_LEFT, E_TAG_RIGHT, E_ATTACK_LEFT, E_ATTACK_RIGHT,
        };

        [SerializeField, Header("Rush speed")]
        private float movespeed = 0.01f;

        private Vector2 pos;

        [SerializeField, Header("Enemy State")]
        private int state;
        private bool attack;

        float timer;
        float speed;


        //　一定距離近づいたら攻撃
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!attack)
            {
                if (collision.gameObject.tag == "Player")
                {
                    if ((this.transform.position.x > collision.gameObject.transform.position.x))
                    {
                        state = (int)(Attack_Type.E_TAG_LEFT);
                    }
                    else if ((this.transform.position.x < collision.gameObject.transform.position.x))
                    {
                        state = (int)(Attack_Type.E_TAG_RIGHT);
                    }
                    attack = true;
                }
                //GameObject.Find("Player");
            }
        }


        // 初期化 / Initialize
        void Start()
        {
            state = 0;
            pos = new Vector2(0, 0);
            transform.Translate(pos.x, pos.y, 0.0f);
            timer = 0;
            attack = false;
        }


        void Update()
        {
            switch (state)
            {
                // 左右移動 / For Move
                case (int)(Attack_Type.E_LEFT):
                    timer += Time.deltaTime;
                    speed = -movespeed;
                    break;

                case (int)(Attack_Type.E_RIGHT):
                    timer += Time.deltaTime;
                    speed = movespeed;
                    break;

                case (int)(Attack_Type.E_TAG_LEFT):
                    timer += Time.deltaTime;
                    speed = -0.1f;
                    break;

                case (int)(Attack_Type.E_TAG_RIGHT):
                    timer += Time.deltaTime;
                    speed = 0.1f;
                    break;

                // 攻撃（突進） / For attack（rush attack）
                case (int)(Attack_Type.E_ATTACK_LEFT):
                    speed = -0.5f;
                    break;

                case (int)(Attack_Type.E_ATTACK_RIGHT):
                    speed = 0.5f;
                    break;
            }


            // 突進攻撃終了 / Rush Attack End
            if (attack && timer >= 1)
            {
                attack = false;
                if (state == (int)(Attack_Type.E_TAG_LEFT)) state = (int)(Attack_Type.E_RIGHT);
                else if (state == (int)(Attack_Type.E_TAG_RIGHT)) state = (int)(Attack_Type.E_LEFT);
            }
            // 移動向き反転
            else if (timer >= 1 + Random.Range(0, 0.05f))
            {
                if (state == (int)(Attack_Type.E_LEFT)) state = (int)(Attack_Type.E_RIGHT);
                else if (state == (int)(Attack_Type.E_RIGHT)) state = (int)(Attack_Type.E_LEFT);
                timer = 0;
            }

            // 画面端反転
            if ((int)transform.position.x <= -10) { timer = 0; state = (int)(Attack_Type.E_RIGHT); }
            else if ((int)transform.position.x >= 10) { timer = 0; state = (int)(Attack_Type.E_LEFT); }


            // 突進 / Rush
            transform.Translate(new Vector2(speed, 0));


            ///if (++timer == 300)
            ///{
            ///    transform.Rotate(0, 0, 0);
            ///    GameObject.Instantiate(this);
            ///    pos.x = pos.y = 0;
            ///    transform.Translate(Random.Range(-5, 5), Random.Range(0, 10), 0.0f);
            ///}

            transform.Translate(pos.x, pos.y, 0.0f);
        }
    }
}

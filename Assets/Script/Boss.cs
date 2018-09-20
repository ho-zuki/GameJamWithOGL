﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Hozuki;
using System;

namespace GameJam.Sho
{
    public class Boss : MonoBehaviour
    {
        enum State
        {
            CallEnemy,
            Lazer,
            Wait,
            Damaged
        }

        [SerializeField, Header("ReadOnly For Debug")]
        private State state = State.Wait;

        [SerializeField]
        private GameObject lazerPrefab = null;
        [SerializeField]
        private GameObject preLazerPrefab = null;
        [SerializeField]
        private float timerRateForExitingWait = 10.0f;
        [SerializeField]
        private float timerRateForLazer = 10.0f;

        // Use this for initialization
        void Start()
        {
            var status = this.GetComponent<Status>();
            GameObject preLazer = GameObject.Instantiate(preLazerPrefab);
            preLazer.SetActive(false);

            var player = GameObject.Find("Player");

            var sounds = this.GetComponents<AudioSource>();
            var animator = this.GetComponentInChildren<Animator>();
            var light = GameObject.Find("BossLight").GetComponent<Light>();

            float waitTimer = 0.0f;
            this.UpdateAsObservable()
                .Where(_ => state == State.Wait)
                .Subscribe(_ =>
                {
                    waitTimer += Time.deltaTime;
                    if (waitTimer >= (status.HP / status.MAXHP + 1.0f) * timerRateForExitingWait)
                    {
                        // 待機の時何もせず一定時間後次のStateに戻す
                        var n = UnityEngine.Random.Range(0, 10);
                        if (n < 5)
                        {
                            state = State.CallEnemy;
                            return;
                        }
                        state = State.Lazer;
                        preLazer.SetActive(true);
                        waitTimer = 0.0f;
                    }
                }).AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => state == State.CallEnemy)
                .Subscribe(_ =>
                {
                    // temp
                    state = State.Wait;
                }).AddTo(this);
            var lazerTimer = 0.0f;
            this.UpdateAsObservable()
                .Where(_ => state == State.Lazer)
                .Subscribe(_ =>
                {
                    lazerTimer += Time.deltaTime;
                    var p = preLazer.transform.position;
                    p.y = Mathf.Lerp(p.y, player.transform.position.y, Time.deltaTime);
                    preLazer.transform.position = p;
                    light.intensity += Time.deltaTime;
                    light.intensity = Mathf.Max(6, light.intensity);
                    if (lazerTimer >= (status.HP / status.MAXHP + 0.5f) * timerRateForLazer)
                    {
                        lazerTimer = 0.0f;
                        state = State.Wait;
                        preLazer.SetActive(false);
                        sounds[0].Play();
                        animator.SetTrigger("Lazer");
                        StartCoroutine(LightDisapp(light));
                        this.DelayMethod(1.5f, () =>
                        {
                            var lazer = GameObject.Instantiate(lazerPrefab);
                            lazer.transform.position = preLazer.transform.position;
                        });
                    }
                }).AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => state == State.Damaged)
                .Subscribe(_ =>
                {

                }).AddTo(this);

            status.DeadEvent
                .Subscribe(_ =>
                {
                    GameObject.Destroy(this.gameObject);
                }).AddTo(this);
        }

        private IEnumerator LightDisapp(Light light)
        {
            while (true)
            {
                light.intensity *= 0.8f;
                yield return new WaitForSecondsRealtime(0.016f);
                if (light.intensity <= 0.0f) break;
            }
        }
    }
}
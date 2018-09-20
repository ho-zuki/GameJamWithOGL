using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Hozuki;
using System;
using DG;
using UnityEngine.SceneManagement;

namespace GameJam.Sho
{
    public class Boss : MonoBehaviour
    {
        enum State
        {
            CallEnemy,
            Lazer,
            Wait,
            Damaged,
            Appear,
            WaitHusuma,
        }

        [SerializeField, Header("ReadOnly For Debug")]
        private State state = State.WaitHusuma;

        [SerializeField]
        private GameObject lazerPrefab = null;
        [SerializeField]
        private GameObject preLazerPrefab = null;
        [SerializeField]
        private float timerRateForExitingWait = 10.0f;
        [SerializeField]
        private float timerRateForLazer = 10.0f;
        [SerializeField]
        private Husuma husumaPrefabs = null;

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
                        this.DelayMethod(1.5f, () =>
                        {
                            var lazer = GameObject.Instantiate(lazerPrefab);
                            lazer.transform.position = preLazer.transform.position;
                            StartCoroutine(LightDisapp(light));
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
                    this.DelayMethod(3.0f, () =>
                    {
                        var h = GameObject.Instantiate(husumaPrefabs).GetComponent<Husuma>();
                        h.transform.SetParent(GameObject.Find("UI").transform, false);
                        h.IsClose = true;
                        h.HusumaCompleteEvent
                        .Subscribe(__ =>
                        {
                            Result.State = ClearState.Win;
                            SceneManager.LoadScene("Result");
                        }).AddTo(this);
                    });
                    GameObject.Destroy(this.GetComponent<Collider2D>());
                }).AddTo(this);

            this.UpdateAsObservable()
               .Where(_ => state == State.Appear)
               .Subscribe(_ =>
               {
                   this.DelayMethod(1.0f, () =>
                   {
                       sounds[1].Play();
                       state = State.Wait;
                   });
               }).AddTo(this);

            GameObject.Find("Husuma").GetComponent<Husuma>().HusumaCompleteEvent
                .Subscribe(_ =>
                {
                    state = State.Wait;
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
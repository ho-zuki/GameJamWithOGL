using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace GameJam.Sho
{
    public class Husuma : MonoBehaviour
    {
        [SerializeField]
        private float speedRate = 1.0f;

        public ISubject<Unit> _HusumaCompleteEvent { get; set; } = new Subject<Unit>();
        public IObservable<Unit> HusumaCompleteEvent => _HusumaCompleteEvent;

        public bool IsClose { get; set; } = true;

        // Use this for initialization
        void Start()
        {
            var L0 = GameObject.Find("L0");
            var L1 = GameObject.Find("L1");
            var L2 = GameObject.Find("L2");
            var R0 = GameObject.Find("R0");
            var R1 = GameObject.Find("R1");
            var R2 = GameObject.Find("R2");

            L0.SetActive(false);
            R0.SetActive(false);
            var L1Pos = L1.transform.position;
            var L2Pos = L2.transform.position;
            var R1Pos = R1.transform.position;
            var R2Pos = R2.transform.position;

            if (IsClose)
            {
                L1.transform.position = L2.transform.position = L0.transform.position;
                R1.transform.position = R2.transform.position = R0.transform.position;
            }

            var t2 = 0.0f;
            var t1 = 0.0f;
            var s = this.GetComponent<AudioSource>();
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    t2 += speedRate * Time.deltaTime;
                    if (!s.isPlaying && t1 <= 1.0f && t2 <= 1.0f)
                    {
                        s.Play();
                    }
                    if (IsClose)
                    {
                        t2 += speedRate * Time.deltaTime;
                        L2.transform.position = Vector3.Lerp(L2.transform.position, L2Pos, t2);
                        R2.transform.position = Vector3.Lerp(R2.transform.position, R2Pos, t2);
                        if (t2 >= 0.5f)
                        {
                            t1 += speedRate * Time.deltaTime;
                            L1.transform.position = Vector3.Lerp(L1.transform.position, L1Pos, t1);
                            R1.transform.position = Vector3.Lerp(R1.transform.position, R1Pos, t1);
                        }
                    }
                    else
                    {
                        L2.transform.position = Vector3.Lerp(L2.transform.position, L0.transform.position, t2);
                        R2.transform.position = Vector3.Lerp(R2.transform.position, R0.transform.position, t2);
                        if (t2 >= 0.5f)
                        {
                            t1 += speedRate * Time.deltaTime;
                            L1.transform.position = Vector3.Lerp(L1.transform.position, L0.transform.position, t1);
                            R1.transform.position = Vector3.Lerp(R1.transform.position, R0.transform.position, t1);
                        }
                    }
                    if (t1 >= 1.0f) _HusumaCompleteEvent.OnNext(Unit.Default);
                }).AddTo(this);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameJam.Sho
{
    public class WindUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject windTPrefab = null;
        [SerializeField]
        private GameObject windFPrefab = null;

        [SerializeField]
        private float interval = 10.0f;

        private List<GameObject> WindsT { get; set; } = new List<GameObject>();
        private List<GameObject> WindsF { get; set; } = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            var playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
            for (int i = 0; i < playerStatus.WindMax; i++)
            {
                var wt = GameObject.Instantiate(windTPrefab);
                var wf = GameObject.Instantiate(windFPrefab);
                var p = this.transform.position;
                p.x += i * interval;
                wt.transform.position = p;
                wt.transform.SetParent(this.transform);
                WindsT.Add(wt);
                wf.transform.position = p;
                wf.transform.SetParent(this.transform);
                wf.SetActive(false);
                WindsF.Add(wf);
            }
            playerStatus.WindCountChangeEvent
                .Subscribe(count =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        WindsT[i].SetActive(true);
                        WindsF[i].SetActive(false);
                    }
                    for (int i = 0; i < count; i++)
                    {
                        WindsT[5 - i - 1].SetActive(false);
                        WindsF[5 - i - 1].SetActive(true);
                    }
                }).AddTo(this);
        }
    }
}
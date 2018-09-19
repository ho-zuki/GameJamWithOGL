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
        private GameObject windPrefab = null;
        [SerializeField]
        private float interval = 10.0f;

        [SerializeField]
        private List<GameObject> Winds = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            var playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
            for (int i = 0; i < playerStatus.WindMax; i++)
            {
                var w = GameObject.Instantiate(windPrefab);
                var p = this.transform.position;
                p.x += i * interval;
                w.transform.position = p;
                w.transform.SetParent(this.transform);
                Winds.Add(w);
            }
            playerStatus.WindCountChangeEvent
                .Subscribe(count =>
                {
                    // 悪い処理。
                    var n = new GameObject[Winds.Count];
                    for (int i = 0; i < n.Length; i++)
                    {
                        n[i] = Winds[i];
                    }
                    for (int i = 0; i < n.Length; i++)
                    {
                        GameObject.Destroy(n[i]);
                    }
                    Winds.Clear();

                    Debug.Log(playerStatus.WindMax - count);
                    for (int i = 0; i < playerStatus.WindMax - count; i++)
                    {
                        var w = GameObject.Instantiate(windPrefab);
                        var p = this.transform.position;
                        p.x += i * interval;
                        w.transform.position = p;
                        w.transform.SetParent(this.transform);
                        Winds.Add(w);
                    }
                }).AddTo(this);
        }
    }
}
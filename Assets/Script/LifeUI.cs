using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameJam.Sho
{
    public class LifeUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject heartPrefab;
        [SerializeField]
        private float interval = 1.0f;

        private List<GameObject> HeartList { get; set; } = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            var status = GameObject.Find("Player").GetComponent<PlayerStatus>();
            for (int i = 0; i < status.HP; i++)
            {
                var heart = GameObject.Instantiate(heartPrefab);
                var p = this.transform.position;
                p.x += interval * i;
                heart.transform.position = p;
                heart.transform.SetParent(this.transform);
                HeartList.Add(heart);
            }
            status.HPChangedEvent
                .Subscribe(currentHP =>
                {
                    int index = 0;
                    for (index = 0; index < currentHP; index++) ;
                    GameObject.Destroy(HeartList[index].gameObject);
                }).AddTo(this);
        }
    }
}
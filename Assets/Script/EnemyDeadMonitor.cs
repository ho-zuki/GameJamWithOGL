using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameJam.Sho
{
    public class EnemyDeadMonitor : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            this.GetComponent<Status>().DeadEvent
                .Subscribe(_ =>
                {
                    GameObject.Destroy(this.gameObject);
                }).AddTo(this);
        }
        private void OnDestroy()
        {
            this.GetComponentInParent<Enemies>().Defeat();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameJam.Sho
{
    public class Wind : MonoBehaviour, ISpawnedByPlayer
    {
        [SerializeField]
        private float lifeTimer = 5.0f;
        [SerializeField]
        private int attackPower = 1;
        [SerializeField]
        private Vector2 offset = new Vector2(2, 0);
        public Vector2 Offset => offset;

        void Start()
        {
            GameObject.Destroy(this.gameObject, lifeTimer);
        }
    }
}
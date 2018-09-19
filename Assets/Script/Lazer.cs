using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;

namespace GameJam.Sho
{
    public class Lazer : MonoBehaviour
    {
        private TrailRenderer TrailRenderer { get; set; } = null;
        private BoxCollider2D Collider2D { get; set; } = null;

        [SerializeField]
        private float speed = 10.0f;


        // Use this for initialization
        void Start()
        {
            TrailRenderer = this.GetComponentInChildren<TrailRenderer>();
            Collider2D = this.GetComponentInChildren<BoxCollider2D>();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    this.transform.position -= Vector3.right * Time.deltaTime * speed;
                    if (TrailRenderer.positionCount <= 0) return;
                    Vector3[] pos = new Vector3[TrailRenderer.positionCount];
                    TrailRenderer.GetPositions(pos);
                    var max = pos.Select(n => (TrailRenderer.localToWorldMatrix * n).x).Max();
                    var min = pos.Select(n => (TrailRenderer.localToWorldMatrix * n).x).Min();

                    Collider2D.offset = new Vector2((max - min) * 0.5f, 0.0f);
                    Collider2D.size = new Vector2(max - min, 1.5f);
                }).AddTo(this);
        }
    }
}
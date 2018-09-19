using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameJam.Sho
{
    public class WindJumpSpot : MonoBehaviour
    {
        private Wind Wind { get; set; } = null;

        private StandObjectOnWind pastTouchObject { get; set; } = null;

        // Use this for initialization
        void Start()
        {
            Wind = this.GetComponentInParent<Wind>();
            this.OnCollisionEnter2DAsObservable()
                .Select(obj => obj.gameObject.GetComponent<StandObjectOnWind>())
                .Subscribe(obj =>
                {
                    pastTouchObject?.OnWindDestroyed();
                    pastTouchObject = obj;
                }).AddTo(this);

            this.OnCollisionExit2DAsObservable()
                .Where(n => pastTouchObject != null)
                .Where(n => n.gameObject == pastTouchObject.gameObject)
                .Subscribe(_ =>
                {
                    pastTouchObject.OnWindDestroyed();
                }).AddTo(this);
        }
    }
}
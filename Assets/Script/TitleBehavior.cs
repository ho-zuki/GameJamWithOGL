using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

namespace GameJam.Sho
{
    public class TitleBehavior : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => Input.anyKeyDown)
                .Subscribe(_ =>
                {
                    // MainScene / temp
                    SceneManager.LoadScene(1);
                }).AddTo(this);
        }
    }
}
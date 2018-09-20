using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

namespace GameJam.Sho
{
    public enum ClearState
    {
        None,
        Win,
        Lose
    }
    public class Result : MonoBehaviour
    {
        public static ClearState State { get; set; } = ClearState.None;

        // Use this for initialization
        void Start()
        {
            var result = GameObject.Find("ResultText").GetComponent<TextMeshProUGUI>();
            if (State == ClearState.Win)
            {
                result.text = "You Win";
                this.GetComponents<AudioSource>()[0].Play();
            }
            if (State == ClearState.Lose)
            {
                result.text = "You Lose";
                this.GetComponents<AudioSource>()[1].Play();
            }
            if (State == ClearState.None)
            {
                result.text = "DebugMode";
            }

            this.UpdateAsObservable()
                .Where(_ => Input.anyKeyDown)
                .Subscribe(_ =>
                {
                    SceneManager.LoadScene(0);
                }).AddTo(this);
        }
    }
}
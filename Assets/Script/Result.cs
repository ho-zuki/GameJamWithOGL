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

        [SerializeField]
        private GameObject husuma;

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

            Husuma h = null;
            this.UpdateAsObservable()
                .Where(_ => h == null)
                .Subscribe(_ =>
                {
                    h = GameObject.Instantiate(husuma).GetComponent<Husuma>();
                    h.transform.SetParent(GameObject.Find("UI").transform, false);
                    h.IsClose = false;
                    h.HusumaCompleteEvent
                        .Subscribe(__ =>
                        {
                            if (State == ClearState.Win)
                            {
                                this.GetComponents<AudioSource>()[0].Play();
                            }
                            if (State == ClearState.Lose)
                            {
                                this.GetComponents<AudioSource>()[1].Play();
                            }
                        }).AddTo(this);
                }).AddTo(this);


            this.UpdateAsObservable()
                .Where(_ => Input.anyKeyDown)
                .Subscribe(_ =>
                {
                    SceneManager.LoadScene(0);
                }).AddTo(this);
        }
    }
}
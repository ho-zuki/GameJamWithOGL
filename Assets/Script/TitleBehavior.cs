using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;
using Hozuki;

namespace GameJam.Sho
{
    public class TitleBehavior : MonoBehaviour
    {
        [SerializeField]
        private GameObject husuma;


        // Use this for initialization
        void Start()
        {
            Husuma h = null;
            this.UpdateAsObservable()
                .Where(_ => Input.anyKeyDown)
                .Where(_ => h == null)
                .Subscribe(_ =>
                {
                    this.DelayMethodInRealTime(1.0f, () =>
                    {
                        h = GameObject.Instantiate(husuma).GetComponent<Husuma>();
                        h.transform.SetParent(GameObject.Find("UI").transform, false);
                        h.IsClose = true;

                        h.HusumaCompleteEvent
                        .Subscribe(__ =>
                        {
                            SceneManager.LoadScene("FirstScene");
                        }).AddTo(this);
                    });
                }).AddTo(this);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

namespace GameJam.Sho
{
    public class Enemies : MonoBehaviour
    {
        private int childrenCount = 0;
        [SerializeField]
        private string nextSceneName = "";

        [SerializeField]
        private GameObject husumaPrefabs = null;

        private Husuma husuma = null;
        // Use this for initialization
        void Start()
        {
            GameObject.Find("Husuma").GetComponent<Husuma>().HusumaCompleteEvent
                .Subscribe(_ =>
                {
                    Time.timeScale = 1.0f;
                }).AddTo(this);
            childrenCount = this.transform.childCount;
        }

        public void Update()
        {
            if (husuma != null) return;
            childrenCount = this.transform.childCount;
            if (childrenCount <= 0)
            {
                husuma = GameObject.Instantiate(husumaPrefabs).GetComponent<Husuma>();
                husuma.transform.SetParent(GameObject.Find("UI").transform, false);
                husuma.IsClose = true;
                husuma.HusumaCompleteEvent
                .Subscribe(__ =>
                {
                    SceneManager.LoadScene(nextSceneName);
                }).AddTo(this);
            }
        }
    }
}
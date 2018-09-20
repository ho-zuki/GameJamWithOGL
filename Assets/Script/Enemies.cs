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

        public void Defeat()
        {
            childrenCount--;
            if (childrenCount <= 0)
            {
                var h = GameObject.Instantiate(husumaPrefabs).GetComponent<Husuma>();
                h.transform.SetParent(GameObject.Find("UI").transform, false);
                h.IsClose = true;
                h.HusumaCompleteEvent
                .Subscribe(__ =>
                {
                    SceneManager.LoadScene(nextSceneName);
                }).AddTo(this);
            }
        }
    }
}
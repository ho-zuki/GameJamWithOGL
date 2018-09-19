using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace GameJam.Sho
{
    public class PlayerStatus : Status
    {
        [SerializeField]
        private int windMax = 5;

        public int WindMax => windMax;

        private int currentWindCount = 0;
        public int CurrentWindCount
        {
            get { return currentWindCount; }
            set
            {
                currentWindCount = value;
                currentWindCount = Mathf.Max(currentWindCount, 0);
                _WindCountChangeEvent.OnNext(currentWindCount);
            }
        }

        private ISubject<int> _WindCountChangeEvent { get; set; } = new Subject<int>();
        public IObservable<int> WindCountChangeEvent => _WindCountChangeEvent;

        // Use this for initialization
        void Start()
        {
            CurrentWindCount = 0;
        }
    }
}
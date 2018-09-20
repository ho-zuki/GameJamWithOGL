using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace GameJam.Sho
{
    public class Status : MonoBehaviour
    {
        [SerializeField]
        private int hp = 10;
        public int HP
        {
            get { return hp; }
            set
            {
                if (IsIncredible) return;
                hp = value;
                hp = Mathf.Max(hp, 0);
                _HpChangedEvent.OnNext(hp);
                if (hp == 0) _DeadEvent.OnNext(Unit.Default);
            }
        }
        public int MAXHP { get; set; } = 0;

        private ISubject<int> _HpChangedEvent { get; set; } = new Subject<int>();
        public IObservable<int> HPChangedEvent => _HpChangedEvent;

        private ISubject<Unit> _DeadEvent { get; set; } = new Subject<Unit>();
        public IObservable<Unit> DeadEvent => _DeadEvent;

        public bool IsIncredible { get; set; } = false;

        // Use this for initialization
        void Awake()
        {
            MAXHP = HP;
        }
        private void Start()
        {
            HP = hp;
        }
    }
}
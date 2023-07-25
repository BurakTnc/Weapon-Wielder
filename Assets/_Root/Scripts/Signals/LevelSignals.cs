using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Root.Scripts.Signals
{
    public class LevelSignals : MonoBehaviour
    {
        public static LevelSignals Instance;
        
        public UnityAction<int> OnXpClaimed = delegate { };
        public UnityAction<Transform> OnNewGangMember = delegate { };
        public UnityAction<List<GameObject>> OnGrid = delegate { };
        public UnityAction OnStop = delegate { };
        public UnityAction<int, int> OnNewGrid = delegate { };
        public UnityAction<int> OnGridLeave = delegate { };
        public UnityAction OnFight = delegate { };
        public UnityAction OnBuySoldier = delegate { };
        public UnityAction OnEnemyKilled = delegate { };
        public UnityAction OnUpgrade = delegate { };

        private void Awake()
        {
            if (Instance!=null && Instance!= this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
    }
}
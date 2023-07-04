using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Root.Scripts.Signals
{
    public class LevelSignals : MonoBehaviour
    {
        public static LevelSignals Instance;
        
        public UnityAction<int> OnXpClaimed = delegate { };
        public UnityAction<Transform> OnNewGangMember = delegate { };

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
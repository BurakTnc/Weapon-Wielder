using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Root.Scripts.Signals
{
    public class CoreGameSignals : MonoBehaviour
    {
        public static CoreGameSignals Instance;

        public UnityAction OnGameStart = delegate { };
        public UnityAction OnLevelComplete = delegate { };
        public UnityAction OnLevelLoad = delegate { };

        private void Awake()
        {
            if (Instance!= this && Instance!=null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
    }
}
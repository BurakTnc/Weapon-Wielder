using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Root.Scripts.Signals
{
    public class CameraSignals : MonoBehaviour
    {
        public static CameraSignals Instance;

        public UnityAction<Vector3> OnMergeLook = delegate { };
        public UnityAction<Vector3> OnFightLook = delegate { };

        private void Awake()
        {
            if (Instance != this && Instance != null) 
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
    }
}
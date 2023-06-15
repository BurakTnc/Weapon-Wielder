using System;
using UnityEngine;

namespace _Root.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public int money;

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
using System;
using _Root.Scripts.Signals;
using UnityEngine;

namespace _Root.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public int money;
        public int earnedMoney;

        private void Awake()
        {
            if (Instance!= this && Instance!=null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            GetValues();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            CoreGameSignals.Instance.OnSave += Save;
            CoreGameSignals.Instance.OnLevelLoad += ArrangeMoney;
            LevelSignals.Instance.OnEnemyKilled += IncreaseEarnedMoney;
        }

        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnSave -= Save;
            CoreGameSignals.Instance.OnLevelLoad -= ArrangeMoney;
            LevelSignals.Instance.OnEnemyKilled -= IncreaseEarnedMoney;
        }

        private void InitializeLevel()
        {
            
        }
        private void Save()
        {
            PlayerPrefs.SetInt("money",money);
        }

        private void GetValues()
        {
            money = PlayerPrefs.GetInt("money", 0);
        }

        private void IncreaseEarnedMoney()
        {
            earnedMoney += 500;
        }

        private void ArrangeMoney()
        {
            money += earnedMoney;
            Save();
            Debug.Log(money);
        }
    }
}
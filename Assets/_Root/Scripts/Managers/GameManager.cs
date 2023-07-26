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
        [SerializeField] private GameObject[] levelPrefabs;
        
        private int _level;

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

        private void Start()
        {
            InitializeLevel();
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
            CoreGameSignals.Instance.OnLevelComplete += LevelWin;
        }

        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnSave -= Save;
            CoreGameSignals.Instance.OnLevelLoad -= ArrangeMoney;
            LevelSignals.Instance.OnEnemyKilled -= IncreaseEarnedMoney;
            CoreGameSignals.Instance.OnLevelComplete -= LevelWin;
        }

        private void InitializeLevel()
        {
            levelPrefabs[_level].SetActive(true);
        }
        private void Save()
        {
            PlayerPrefs.SetInt("money",money);
            PlayerPrefs.SetInt("level",_level);
        }

        private void GetValues()
        {
            money = PlayerPrefs.GetInt("money", 0);
            _level = PlayerPrefs.GetInt("level", 0);
        }

        private void IncreaseEarnedMoney()
        {
            earnedMoney += 10;
        }

        private void ArrangeMoney()
        {
            money += earnedMoney;
            Save();
        }

        private void LevelWin()
        {
            _level++;
            if (_level>4)
            {
                _level = 0;
            }
            Save();
        }
    }
}
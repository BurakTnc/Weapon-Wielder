using System;
using _Root.Scripts.Signals;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Root.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private GameObject gamePanel, startPanel, winPanel, mergePanel, fightPanel;
        [SerializeField] private Image xpBar;

        private void Awake()
        {
            if (Instance!=this && Instance!= null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void UnSubscribe()
        {
            //CoreGameSignals.Instance.OnLevelComplete -= Completed;
        }

        private void Subscribe()
        {
           // CoreGameSignals.Instance.OnLevelComplete += Completed;
        }

        private void Completed()
        {
            gamePanel.SetActive(false);
            winPanel.SetActive(true);
        }
        public void UpdateXp(int xp)
        {
            var amount = xp / 100;
            xpBar.DOFillAmount(amount, .4f);
        }
        public void StartButton()
        {
            startPanel.SetActive(false);
            gamePanel.SetActive(true);
            CoreGameSignals.Instance.OnGameStart?.Invoke();
        }

        public void NextLevelButton()
        {
            CoreGameSignals.Instance.OnLevelLoad?.Invoke();
        }

        public void FightButton()
        {
            LevelSignals.Instance.OnFight?.Invoke();
        }
    }
}
using System;
using _Root.Scripts.Signals;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Root.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private GameObject gamePanel, startPanel, winPanel, mergePanel, fightPanel;
        [SerializeField] private Image xpBar;
        [SerializeField] private TextMeshProUGUI[] moneyTexts;
        [SerializeField] private TextMeshProUGUI earnedMoneyText;
        [SerializeField] private GameObject mergeTutorial;

        private int _tutorialSeen;

        private void Awake()
        {
            if (Instance!=this && Instance!= null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            _tutorialSeen = PlayerPrefs.GetInt("tutorialSeen", 0);
        }

        private void Start()
        {
            SetMoneyTexts();
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
            CoreGameSignals.Instance.OnLevelComplete -= Completed;
            CoreGameSignals.Instance.OnSave -= SetMoneyTexts;
            LevelSignals.Instance.OnStop -= Merge;
        }

        private void Subscribe()
        {
            CoreGameSignals.Instance.OnLevelComplete += Completed;
            CoreGameSignals.Instance.OnSave += SetMoneyTexts;
            LevelSignals.Instance.OnStop += Merge;
        }

        private void SetMoneyTexts()
        {
            var currentMoney = GameManager.Instance.money;
            foreach (var tmp in moneyTexts)
            {
                tmp.text = currentMoney.ToString();
            }
        }

        private void SetEarnedMoneyText()
        {
            var earnedMoney = GameManager.Instance.earnedMoney;

            earnedMoneyText.text = earnedMoney.ToString();
        }
        private void Completed()
        {
            gamePanel.SetActive(false);
            winPanel.SetActive(true);
            SetEarnedMoneyText();
            CoreGameSignals.Instance.OnSave?.Invoke();
        }

        private void Merge()
        {
            mergePanel.SetActive(true);
            PlayerPrefs.SetInt("tutorialSeen",1);
            if (_tutorialSeen == 0) 
            {
                if(!mergeTutorial)
                    return;
                mergeTutorial.SetActive(true);
            }
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
            HapticManager.Instance.PlaySelectionHaptic();
        }

        public void NextLevelButton()
        {
            CoreGameSignals.Instance.OnLevelLoad?.Invoke();
            HapticManager.Instance.PlaySelectionHaptic();
        }

        public void FightButton()
        {
            mergePanel.SetActive(false);
            LevelSignals.Instance.OnFight?.Invoke();
            HapticManager.Instance.PlaySelectionHaptic();
        }

        public void BuySoldierButton()
        {
            LevelSignals.Instance.OnBuySoldier?.Invoke();
        }
    }
}
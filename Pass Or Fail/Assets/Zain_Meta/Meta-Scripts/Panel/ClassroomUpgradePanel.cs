using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.Panel
{
    public class ClassroomUpgradePanel : MonoBehaviour
    {
        public static ClassroomUpgradePanel Instance;
        [SerializeField] private CanvasGroup panel;

        private Action _savingAction;

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private Button optionA, optionB, optionC;
        [SerializeField] private Text levelText;


        private void OnEnable()
        {
            Callbacks.OnRewardClassroomUpgrade += ApplyTheRewardedOption;
        }

        private void OnDisable()
        {
            Callbacks.OnRewardClassroomUpgrade -= ApplyTheRewardedOption;
        }

        private void Start()
        {
            panel.HideCanvas();
        }

        public void PopulateThePanel(UnityAction firstOption, UnityAction secondOption, UnityAction thirdOption,
            Action applyingSettings, int levelToUpgradeTo)
        {
            _savingAction = null;
            optionA.onClick.AddListener(firstOption);
            optionB.onClick.AddListener(secondOption);
            optionC.onClick.AddListener(thirdOption);
            levelText.text = "LEVEL " + levelToUpgradeTo;
            _savingAction = applyingSettings;
            panel.ShowCanvas();
        }

        public void ApplyAndCloseWithOptionA()
        {
            optionA.onClick.Invoke();
            panel.HideCanvas();
            optionA.onClick.RemoveAllListeners();
            optionB.onClick.RemoveAllListeners();
            optionC.onClick.RemoveAllListeners();
            EventsManager.ClassReadyToUpgradeEvent(null, false);
            XpManager.Instance.AddXp(3);
        }

        public void ApplyAndCloseWithOptionB()
        {
            optionB.onClick.Invoke();
            panel.HideCanvas();
            optionA.onClick.RemoveAllListeners();
            optionB.onClick.RemoveAllListeners();
            optionC.onClick.RemoveAllListeners();
            EventsManager.ClassReadyToUpgradeEvent(null, false);
            XpManager.Instance.AddXp(3);
        }

        public void ApplyOptionC()
        {
            optionC.onClick.Invoke();
            Callbacks.rewardType = Callbacks.RewardType.RewardClassroomUpgradeInMeta;
            AdsCaller.Instance.ShowRewardedAd();
        }

        private void ApplyTheRewardedOption()
        {
            _savingAction?.Invoke();
            HideThePanel();
        }

        private void HideThePanel()
        {
            panel.HideCanvas();
            optionA.onClick.RemoveAllListeners();
            optionB.onClick.RemoveAllListeners();
            optionC.onClick.RemoveAllListeners();
            EventsManager.ClassReadyToUpgradeEvent(null, false);
            XpManager.Instance.AddXp(5);
        }
    }
}
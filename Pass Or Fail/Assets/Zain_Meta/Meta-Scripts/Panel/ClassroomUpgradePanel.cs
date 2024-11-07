using System;
using DG.Tweening;
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

        [SerializeField] private Panel[] panelSelection;
        [SerializeField] private Button optionA, optionB, optionC;
        [SerializeField] private Image renderA, renderB, renderC;
        [SerializeField] private Text levelText;
        [SerializeField] private float selectionVal, deSelectionVal;


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
            Action applyingSettings, int levelToUpgradeTo,Sprite spriteA,Sprite spriteB,Sprite spriteC)
        {
            AddingListeners();
            _savingAction = null;
            renderA.sprite = spriteA;
            renderB.sprite = spriteB;
            renderC.sprite = spriteC;
            optionA.onClick.AddListener(firstOption);
            optionB.onClick.AddListener(secondOption);
            optionC.onClick.AddListener(thirdOption);
            var levelToShow = levelToUpgradeTo + 1;
            levelText.text = "LEVEL " + levelToShow;
            _savingAction = applyingSettings;
            panel.ShowCanvas();
            DeSelectAll();
        }

        private void AddingListeners()
        {
            optionA.onClick.AddListener(ShowFirst);
            optionB.onClick.AddListener(ShowSecond);
            optionC.onClick.AddListener(ShowThird);
        }

        private void ShowFirst()
        {
            DeSelectAll();
            panelSelection[0].SelectMe(true, selectionVal);
        }

        private void ShowSecond()
        {
            DeSelectAll();
            panelSelection[1].SelectMe(true, selectionVal);
        }

        private void ShowThird()
        {
            DeSelectAll();
            panelSelection[2].SelectMe(true, selectionVal);
        }

        private void DeSelectAll()
        {
            for (var i = 0; i < panelSelection.Length; i++)
            {
                panelSelection[i].SelectMe(false, deSelectionVal);
            }
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
            _savingAction?.Invoke();
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
            _savingAction?.Invoke();
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

    [Serializable]
    public struct Panel
    {
        public RectTransform selectionPivot;
        public GameObject selectionOutline;

        public void SelectMe(bool val, float moveVal)
        {
            DOTween.Kill(selectionPivot);
            selectionPivot.DOAnchorPosY(moveVal, .25f);
            selectionOutline.SetActive(val);
        }
    }
}
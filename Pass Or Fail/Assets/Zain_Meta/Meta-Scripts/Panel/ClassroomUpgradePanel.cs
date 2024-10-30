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

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private Button optionA, optionB, optionC;
        [SerializeField] private Text levelText;

        private void Start()
        {
            panel.HideCanvas();
        }

        public void PopulateThePanel(UnityAction firstOption, UnityAction secondOption, UnityAction thirdOption,
            int levelToUpgradeTo)
        {
            optionA.onClick.AddListener(firstOption);
            optionB.onClick.AddListener(secondOption);
            optionC.onClick.AddListener(thirdOption);
            levelText.text = "LEVEL :" + levelToUpgradeTo;
            panel.ShowCanvas();
        }   

        public void ApplyAndCloseThePanel()
        {
            panel.HideCanvas();
            optionA.onClick.RemoveAllListeners();
            optionB.onClick.RemoveAllListeners();
            optionC.onClick.RemoveAllListeners();
            EventsManager.ClassReadyToUpgradeEvent(null, false);
        }
    }
}
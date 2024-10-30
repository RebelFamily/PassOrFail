using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.DataRelated;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.Panel;

namespace Zain_Meta.Meta_Scripts.MetaRelated.Upgrades
{
    public class ClassroomUpgradeProfile : MonoBehaviour, IUnlocker
    {
        [SerializeField] private ItemsName fileName;
        [SerializeField] private UpgradeData upgradeData;
        [SerializeField] private int _curUpgradeLevel, _curLevelIndex;
        [SerializeField] private Transform scalingPivot;
        [SerializeField] private Transform upgradingPivot;
        private ClassroomUpgradePanel _upgradePanel;
        private GameObject _curSpawnedUpgrade;
        private const string path = "Classrooms/Class_Maths";

        private string _fileString;

        private void Awake()
        {
            _fileString = "GameData/Upgrades/" + fileName + ".es3";
            LoadData();
        }

        private void LoadData()
        {
            _fileString = "GameData/Upgrades/" + fileName + ".es3";
            upgradeData = ES3.Load(upgradeData.saveKey, _fileString, upgradeData);
            _curUpgradeLevel = upgradeData.upgradedLevel;
            _curLevelIndex = upgradeData.upgradeIndex;
            ApplyMeshes();
        }

        private void Start()
        {
            _upgradePanel = ClassroomUpgradePanel.Instance;
        }

        public void UnlockWithAnimation()
        {
            _curUpgradeLevel = upgradeData.upgradedLevel;
            _curLevelIndex = upgradeData.upgradeIndex;
            _upgradePanel.PopulateThePanel(ApplyFirstUpgrade, ApplySecondUpgrade, ApplyThirdUpgrade, _curUpgradeLevel);
            ApplyFirstUpgrade();
            EventsManager.ClassReadyToUpgradeEvent(this, true);
        }

        public void UnlockWithoutAnimation()
        { }

        public void KeepItLocked()
        { }

        private void ApplyFirstUpgrade()
        {
            _curLevelIndex = 0;
            ApplyMeshes();
        }

        private void ApplySecondUpgrade()
        {
            _curLevelIndex = 1;
            ApplyMeshes();
        }
        private void ApplyThirdUpgrade()
        {
            _curLevelIndex = 2;
            ApplyMeshes();
        }
        
        private void ApplyMeshes()
        {
            if (_curSpawnedUpgrade)
                Destroy(_curSpawnedUpgrade);
            _curSpawnedUpgrade = Instantiate(Resources.Load<GameObject>
                (path + "_" + _curUpgradeLevel + "_" + _curLevelIndex), scalingPivot);
            DOTween.Kill(scalingPivot);
            var localScale = scalingPivot.localScale;
            localScale.y = 0.1f;
            scalingPivot.localScale = localScale;
            scalingPivot.DOScaleY(1, .25f);
            SaveTheData();
        }

        private void SaveTheData()
        {
            upgradeData.upgradedLevel = _curUpgradeLevel;
            upgradeData.upgradeIndex = _curLevelIndex;
            ES3.Save(upgradeData.saveKey, upgradeData, _fileString);
        }

        public Transform GetUpgradingCameraPos() => upgradingPivot;
    }
}
using Cinemachine;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.Panel;

namespace Zain_Meta.Meta_Scripts.MetaRelated.Upgrades
{
    public class ClassroomUpgradeProfile : MonoBehaviour, IUnlocker
    {
        [SerializeField] private MeshLoader[] meshesToUpgrade;
        [SerializeField] private CinemachineVirtualCamera roomCamera;
        [SerializeField] private int _curUpgradeLevel, _curLevelIndex;
        private ClassroomUpgradePanel _upgradePanel;

        public int GetCurLevel() => _curUpgradeLevel;
        public int GetCurIndex() => _curLevelIndex;
        
        private void Start()
        {
            _upgradePanel = ClassroomUpgradePanel.Instance;
            roomCamera.m_Priority = 1;
         
        }

        public void ReloadTheUpgrade(int level,int index)
        {
            _curUpgradeLevel = level;
            _curLevelIndex = index;
            ApplyMeshes();
        }

        private void OnEnable()
        {
            EventsManager.OnClassroomUpgraded += ResetTheCamera;
        }

        private void OnDisable()
        {
            EventsManager.OnClassroomUpgraded -= ResetTheCamera;
        }

        private void ResetTheCamera()
        {
            roomCamera.m_Priority = 1;
        }

        public void UnlockWithAnimation()
        {
            roomCamera.m_Priority = 30;
            _upgradePanel.PopulateThePanel(ApplyFirstUpgrade, ApplySecondUpgrade, ApplyThirdUpgrade, _curUpgradeLevel);
        }

        public void UnlockWithoutAnimation()
        {
            roomCamera.m_Priority = 1;
        }

        public void KeepItLocked()
        {
            roomCamera.m_Priority = 1;
        }

        private void ApplyFirstUpgrade()
        {
            _curUpgradeLevel = 0;
            _curLevelIndex = 0;
            ApplyMeshes();
        }


        private void ApplySecondUpgrade()
        {
            _curUpgradeLevel = 1;
            _curLevelIndex = 0;
            ApplyMeshes();
        }

        private void ApplyThirdUpgrade()
        {
            _curUpgradeLevel = 2;
            _curLevelIndex = 0;
            ApplyMeshes();
        }


        private void ApplyMeshes()
        {
            for (var i = 0; i < meshesToUpgrade.Length; i++)
            {
                meshesToUpgrade[i].LoadTheMesh(_curUpgradeLevel, _curLevelIndex);
            }
        }
    }
}
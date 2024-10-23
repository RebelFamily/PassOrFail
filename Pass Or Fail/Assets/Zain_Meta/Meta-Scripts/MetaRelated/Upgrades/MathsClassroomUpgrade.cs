using Cinemachine;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.Panel;

namespace Zain_Meta.Meta_Scripts.MetaRelated.Upgrades
{
    public class MathsClassroomUpgrade : MonoBehaviour, IUnlocker
    {
        [SerializeField] private CinemachineVirtualCamera roomCamera;
        [SerializeField] private GameObject[] roomVariations;
        [SerializeField] private int curLevel;
        private int _curRoomIndex;
        private ClassroomUpgradePanel _upgradePanel;

        private void Start()
        {
            _upgradePanel = ClassroomUpgradePanel.Instance;
            roomCamera.m_Priority = 1;
            roomVariations[_curRoomIndex].SetActive(true);
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
            _upgradePanel.PopulateThePanel(ApplyFirstUpgrade, ApplySecondUpgrade, ApplyThirdUpgrade, curLevel);
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
            roomVariations[_curRoomIndex].SetActive(false);
            _curRoomIndex = 0;
            roomVariations[_curRoomIndex].SetActive(true);
        }

        private void ApplySecondUpgrade()
        {
            roomVariations[_curRoomIndex].SetActive(false);
            _curRoomIndex = 1;
            roomVariations[_curRoomIndex].SetActive(true);
        }

        private void ApplyThirdUpgrade()
        {
            roomVariations[_curRoomIndex].SetActive(false);
            _curRoomIndex = 2;
            roomVariations[_curRoomIndex].SetActive(true);
        }
    }
}
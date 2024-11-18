using Cinemachine;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.MetaRelated.Upgrades;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class UpgradingCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera upgradingCamera;

        private void OnEnable()
        {
            EventsManager.OnClassReadyToUpgrade += EnableTheCamera;
        }


        private void OnDisable()
        {
            EventsManager.OnClassReadyToUpgrade -= EnableTheCamera;
        }


        private void EnableTheCamera(ClassroomUpgradeProfile classroomUpgradeProfile, bool toUpgrade)
        {
            if (!toUpgrade)
            {
                upgradingCamera.m_Priority = 1;
                return;
            }

            upgradingCamera.m_Follow = classroomUpgradeProfile.GetUpgradingCameraPos();
            upgradingCamera.m_LookAt = classroomUpgradeProfile.GetUpgradingCameraPos();
            upgradingCamera.m_Priority = 20;
        }
    }
}
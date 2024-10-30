using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera playerCamera, switchingCamera;

        private void Start()
        {
            switchingCamera.m_Priority = 1;
        }

        private bool _switching;

        public void ZoomToTarget(Transform target)
        {
            if (_switching || !target) return;

            EventsManager.SwitchTheCameraEvent(true);
            _switching = true;

            switchingCamera.Follow = target;
            switchingCamera.LookAt = target;
            var oldPriority = playerCamera.m_Priority;
            playerCamera.m_Priority = 1;
            switchingCamera.m_Priority = 100;
            DOVirtual.DelayedCall(1.5f, () =>
            {
                EventsManager.SwitchTheCameraEvent(false);
                playerCamera.m_Priority = oldPriority;
                switchingCamera.m_Priority = 1;
                _switching = false;
            });
        }

        public void ZoomToTarget(Transform target, float zoomTime)
        {
            if (_switching || !target) return;

            _switching = true;

            EventsManager.SwitchTheCameraEvent(true);
            switchingCamera.Follow = target;
            switchingCamera.LookAt = target;
            var oldPriority = playerCamera.m_Priority;
            playerCamera.m_Priority = 1;
            switchingCamera.m_Priority = 100;
            DOVirtual.DelayedCall(zoomTime, () =>
            {
                EventsManager.SwitchTheCameraEvent(false);
                playerCamera.m_Priority = oldPriority;
                switchingCamera.m_Priority = 1;
                _switching = false;
            });
        }

        public void StopSwitching()
        {
            if (_switching)
                _switching = false;
        }
    }
}
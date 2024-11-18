using System;
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

        public void ZoomToTarget(Transform target, bool playWithSound = false)
        {
            if (_switching || !target) return;

            EventsManager.SwitchTheCameraEvent(true);
            _switching = true;
            switchingCamera.Follow = target;
            switchingCamera.LookAt = target;
            var oldPriority = playerCamera.m_Priority;
            playerCamera.m_Priority = 1;
            switchingCamera.m_Priority = 100;
            if(playWithSound)
                AudioManager.Instance.PlaySound("CameraWoosh");
            DOVirtual.DelayedCall(1.75f, () =>
            {
                EventsManager.SwitchTheCameraEvent(false);
                playerCamera.m_Priority = oldPriority;
                switchingCamera.m_Priority = 1;
                _switching = false;
                if(playWithSound)
                    AudioManager.Instance.PlaySound("CameraWoosh");
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
    }
}
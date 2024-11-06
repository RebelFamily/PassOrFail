using System.Collections;
using Cinemachine;
using UnityEngine;
using CameraSwitcher = Zain_Meta.Meta_Scripts.Helpers.CameraSwitcher;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        private void Awake()
        {
            Instance = this;
        }


        [SerializeField] private CameraSwitcher cameraSwitcher;
        [SerializeField] private CinemachineVirtualCamera breakRoomCamera, staffRoomCamera, playerCamera;

        private void OnEnable()
        {
            EventsManager.OnClickedCoffeeButton += NavigateToBreakRoom;
        }

        private void OnDisable()
        {
            EventsManager.OnClickedCoffeeButton -= NavigateToBreakRoom;
        }

        private void NavigateToBreakRoom()
        {
            StartCoroutine(nameof(Switching_CO));
        }

        private IEnumerator Switching_CO()
        {
            EventsManager.SwitchTheCameraEvent(true);
            playerCamera.m_Priority = 1;
            breakRoomCamera.m_Priority = 100;
            AudioManager.Instance.PlaySound("CameraWoosh");
            yield return new WaitForSeconds(1.25f);
            breakRoomCamera.m_Priority = 1;
            staffRoomCamera.m_Priority = 100;
            yield return new WaitForSeconds(1.25f);
            playerCamera.m_Priority = 10;
            AudioManager.Instance.PlaySound("CameraWoosh");
            staffRoomCamera.m_Priority = 1;
            breakRoomCamera.m_Priority = 1;
            EventsManager.SwitchTheCameraEvent(false);
        }

        public void SetCameraTarget(Transform target)
        {
            cameraSwitcher.ZoomToTarget(target);
        }

        public void SetCameraTarget(Transform target, float time)
        {
            cameraSwitcher.ZoomToTarget(target, time);
        }
    }
}
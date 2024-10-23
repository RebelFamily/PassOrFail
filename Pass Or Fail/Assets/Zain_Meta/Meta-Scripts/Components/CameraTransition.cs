using Cinemachine;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class CameraTransition : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cmCamera, leftCamera, rightCamera;

        private void OnEnable()
        {
            EventsManager.OnEnteredClassroom += TransitionTheCamera;
        }

        private void OnDisable()
        {
            EventsManager.OnEnteredClassroom -= TransitionTheCamera;
        }

        private void TransitionTheCamera(bool isLeftSide, bool hasEntered)
        {
            if (!hasEntered)
            {
                cmCamera.m_Priority = 10;
                rightCamera.m_Priority = 1;
                leftCamera.m_Priority = 1;
                return;
            }

            if (isLeftSide)
            {
                rightCamera.m_Priority = 1;
                leftCamera.m_Priority = 20;
            }
            else
            {
                rightCamera.m_Priority = 20;
                leftCamera.m_Priority = 1;
            }
        }
    }
}
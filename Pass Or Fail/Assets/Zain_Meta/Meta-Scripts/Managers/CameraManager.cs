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
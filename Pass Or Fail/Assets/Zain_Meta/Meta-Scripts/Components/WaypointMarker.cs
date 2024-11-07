using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class WaypointMarker : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        public Transform markerPivot;
        public Transform target;
        public Transform arrowPivot;

        private void LateUpdate()
        {
            if (!target) return;
            UpdateMarker();
            CheckForVisibility();
        }

        private void CheckForVisibility()
        {
            arrowPivot.position = Vector3.Lerp(arrowPivot.position, target.position, 20 * Time.deltaTime);
            if (!mainCam) return;
            var screenPoint = mainCam.WorldToViewportPoint(target.position);
            if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
                markerPivot.gameObject.SetActive(false);

            else
                markerPivot.gameObject.SetActive(true);
        }

        private void UpdateMarker()
        {
            var position = target.position;
            var targetPos = new Vector3(position.x,
                markerPivot.position.y,
                position.z);
            markerPivot.transform.LookAt(targetPos);
        }
    }
}
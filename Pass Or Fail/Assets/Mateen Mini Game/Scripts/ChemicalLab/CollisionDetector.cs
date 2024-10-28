using UnityEngine;

namespace PassOrFail.MiniGames
{
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField] private Variables.ColorsName acceptedColor;
        [SerializeField] private bool isLastCollider;
        [SerializeField] private bool isToDetectMultiplesTimes;
        private bool _isDetectionStopped;
        private void OnTriggerEnter(Collider other)
        {
            if(_isDetectionStopped) return;
            if (other.TryGetComponent(out CollisionTag od))
            {
                if (!isToDetectMultiplesTimes) _isDetectionStopped = true;
                if(isLastCollider) Debug.Log("last collider enter: "+od.gameObject,od.gameObject);
                EventManager.InvokeBoundaryEnter(acceptedColor,isLastCollider);
            }
        }
    }
}
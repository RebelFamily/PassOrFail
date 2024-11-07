using UnityEngine;

namespace PassOrFail.MiniGames
{
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField] private Variables.ColorsName acceptedColor;
        [SerializeField] private bool isLastCollider;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CollisionTag od))
            {
                EventManager.InvokeBoundaryEnter(acceptedColor,isLastCollider);
            }
        }
    }
}
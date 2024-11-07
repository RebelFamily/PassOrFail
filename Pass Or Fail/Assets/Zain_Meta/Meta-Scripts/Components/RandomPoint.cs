using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class RandomPoint : MonoBehaviour
    {
        [SerializeField] private Transform point;
        [SerializeField] private bool isOccupied;

        private void Awake()
        {
            point = transform;
        }

        public void OccupyThis()
        {
            isOccupied = true;
        }

        public void EmptyThis()
        {
            isOccupied = false;
        }

        public bool IsEmpty()
        {
            return !isOccupied;
        }

        public Transform GetPoint() => point;
    }
}
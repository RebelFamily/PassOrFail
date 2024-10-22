using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class SeatProfile : MonoBehaviour
    {
        [SerializeField] private Transform reachingPoint;
        [SerializeField] private Transform sittingPoint;
        [SerializeField] private bool isOccupied;

        private bool _isMarked;
        public bool IsSeatOccupied() => isOccupied;
        public bool MarkForSitting() => _isMarked;
    }
}
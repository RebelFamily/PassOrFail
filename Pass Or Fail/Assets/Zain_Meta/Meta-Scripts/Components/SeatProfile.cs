using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class SeatProfile : MonoBehaviour
    {
        [SerializeField] private Transform reachingPoint;
        [SerializeField] private Transform sittingPoint;
        [SerializeField] private bool isOccupied;

        public bool IsSeatOccupied() => isOccupied;
        public void MarkForSitting() => isOccupied = true;
    }
}
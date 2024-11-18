using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class PlacingTrigger : MonoBehaviour
    {
        [SerializeField] private Transform rewardedPlacingPosition;


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ArcadeMovement _))
            {
                EventsManager.TriggerWithRoomEvent(rewardedPlacingPosition);
            }
        }
    }
}
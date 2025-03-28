﻿using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class ClassroomTrigger : MonoBehaviour
    {
        [SerializeField] private Transform rewardedPlacingPosition;
        [SerializeField] private bool isLeftSided;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ArcadeMovement _))
            {
                EventsManager.EnteredClassroomEvent(isLeftSided, true);
                EventsManager.TriggerWithRoomEvent(rewardedPlacingPosition);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ArcadeMovement _))
            {
                EventsManager.EnteredClassroomEvent(isLeftSided, false);
            }
        }
    }
}
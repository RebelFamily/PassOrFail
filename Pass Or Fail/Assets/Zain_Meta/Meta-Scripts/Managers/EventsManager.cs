using System;
using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class EventsManager : MonoBehaviour
    {
        public static event Action<bool,bool> OnEnteredClassroom;
        public static event Action<bool,Vector3,Vector3> OnTriggerTeaching;
        public static event Action OnClassroomUpgraded;

        public static void EnteredClassroomEvent(bool isLeftClassroom,bool hasEntered)
        {
            OnEnteredClassroom?.Invoke(isLeftClassroom,hasEntered);
        }

        public static void TriggerTeachingEvent(bool toTeach,Vector3 positionToMoveTo,Vector3 rotationToUse)
        {
            OnTriggerTeaching?.Invoke(toTeach,positionToMoveTo,rotationToUse);
        }

        public static void ClassroomUpgradedEvent()
        {
            OnClassroomUpgraded?.Invoke();
        }
    }
}
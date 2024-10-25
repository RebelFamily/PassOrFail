using System;
using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;
using Zain_Meta.Meta_Scripts.Components;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class EventsManager : MonoBehaviour
    {
        public static event Action<bool,bool> OnEnteredClassroom;
        public static event Action<bool,Vector3,Vector3> OnTriggerTeaching;
        public static event Action<ClassroomProfile> OnTeacherStartTeaching;
        public static event Action OnClassroomUpgraded;
        public static event Action OnClassroomUnlocked;
        public static event Action<StudentStateManager> OnStudentLeftTheClassroom;
        public static event Action OnStudentSatInClass;
        

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

        public static void StudentSatInClassEvent()
        {
            OnStudentSatInClass?.Invoke();
        }

        public static void TeacherStartTeachingEvent(ClassroomProfile obj)
        {
            OnTeacherStartTeaching?.Invoke(obj);
        }

        public static void StudentLeftTheClassroomEvent(StudentStateManager student)
        {
            OnStudentLeftTheClassroom?.Invoke(student);
        }

        public static void ClassroomUnlockedEvent()
        {
            OnClassroomUnlocked?.Invoke();
        }
    }
}
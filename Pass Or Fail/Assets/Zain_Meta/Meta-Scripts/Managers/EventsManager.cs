using System;
using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.MetaRelated;
using Zain_Meta.Meta_Scripts.MetaRelated.Upgrades;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class EventsManager : MonoBehaviour
    {
        public static event Action<bool,bool> OnEnteredClassroom;
        public static event Action<bool,Vector3,Vector3> OnTriggerTeaching;
        public static event Action<ClassroomProfile> OnTeacherStartTeaching;
        public static event Action<IPurchase> OnItemUnlocked;
        public static event Action<ClassroomUpgradeProfile,bool> OnClassReadyToUpgrade;
        public static event Action OnClassroomUnlocked;
        public static event Action<StudentStateManager> OnStudentLeftTheClassroom;
        public static event Action OnStudentSatInClass;
        public static event Action<bool> OnSwitchTheCamera;
        public static event Action OnClickedCoffeeButton;
        public static event Action<bool> OnTeacherEnterSleepyState;
        public static event Action OnStudentStateUpdated;
        public static event Action OnStudentAdmitted;
        public static event Action<StudentRequirements> OnStudentLeftTheSchool;
        

        public static void EnteredClassroomEvent(bool isLeftClassroom,bool hasEntered)
        {
            OnEnteredClassroom?.Invoke(isLeftClassroom,hasEntered);
        }

        public static void TriggerTeachingEvent(bool toTeach,Vector3 positionToMoveTo,Vector3 rotationToUse)
        {
            OnTriggerTeaching?.Invoke(toTeach,positionToMoveTo,rotationToUse);
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

        public static void SwitchTheCameraEvent(bool obj)
        {
            OnSwitchTheCamera?.Invoke(obj);
        }

        public static void StudentStateUpdatedEvent()
        {
            OnStudentStateUpdated?.Invoke();
        }

        public static void StudentLeftTheSchoolEvent(StudentRequirements student)
        {
            OnStudentLeftTheSchool?.Invoke(student);
        }

        public static void ClassReadyToUpgradeEvent(ClassroomUpgradeProfile obj,bool val)
        {
            OnClassReadyToUpgrade?.Invoke(obj,val);
        }

        public static void ItemUnlockedEvent(IPurchase obj)
        {
            OnItemUnlocked?.Invoke(obj);
        }

        public static void ClickedCoffeeButtonEvent()
        {
            OnClickedCoffeeButton?.Invoke();
        }

        public static void TeacherEnteredSleepyStateEvent(bool hasEntered)
        {
            OnTeacherEnterSleepyState?.Invoke(hasEntered);
        }

        public static void StudentAdmitEvent()
        {
            OnStudentAdmitted?.Invoke();
        }
    }
}
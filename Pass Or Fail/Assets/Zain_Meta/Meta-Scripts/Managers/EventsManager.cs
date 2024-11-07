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
        public static event Action<bool,Vector3,Vector3,ClassroomProfile> OnTriggerTeaching;
        public static event Action<bool,ClassroomProfile> OnShowBoardText;
        public static event Action<ClassroomProfile,bool> OnTeacherStartTeaching;
        public static event Action<IPurchase> OnItemUnlocked;
        public static event Action<Transform> OnTriggeredWithRoom;
        public static event Action<ClassroomUpgradeProfile,bool> OnClassReadyToUpgrade;
        public static event Action OnClassroomUnlocked;
        public static event Action<StudentStateManager,ClassroomProfile> OnStudentLeftTheClassroom;
        public static event Action<ClassroomProfile> OnStudentSatInClass;
        public static event Action<Transform> OnSnapPlayer;
        public static event Action<bool> OnSwitchTheCamera;
        public static event Action<bool> OnTriggerWithReward;
        public static event Action OnClickedCoffeeButton;
        public static event Action OnBackToFoot;
        public static event Action OnTutComplete;
        public static event Action<bool> OnInterPopupShown;
        public static event Action<bool> OnTeacherEnterSleepyState;
        public static event Action OnStudentStateUpdated;
        public static event Action OnStudentAdmitted;
        public static event Action<StudentRequirements> OnStudentLeftTheSchool;
        

        public static void EnteredClassroomEvent(bool isLeftClassroom,bool hasEntered)
        {
            OnEnteredClassroom?.Invoke(isLeftClassroom,hasEntered);
        }

        public static void TriggerTeachingEvent(bool toTeach,Vector3 positionToMoveTo,Vector3 rotationToUse
            ,ClassroomProfile classroomProfile)
        {
            OnTriggerTeaching?.Invoke(toTeach,positionToMoveTo,rotationToUse,classroomProfile);
        }
        
        public static void StudentSatInClassEvent(ClassroomProfile classroomProfile)
        {
            OnStudentSatInClass?.Invoke(classroomProfile);
        }

        public static void TeacherStartTeachingEvent(ClassroomProfile obj,bool taughtByPlayer)
        {
            OnTeacherStartTeaching?.Invoke(obj,taughtByPlayer);
        }

        public static void StudentLeftTheClassroomEvent(StudentStateManager student,ClassroomProfile classroomProfile)
        {
             OnStudentLeftTheClassroom?.Invoke(student,classroomProfile);
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

        public static void ShowBoardTextEvent(bool toShow,ClassroomProfile classroomProfile)
        {
            OnShowBoardText?.Invoke(toShow,classroomProfile);
        }

        public static void SnapPlayerEvent(Transform obj)
        {
            OnSnapPlayer?.Invoke(obj);
        }

        public static void TutCompleteEvent()
        {
            OnTutComplete?.Invoke();
        }

        public static void BackToFootEvent()
        {
            OnBackToFoot?.Invoke();
        }

        public static void TriggerWithRoomEvent(Transform obj)
        {
            OnTriggeredWithRoom?.Invoke(obj);
        }

        public static void TriggerWithRewardEvent(bool obj)
        {
            OnTriggerWithReward?.Invoke(obj);
        }

        public static void InterPopupShown(bool val)
        {
            OnInterPopupShown?.Invoke(val);
        }
    }
}
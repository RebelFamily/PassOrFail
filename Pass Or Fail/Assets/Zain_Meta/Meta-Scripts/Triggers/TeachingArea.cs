using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.Triggers
{
    public class TeachingArea : MonoBehaviour, IReception
    {
        [SerializeField] private Collider collisionTrigger;
        [SerializeField] private Transform snappingPoint;
        [SerializeField] private GameObject triggerVisuals;
        [SerializeField] private ClassroomProfile myClass;
        public bool isPlayerTriggering;

        private void Awake()
        {
            collisionTrigger.enabled = false;
            triggerVisuals.SetActive(false);
        }

        public void StartServing()
        {
            isPlayerTriggering = true; //later used in AI detection
            HideTeachingArea();
            EventsManager.TriggerTeachingEvent(true,
                snappingPoint.position, snappingPoint.localEulerAngles,myClass);

            EventsManager.ShowBoardTextEvent(true,myClass);
            if (OnBoardingManager.Instance.CheckForCurrentState(TutorialState.TeachTheClass))
                OnBoardingManager.Instance.HideWaypoints();
            DOVirtual.DelayedCall(2.4f, () =>
            {
                EventsManager.TeacherStartTeachingEvent(myClass);
                StopServing();
                if (OnBoardingManager.TutorialComplete) return;
                OnBoardingManager.Instance.SetStateBasedOn(TutorialState.TeachTheClass,
                    TutorialState.UnlockNextClassroom);
            });
        }

        public void StopServing()
        {  
            isPlayerTriggering = false;
            EventsManager.TriggerTeachingEvent(false,
                snappingPoint.position, snappingPoint.localEulerAngles,null);
        }

        public void HideTeachingArea()
        {
            collisionTrigger.enabled = false;
            triggerVisuals.SetActive(false);
        }

        public void ShowTeachingArea()
        {
            collisionTrigger.enabled = true;
            triggerVisuals.SetActive(true);

            if (OnBoardingManager.TutorialComplete) return;
            OnBoardingManager.Instance.SetStateBasedOn(TutorialState.IdleForSometime,
                TutorialState.TeachTheClass);
        }

        public void ServeByTeacher()
        {
            HideTeachingArea();
            EventsManager.ShowBoardTextEvent(true,myClass);
            DOVirtual.DelayedCall(2.4f, () =>
            {
                EventsManager.TeacherStartTeachingEvent(myClass);
                StopServing();
            });
        }

        public Transform GetTeachingPos() => snappingPoint;
    }
}
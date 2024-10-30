using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.Triggers
{
    public class TeachingArea : MonoBehaviour, IReception
    {
        [SerializeField] private bool classHasATeacher;
        [SerializeField] private Collider collisionTrigger;
        [SerializeField] private Transform snappingPoint;
        [SerializeField] private GameObject triggerVisuals;
        [SerializeField] private ClassroomProfile myClass;
        public bool isPlayerTriggering;

        private void Awake()
        {
            collisionTrigger.enabled = !classHasATeacher;
            triggerVisuals.SetActive(!classHasATeacher);
        }

        public void StartServing()
        {
            if (classHasATeacher) return;
            HideTeachingArea();
            isPlayerTriggering = true; //later used in AI detection
            EventsManager.TriggerTeachingEvent(true,
                snappingPoint.position, snappingPoint.localEulerAngles);

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
            if (classHasATeacher) return;
            isPlayerTriggering = false;
            EventsManager.TriggerTeachingEvent(false,
                snappingPoint.position, snappingPoint.localEulerAngles);
        }

        public void HideTeachingArea()
        {
            collisionTrigger.enabled = false;
            triggerVisuals.SetActive(false);
        }

        public void ShowTeachingArea()
        {
            print("Show The Teaching Area");
            collisionTrigger.enabled = true;
            triggerVisuals.SetActive(true);

            if (OnBoardingManager.TutorialComplete) return;
            OnBoardingManager.Instance.SetStateBasedOn(TutorialState.IdleForSometime,
                TutorialState.TeachTheClass);
        }

        public void ServeByTeacher()
        {
            HideTeachingArea();
            DOVirtual.DelayedCall(2.4f, () =>
            {
                EventsManager.TeacherStartTeachingEvent(myClass);
                StopServing();
            });
        }

        public Transform GetTeachingPos() => snappingPoint;
    }
}
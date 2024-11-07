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
        private Vector3 _startScale;

        private void Awake()
        {
            collisionTrigger.enabled = false;
            triggerVisuals.SetActive(false);
            _startScale = triggerVisuals.transform.localScale;
        }

        public void StartServing()
        {
            isPlayerTriggering = true; //later used in AI detection
            HideTeachingArea();
            EventsManager.TriggerTeachingEvent(true,
                snappingPoint.position, snappingPoint.localEulerAngles, myClass);

            EventsManager.ShowBoardTextEvent(true, myClass);
            if (OnBoardingManager.Instance.CheckForCurrentState(TutorialState.TeachTheClass))
                OnBoardingManager.Instance.HideWaypoints();
            DOVirtual.DelayedCall(2.4f, () =>
            {
                EventsManager.TeacherStartTeachingEvent(myClass, true);
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
                snappingPoint.position, snappingPoint.localEulerAngles, null);
        }

        public void HideTeachingArea()
        {
            DOTween.Kill(triggerVisuals);
            collisionTrigger.enabled = false;
            triggerVisuals.transform.DOScale(0, .25f).OnComplete(() => { triggerVisuals.SetActive(false); });
        }

        public void ShowTeachingArea()
        {
            DOTween.Kill(triggerVisuals);
            collisionTrigger.enabled = true;
            triggerVisuals.SetActive(true);
            triggerVisuals.transform.DOScale(_startScale, .25f);

            if (OnBoardingManager.TutorialComplete) return;
            OnBoardingManager.Instance.SetStateBasedOn(TutorialState.IdleForSometime,
                TutorialState.TeachTheClass);
        }

        public void ServeByTeacher()
        {
            HideTeachingArea();
            EventsManager.ShowBoardTextEvent(true, myClass);
            DOVirtual.DelayedCall(2.4f, () =>
            {
                EventsManager.TeacherStartTeachingEvent(myClass, false);
                StopServing();
            });
        }

        public Transform GetTeachingPos() => snappingPoint;
    }
}
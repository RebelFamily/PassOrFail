using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.Triggers
{
    public class TeachingArea : MonoBehaviour, IReception
    {
        [FormerlySerializedAs("hasReceptionist")] [SerializeField]
        private bool classHasATeacher;

        [SerializeField] private Collider collisionTrigger;
        [SerializeField] private Transform snappingPoint;
        [SerializeField] private GameObject triggerVisuals;
        [SerializeField] private Image receptionServingFiller;
        [SerializeField] private ClassroomProfile myClass;
        private bool _isPlayerTriggering;

        private void Awake()
        {
            collisionTrigger.enabled = !classHasATeacher;
            triggerVisuals.SetActive(!classHasATeacher);
        }
        
        public void StartServing()
        {
            if (classHasATeacher) return;
            _isPlayerTriggering = true; //later used in AI detection
            EventsManager.TriggerTeachingEvent(true,
                snappingPoint.position, snappingPoint.localEulerAngles);
            
            DOVirtual.DelayedCall(2.4f,()=>
            {
                EventsManager.TeacherStartTeachingEvent(myClass);
                StopServing();
            });
        }

        public void StopServing()
        {
            if (classHasATeacher) return;
            _isPlayerTriggering = false;
            receptionServingFiller.fillAmount = 0;
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
            collisionTrigger.enabled = true;
            triggerVisuals.SetActive(true);
        }
        
    }
}
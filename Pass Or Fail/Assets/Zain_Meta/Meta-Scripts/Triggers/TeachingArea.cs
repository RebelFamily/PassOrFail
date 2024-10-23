using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
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
        private bool _isPlayerTriggering;

        private void Start()
        {
            collisionTrigger.enabled = !classHasATeacher;
            triggerVisuals.SetActive(!classHasATeacher);
        }


        /*
        private void Update()
        {
            if (classHasATeacher)
                ServeByHelper();
        }
        */


        /*private void ServeByPlayer()
        {
            if (!CanStudentBeAdmitted())
            {
                receptionServingFiller.fillAmount = 0;
                return;
            }

            ServingTimer();
        }*/

        private void ServeByHelper()
        {
            /*if (!queuePoints[0].IsOccupied())
            {
                receptionServingFiller.fillAmount = 0;
                receptionist.BackToIdle();
                return;
            }*/

            ServingTimer();
        }

        private void ServingTimer()
        {
            /*if (_curTimerToServe < .1f)
            {
                _curTimerToServe = servingDelay;
            }
            else
            {
                _curTimerToServe -= Time.deltaTime;
            }

            var normalValue = Mathf.InverseLerp(servingDelay, 0, _curTimerToServe);
            receptionServingFiller.fillAmount = normalValue;*/
        }

        public void StartServing()
        {
            if (classHasATeacher) return;
            _isPlayerTriggering = true; //later used in AI detection
            EventsManager.TriggerTeachingEvent(true,
                snappingPoint.position, snappingPoint.localEulerAngles);
            DOVirtual.DelayedCall(2.4f, StopServing);
        }

        public void StopServing()
        {
            if (classHasATeacher) return;
            _isPlayerTriggering = false;
            receptionServingFiller.fillAmount = 0;
            EventsManager.TriggerTeachingEvent(false,
                snappingPoint.position, snappingPoint.localEulerAngles);
        }

        private bool CanStudentBeAdmitted()
        {
            if (!_isPlayerTriggering) return false;


            // check for if all the classes are filled and there is no space left
            return true;
        }

        public void SetReceptionist(bool val)
        {
            classHasATeacher = val;
            collisionTrigger.enabled = !classHasATeacher;
            triggerVisuals.SetActive(!classHasATeacher);
        }
    }
}
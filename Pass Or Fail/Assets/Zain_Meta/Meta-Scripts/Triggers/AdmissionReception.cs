using UnityEngine;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.MetaRelated;

namespace Zain_Meta.Meta_Scripts.Triggers
{
    public class AdmissionReception : MonoBehaviour, IReception
    {
        [SerializeField] private CashGenerationSystem myCashGeneration;
        [SerializeField] private bool hasReceptionist;
        [SerializeField] private Collider collisionTrigger;
        [SerializeField] private Receptionist receptionist;
        [SerializeField] private Image receptionServingFiller;
        [SerializeField] private float servingDelay;
        [SerializeField] private ReceptionPoint[] queuePoints;
        private ClassroomProfilesManager _classesManager;
        private bool _isPlayerTriggering;
        private float _curTimerToServe;

        private void Start()
        {
            collisionTrigger.enabled = !hasReceptionist;
            _classesManager = ClassroomProfilesManager.Instance;
        }


        private void Update()
        {
            if (hasReceptionist)
                ServeByHelper();
            else
                ServeByPlayer();
        }


        private void ServeByPlayer()
        {
            if (!_isPlayerTriggering) return;
            if (!CanStudentBeAdmitted())
            {
                receptionServingFiller.fillAmount = 0;
                return;
            }

            ServingTimer();
        }

        private void ServeByHelper()
        {
            if (!CanStudentBeAdmitted())
            {
                receptionServingFiller.fillAmount = 0;
                receptionist.BackToIdle();
                return;
            }

            receptionist.StartWorking();
            ServingTimer();
        }

        private void ServingTimer()
        {
            if (_curTimerToServe < .1f)
            {
                _curTimerToServe = servingDelay;
                myCashGeneration.AddCash(1, queuePoints[0].transform);
                queuePoints[0].GetStudentAtThisPoint().AdmitMePlease();
                queuePoints[0].FreeTheSpot();
                RearrangeAllStudentsInTheQueue();
            }
            else
            {
                _curTimerToServe -= Time.deltaTime;
            }

            var normalValue = Mathf.InverseLerp(servingDelay, 0, _curTimerToServe);
            receptionServingFiller.fillAmount = normalValue;
        }

        public void StartServing()
        {
            if (hasReceptionist) return;
            _isPlayerTriggering = true;
            receptionServingFiller.fillAmount = 0;
            _curTimerToServe = servingDelay;
        }

        public void StopServing()
        {
            if (hasReceptionist) return;
            _isPlayerTriggering = false;
            receptionServingFiller.fillAmount = 0;
        }

        private bool CanStudentBeAdmitted()
        {
            if (!queuePoints[0].IsOccupied()) return false;
            var firstInLine = queuePoints[0].GetStudentAtThisPoint();
            if (!firstInLine.CanBeServed()) return false;
            
            if (!_classesManager.CheckIfAnyClassIsFree()) return false;

            return true;
        }

        private void RearrangeAllStudentsInTheQueue()
        {
            var waitingPointIndex = 0;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < queuePoints.Length; i++)
            {
                var student = queuePoints[i].GetStudentAtThisPoint();
                if (student)
                {
                    queuePoints[i].FreeTheSpot();
                    queuePoints[i - 1].OccupyThis(student);
                    student.MoveAheadInTheLine(queuePoints[waitingPointIndex++].GetQueuePoint());
                }
            }
        }

        public void SetReceptionist(bool val)
        {
            hasReceptionist = val;
            collisionTrigger.enabled = !hasReceptionist;
        }
    }
}
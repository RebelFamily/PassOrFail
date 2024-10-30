using UnityEngine;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Helpers;
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


        private int _startingStudents;

        private void Start()
        {
            collisionTrigger.enabled = !hasReceptionist;
            _classesManager = ClassroomProfilesManager.Instance;
            _curTimerToServe = servingDelay;
            receptionServingFiller.fillAmount = 0;
            _startingStudents = PlayerPrefs.GetInt("StartingStudents", 0);
            print("Served Students Are " + _startingStudents);
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
            if (!CanStudentBeAdmittedByPlayer())
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
                var firstInLine = queuePoints[0].GetStudentAtThisPoint();
                firstInLine.GetRequirements().AssignMeClasses();
                queuePoints[0].FreeTheSpot();
                RearrangeAllStudentsInTheQueue();
                if (!OnBoardingManager.Instance.CheckForCurrentState(TutorialState.AdmitKids))
                    firstInLine.AdmitMePlease();
                else
                {
                    firstInLine.WaitOutside();
                    _startingStudents++;
                    if (_startingStudents >= 4)
                    {
                        OnBoardingManager.Instance.SetStateBasedOn(TutorialState.AdmitKids,
                            TutorialState.PickReceptionCash);
                    }

                    PlayerPrefs.SetInt("StartingStudents", _startingStudents);
                }
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
            if (OnBoardingManager.Instance.CheckForCurrentState(TutorialState.GotoReception) ||
                OnBoardingManager.Instance.CheckForCurrentState(TutorialState.AdmitKids))
                OnBoardingManager.Instance.HideWaypoints();
            receptionServingFiller.fillAmount = 0;
            _curTimerToServe = servingDelay;

            if (OnBoardingManager.TutorialComplete) return;

            OnBoardingManager.Instance.SetStateBasedOn(TutorialState.GotoReception,
                TutorialState.AdmitKids);
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

        private bool CanStudentBeAdmittedByPlayer()
        {
            if (!queuePoints[0].IsOccupied()) return false;
            var firstInLine = queuePoints[0].GetStudentAtThisPoint();
            if (!firstInLine.CanBeServed()) return false;

            if (!OnBoardingManager.Instance.CheckForCurrentState(TutorialState.AdmitKids))
                if (!_classesManager.CheckIfAnyClassIsFree())
                    return false;

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
                    if (i - 1 >= 0)
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
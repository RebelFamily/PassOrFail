using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.AI
{
    public class StudentRequirements : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float accuracy;
        [SerializeField] private float learningTime;
        [SerializeField] private AIPath agentSettings;
        [SerializeField] private AIDestinationSetter destinationSetter;
        [SerializeField] private StudentAnimation studentAnimator;
        [SerializeField] private GameObject workingCanvas;
        [SerializeField] private Image fillingImage;
        public RandomPoint curRandomPoint;
        public SeatProfile mySeat;
        public List<int> classesIndex = new();
        public int curClassIndex;
        public Transform curTarget;
        private ClassroomProfilesManager _classesManager;
        public bool hasTakenAllClasses;
        private float _curLearningTimer;

        private void Start()
        {
            agentSettings.maxSpeed = moveSpeed;
            _classesManager = ClassroomProfilesManager.Instance;
            _curLearningTimer = learningTime;
        }

        private void OnEnable()
        {
            EventsManager.OnClassroomUnlocked += AssignMeNewClasses;
        }

        private void OnDisable()
        {
            EventsManager.OnClassroomUnlocked -= AssignMeNewClasses;
        }

        private void AssignMeNewClasses()
        {
            Invoke(nameof(AssignMeClasses), 2);
        }

        public void EnableTheStudent(bool val)
        {
            if (agentSettings)
            {
                agentSettings.enabled = val;
                agentSettings.isStopped = !val;
            }

            if (destinationSetter)
                destinationSetter.enabled = val;
            studentAnimator.StrafeAnim(val ? 1f : 0);
        }

        public void MoveTheTargetTo(Transform target)
        {
            PlaceOnGround();
            EnableTheStudent(true);
            destinationSetter.target = target;
        }

        public bool CheckForDistance()
        {
            return Vector3.Distance(transform.position, curTarget.position) < accuracy;
        }

        public bool CheckForDistance(float distance)
        {
            return Vector3.Distance(transform.position, curTarget.position) < distance;
        }


        public void MoveTheTargetTo(Vector3 target)
        {
            destinationSetter.target = null;
            EnableTheStudent(true);
            destinationSetter.targetVector = target;
        }

        public ClassroomProfilesManager GetManager()
        {
            if (!_classesManager)
                _classesManager = ClassroomProfilesManager.Instance;
            return _classesManager;
        }

        public ClassroomProfile GetMyClass()
        {
            return _classesManager.GetThisClass(classesIndex[curClassIndex]);
        }

        public void FaceTheTarget()
        {
            if (!curTarget) return;
            transform.DORotateQuaternion(curTarget.rotation, .1f);
            transform.DOMove(curTarget.position, .1f);
        }

        private void PlaceOnGround()
        {
            var transformLocalPosition = transform.localPosition;
            transformLocalPosition.y = 0;
            transform.localPosition = transformLocalPosition;
        }

        public void SitOnDesk() => studentAnimator.PlaySittingAnimation(true);

        public void GetUpFromDesk()
        {
            studentAnimator.PlaySittingAnimation(false);
            workingCanvas.SetActive(false);
        }

        public void CheckForLeavingTheSchool()
        {
            EventsManager.StudentStateUpdatedEvent();
            if (curClassIndex >= classesIndex.Count)
                hasTakenAllClasses = true;

            if (!hasTakenAllClasses) return;

            EventsManager.StudentLeftTheSchoolEvent(this);
        }

        public void ResetTheMovementAnimation() => studentAnimator.NormalizeTheMovement();

        public void StartDoingClasswork()
        {
            workingCanvas.SetActive(true);
            _curLearningTimer = learningTime;
            studentAnimator.PlayLearningAnimation();
        }

        public bool HasFinishedDoingClasswork()
        {
            fillingImage.fillAmount = Mathf.InverseLerp(learningTime, 0, _curLearningTimer);
            if (_curLearningTimer < .1f)
            {
                _curLearningTimer = learningTime;
                return true;
            }

            _curLearningTimer -= Time.deltaTime;
            return false;
        }

        public void MoveToRandomPointInCorridor()
        {
            EnableTheStudent(true);
            curRandomPoint = GetManager().GetARandomWaitingPoint();
            if (curRandomPoint)
            {
                curTarget = curRandomPoint.GetPoint();
                MoveTheTargetTo(curTarget);
            }
            else
                Debug.LogError("No Random Point");
        }

        public void AssignMeClasses()
        {
            if (!_classesManager) _classesManager = ClassroomProfilesManager.Instance;

            _classesManager.AssignClasses(this);
            EventsManager.StudentStateUpdatedEvent();
        }


        public void SwapClassIndices(int replacingIndex)
        {
            (classesIndex[replacingIndex], classesIndex[curClassIndex]) =
                (classesIndex[curClassIndex], classesIndex[replacingIndex]);
        }


        public void PopulateStates(int[] statesIndices, int previousStateIndex)
        {
            foreach (var t in statesIndices)
            {
                classesIndex.Add(t);
            }

            curClassIndex = previousStateIndex;
        }

        public void DestroyMe()
        {
            Destroy(gameObject);
        }

        public void RemoveTheTakenClass()
        {
            curClassIndex++;
        }
    }
}
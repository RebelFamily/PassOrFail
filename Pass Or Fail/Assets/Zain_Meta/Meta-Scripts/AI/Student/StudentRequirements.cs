using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
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
        public List<int> classesIndex = new();
        public Transform curTarget;
        private ClassroomProfilesManager _classesManager;
        public int curClassIndex;
        public Vector3 randomWaitingPoint;
        public bool hasTakenAllClasses;
        private float _curLearningTimer;

        private void Start()
        {
            agentSettings.maxAcceleration = moveSpeed;
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
            EnableTheStudent(true);
            destinationSetter.target = target;
        }

        public bool CheckForDistance()
        {
            return Vector3.Distance(transform.position, curTarget.position) < accuracy;
        }

        public bool CheckForDistance(Vector3 target)
        {
            return Vector3.Distance(transform.position, target) < accuracy;
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

        public void FaceTheTarget()
        {
            if (!curTarget) return;
            transform.DORotateQuaternion(curTarget.rotation, .1f);
        }

        public void SitOnDesk() => studentAnimator.PlaySittingAnimation(true);
        public void GetUpFromDesk() => studentAnimator.PlaySittingAnimation(false);

        public void CheckForLeavingTheSchool()
        {
            EventsManager.StudentStateUpdatedEvent();
            if (curClassIndex >= classesIndex.Count)
                hasTakenAllClasses = true;
            if (!hasTakenAllClasses) return;

            print("Chal Beta Nikal Shaba");

            EventsManager.StudentLeftTheSchoolEvent(this);
        }

        public void ResetTheMovementAnimation() => studentAnimator.NormalizeTheMovement();

        public void StartDoingClasswork()
        {
            _curLearningTimer = learningTime;
            studentAnimator.PlayLearningAnimation();
        }

        public bool HasFinishedDoingClasswork()
        {
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
            randomWaitingPoint = GetManager().GetRandomPointInCorridor();
            MoveTheTargetTo(randomWaitingPoint);
        }

        public void AssignMeClasses()
        {
            if (!_classesManager) _classesManager = ClassroomProfilesManager.Instance;

            print("assign Classes");
            _classesManager.AssignClasses(this);
            EventsManager.StudentStateUpdatedEvent();
        }

        public void AdjustRidesIndices(int replacingIndex)
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
    }
}
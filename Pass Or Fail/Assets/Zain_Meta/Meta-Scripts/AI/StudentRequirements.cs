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

        public ClassroomProfilesManager attendingClass;
        public Transform curTarget;
        private ClassroomProfilesManager _classesManager;

        private float _curLearningTimer;
        private void Start()
        {
            agentSettings.maxAcceleration = moveSpeed;
            _classesManager = ClassroomProfilesManager.Instance;
            _curLearningTimer = learningTime;
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
    

        public void MoveTheTargetTo(Vector3 target)
        {
            destinationSetter.target = null;
            EnableTheStudent(true);
            destinationSetter.targetVector = target;
        }

        public ClassroomProfilesManager GetManager() => _classesManager;

        public void FaceTheTarget()
        {
            if (!curTarget) return;
            transform.DORotateQuaternion(curTarget.rotation, .1f);
        }

        public void SitOnDesk() => studentAnimator.PlaySittingAnimation(true);
        public void GetUpFromDesk() => studentAnimator.PlaySittingAnimation(false);
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
    }
}
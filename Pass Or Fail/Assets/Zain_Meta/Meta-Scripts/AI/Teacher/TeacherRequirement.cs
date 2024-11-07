using DG.Tweening;
using Lean.Pool;
using Pathfinding;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.AI.Teacher
{
    public class TeacherRequirement : MonoBehaviour
    {
        [SerializeField] private ClassroomProfile myClassToTeach;
        [SerializeField] private TeacherAnimator teacherAnim;
        [SerializeField] private StackingHandler myCoffeeStack;
        [SerializeField] private StaffroomCounterProfile myChair;
        [SerializeField] private Transform staffSeatPos;
        [SerializeField] private int minTeaching, maxTeaching;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float accuracy;
        [SerializeField] private float restingTime;
        [SerializeField] private AIPath agentSettings;
        [SerializeField] private AIDestinationSetter destinationSetter;
        [SerializeField] private ParticleSystem sleepyVfx;
        [SerializeField] private TeacherStateManager myManager;
        [HideInInspector] public Transform curTarget;
        private float _curRestingTimer;
        private int _curTeachingIndex, _curTeachingAmount;

        private void OnEnable()
        {
            EventsManager.OnTeacherStartTeaching += MoveToPodiumPos;
        }

        private void OnDisable()
        {
            EventsManager.OnTeacherStartTeaching -= MoveToPodiumPos;
        }


        private void MoveToPodiumPos(ClassroomProfile classroom, bool taughtByPlayer)
        {
            if (taughtByPlayer) return;
            if (classroom != myClassToTeach) return;

            if (ShouldSitInClass())
                myManager.ChangeState(myManager.WaitingInClass);
            else
                myManager.ChangeState(myManager.MoveToRestingState);
        }

        private void Start()
        {
            agentSettings.maxSpeed = moveSpeed;
            _curRestingTimer = restingTime;
            _curTeachingAmount = Random.Range(minTeaching, maxTeaching);
        }


        public void EnableTheTeacher(bool val)
        {
            if (agentSettings)
            {
                agentSettings.enabled = val;
                agentSettings.isStopped = !val;
            }

            if (destinationSetter)
                destinationSetter.enabled = val;
            teacherAnim.PlayStrafeAnim(val ? 1f : 0);
        }

        public void NormalizeMovement()
        {
            teacherAnim.PlayNormalAnim();
        }

        public void MoveTheTargetTo(Transform target)
        {
            PlaceOnGround();
            EnableTheTeacher(true);
            destinationSetter.target = target;
        }

        private void PlaceOnGround()
        {
            var transformLocalPosition = transform.localPosition;
            transformLocalPosition.y = 0;
            transform.localPosition = transformLocalPosition;
        }

        public void MoveTheTargetTo()
        {
            PlaceOnGround();
            EnableTheTeacher(true);
            destinationSetter.target = curTarget;
        }

        public bool CheckForDistance()
        {
            return Vector3.Distance(transform.position, curTarget.position) < accuracy;
        }

        public bool CheckForDistance(float testDistance)
        {
            return Vector3.Distance(transform.position, curTarget.position) < testDistance;
        }


        public void MoveTheTargetTo(Vector3 target)
        {
            destinationSetter.target = null;
            EnableTheTeacher(true);
            destinationSetter.targetVector = target;
        }


        public void FaceTheTarget()
        {
            if (!curTarget) return;
            transform.DOMove(curTarget.position, .1f);
            transform.DORotateQuaternion(curTarget.rotation, .1f);
        }

        public void SitOnDesk() => teacherAnim.PlaySittingAnimation(true);
        public void GetUpFromDesk() => teacherAnim.PlaySittingAnimation(false);

        public void TeachTheClass()
        {
            teacherAnim.PlayTeachingAnim();
            myClassToTeach.GetTeachingArea().ServeByTeacher();
            _curTeachingIndex++;
        }

        public bool IsPlayerTeachingMyClass()
        {
            return myClassToTeach.GetTeachingArea().isPlayerTriggering;
        }

        public void StartRestingTimer()
        {
            _curRestingTimer = restingTime;
        }

        public bool HasRestedInClassroom()
        {
            if (_curRestingTimer < .1f)
            {
                _curRestingTimer = restingTime;
                return true;
            }

            _curRestingTimer -= Time.deltaTime;
            return false;
        }

        public bool HasTaughtEnoughClasses()
        {
            if (_curTeachingIndex >= _curTeachingAmount)
            {
                _curTeachingIndex = 0;
                _curTeachingAmount = Random.Range(minTeaching, maxTeaching);
                return true;
            }

            return false;
        }

        public Transform GetMyTeachingPoint()
        {
            return myClassToTeach.GetTeachingArea().GetTeachingPos();
        }

        public Transform GetMyWaitingPos() => myClassToTeach.podiumTransforms;

        public ClassroomProfile GetMyClass() => myClassToTeach;

        private bool ShouldSitInClass()
        {
            var random = Random.Range(0, 10);

            return random > 7;
        }

        public void GotoStaffRoom()
        {
            EnableTheTeacher(true);
            curTarget = staffSeatPos;
            MoveTheTargetTo();
        }

        public void PlaySleepyAnim()
        {
            teacherAnim.PlaySleepyAnim();
            sleepyVfx.Play();
        }

        public void DrinkTheCoffee()
        {
            sleepyVfx.Stop();
            teacherAnim.PlayCoffeeAnim();
        }

        public bool IsMyCoffeeReady()
        {
            return myCoffeeStack.HasItemsInStack();
        }

        public void ConsumeTheCoffee()
        {
            LeanPool.Despawn(myCoffeeStack.GetLastStackedItem());
        }

        public void ImReadyForCoffee()
        {
            myCoffeeStack.isReadyToAccept = true;
            myChair.Show();
            EventsManager.TeacherEnteredSleepyStateEvent(true);
        }
    }
}
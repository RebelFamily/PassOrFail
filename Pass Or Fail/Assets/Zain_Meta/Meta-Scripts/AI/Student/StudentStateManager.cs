using UnityEngine;
using Zain_Meta.Meta_Scripts.AI.States;
using Zain_Meta.Meta_Scripts.AI.Student.States;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.AI
{
    public class StudentStateManager : MonoBehaviour
    {
        [SerializeField] private StudentRequirements requirements;


        public EnterSchoolState EnterSchoolState = new EnterSchoolState();
        public MoveToQueuePoint MoveToQueuePoint = new MoveToQueuePoint();
        public StandInQueue StandInQueue = new StandInQueue();
        public MoveAheadInQueue MoveAheadInQueue = new MoveAheadInQueue();
        public GoToClassRoom GoToClassRoom = new GoToClassRoom();
        public ReachTheSeat ReachTheSeat = new ReachTheSeat();
        public SitOnDesk SitOnDesk = new SitOnDesk();
        public DoingClassWork DoingClassWork = new DoingClassWork();
        public LeaveTheSeat LeaveTheSeat = new LeaveTheSeat();
        public LeaveClassroom LeaveClassroom = new LeaveClassroom();
        public WaitInCorridor WaitInCorridor = new WaitInCorridor();
        public MoveToCorridor MoveToCorridor = new MoveToCorridor();
        public LeaveSchool LeaveSchool = new LeaveSchool();
        public GraduatingState GraduatingState = new GraduatingState();

        private IState _curState;

        [SerializeField] private string stateName;
        private StudentsDataManager _studentsDataManager;

        private void Start()
        {
            _studentsDataManager = StudentsDataManager.Instance;
        }

        private void LateUpdate()
        {
            _curState?.UpdateState(this);
            stateName = _curState?.ToString();
        }

        public void ChangeState(IState newState)
        {
            if (_curState != null)
            {
                if (_curState == newState) return;
                _curState.ExitState(this);
            }

            _curState = newState;
            _curState.EnterState(this);
        }

        public void AdmitMePlease()
        {
            if (_curState != StandInQueue) return;
            ChangeState(GoToClassRoom);
            _studentsDataManager.AddStudentInTheSchool(requirements,true);
        }
        public void AdmitMePleaseForcefully()
        {
            ChangeState(WaitInCorridor);
            _studentsDataManager=StudentsDataManager.Instance;
            _studentsDataManager.AddStudentInTheSchool(requirements,false);
        }

        public void WaitOutside()
        {
            ChangeState(WaitInCorridor);
            _studentsDataManager.AddStudentInTheSchool(requirements,true);
        }
        public void StartLearning()
        {
            if (_curState != SitOnDesk) return;
            ChangeState(DoingClassWork);
        }

        public void MoveAheadInTheLine(Transform newTarget)
        {
            _curState.ExitState(this);
            requirements.curTarget = newTarget;
            _curState = MoveAheadInQueue;
            _curState.EnterState(this);
        }

        public bool CanBeServed()
        {
            return _curState == StandInQueue;
        }

        public StudentRequirements GetRequirements() => requirements;
    }
}
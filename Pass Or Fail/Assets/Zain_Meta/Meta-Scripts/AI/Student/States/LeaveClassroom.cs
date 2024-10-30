using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class LeaveClassroom : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            EventsManager.StudentLeftTheClassroomEvent(student);
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(true);
            _requirements.curTarget = _requirements.GetManager().GetExitingPoint();
            _requirements.MoveTheTargetTo(_requirements.curTarget);
            stateManager.ChangeState(stateManager.LeaveSchool);
        }

        public void ExitState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(false);
        }
    }
}
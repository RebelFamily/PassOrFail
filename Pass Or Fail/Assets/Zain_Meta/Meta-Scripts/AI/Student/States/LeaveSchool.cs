namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class LeaveSchool : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.curTarget = _requirements.GetManager().GetExitingPoint();
            _requirements.MoveTheTargetTo(_requirements.curTarget);
            _requirements.EnableTheStudent(true);
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            if (!_requirements.CheckForDistance()) return;
            _requirements.DestroyMe();
        }

        public void ExitState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(false);
        }
    }
}
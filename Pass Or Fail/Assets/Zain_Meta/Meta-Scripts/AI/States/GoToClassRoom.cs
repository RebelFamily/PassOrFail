namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class GoToClassRoom : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            var seatTarget = _requirements.GetManager().GetAvailableSeat(stateManager);
            if(!seatTarget) return;
            _requirements.EnableTheStudent(true);
            _requirements.curTarget = seatTarget;
            _requirements.MoveTheTargetTo(seatTarget);
            stateManager.ChangeState(stateManager.ReachTheSeat);
        }

        public void ExitState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(false);
        }
    }
}
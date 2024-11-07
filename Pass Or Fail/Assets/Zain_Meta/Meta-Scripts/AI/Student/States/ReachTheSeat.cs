namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class ReachTheSeat : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.EnableTheStudent(true);
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            if (!_requirements.CheckForDistance(.5f)) return;

            stateManager.ChangeState(stateManager.SitOnDesk);
        }

        public void ExitState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(false);
        }
    }
}
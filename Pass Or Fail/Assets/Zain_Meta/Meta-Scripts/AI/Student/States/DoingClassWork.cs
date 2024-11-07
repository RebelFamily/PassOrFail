namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class DoingClassWork : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.StartDoingClasswork();
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            if(!_requirements.HasFinishedDoingClasswork()) return;
            
            stateManager.ChangeState(stateManager.LeaveTheSeat);
        }

        public void ExitState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(false);
        }
    }
}
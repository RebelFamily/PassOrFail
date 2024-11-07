namespace Zain_Meta.Meta_Scripts.AI.Student.States
{
    public class GraduatingState : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.EnableTheStudent(true);
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            if (!_requirements.CheckForDistance()) return;
            _requirements.GetManager().AddCashInGraduationStack(2, _requirements.transform);
            stateManager.ChangeState(stateManager.LeaveSchool);
        }

        public void ExitState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(false);
        }
    }
}
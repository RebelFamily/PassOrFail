namespace Zain_Meta.Meta_Scripts.AI.Student.States
{
    public class WaitInCorridor : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.MoveToRandomPointInCorridor();
            _requirements.GetManager().AssignClasses(_requirements);
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            if (_requirements.CheckForDistance())
            {
                _requirements.FaceTheTarget();
                stateManager.ChangeState(stateManager.MoveToCorridor);
            }
        }

        public void ExitState(StudentStateManager stateManager)
        {
        }
    }
}
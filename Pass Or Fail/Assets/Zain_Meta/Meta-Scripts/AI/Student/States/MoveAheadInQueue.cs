namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class MoveAheadInQueue: IState
    {
        private StudentRequirements _requirement;
      
        public void EnterState(StudentStateManager student)
        {
            _requirement = student.GetRequirements();
            _requirement.EnableTheStudent(true);
            _requirement.MoveTheTargetTo(_requirement.curTarget);
        }

        public void UpdateState(StudentStateManager student)
        {
            if(!_requirement.CheckForDistance()) return;
            _requirement.FaceTheTarget();
            student.ChangeState(student.StandInQueue);
        }

        public void ExitState(StudentStateManager student)
        {
            _requirement.EnableTheStudent(false);
        }
    }
}
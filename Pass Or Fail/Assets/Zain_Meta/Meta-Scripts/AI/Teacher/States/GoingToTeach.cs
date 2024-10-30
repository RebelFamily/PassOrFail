namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class GoingToTeach : ITeacherState
    {
        private TeacherRequirement _requirement;

        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.EnableTheTeacher(true);
            _requirement.curTarget = _requirement.GetMyTeachingPoint();
            _requirement.MoveTheTargetTo();
        }

        public void UpdateState(TeacherStateManager teacher)
        {
            
            if (!_requirement.CheckForDistance()) return;

            if (!_requirement.GetMyClass().ClassCanBeTaught())
            {
                teacher.ChangeState(teacher.WaitingInClass);
            }
            else
            {
                _requirement.FaceTheTarget();
                teacher.ChangeState(teacher.TeachingState);
            }
        }

        public void ExitState(TeacherStateManager teacher)
        {
            _requirement.EnableTheTeacher(false);
        }
    }
}
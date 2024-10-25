namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class StandingIdleInClass : ITeacherState
    {
        private TeacherRequirement _requirement;

        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.EnableTheTeacher(false);
        }

        public void UpdateState(TeacherStateManager teacher)
        {
            if (_requirement.HasTaughtEnoughClasses())
            {
                teacher.ChangeState(teacher.GoingToStaffRoom);
            }
            else
            {
                if (!_requirement.GetMyClass().ClassCanBeTaught()) return;

                teacher.ChangeState(teacher.GoingToTeach);
            }
        }

        public void ExitState(TeacherStateManager teacher)
        {
        }
    }
}
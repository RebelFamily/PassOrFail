namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class StandingIdleInClass : ITeacherState
    {
        private TeacherRequirement _requirement;

        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.EnableTheTeacher(false);
            if (_requirement.HasTaughtEnoughClasses())
                teacher.ChangeState(teacher.GoingToStaffRoom);
        }

        public void UpdateState(TeacherStateManager teacher)
        {
            if (_requirement.GetMyClass().ClassCanBeTaught())
                teacher.ChangeState(teacher.GoingToTeach);
            else
            {
                _requirement.GetMyClass().MakeSureToTeachMe();
            }
        }

        public void ExitState(TeacherStateManager teacher)
        {
        }
    }
}
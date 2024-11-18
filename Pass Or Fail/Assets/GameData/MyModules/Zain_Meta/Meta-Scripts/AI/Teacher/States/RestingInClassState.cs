namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class RestingInClassState : ITeacherState
    {
        private TeacherRequirement _requirement;

        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.EnableTheTeacher(false);
            _requirement.StartRestingTimer();
        }

        public void UpdateState(TeacherStateManager teacher)
        {
            if (!_requirement.HasRestedInClassroom()) return;
            _requirement.GetUpFromDesk();
            if (!_requirement.HasTaughtEnoughClasses())
                teacher.ChangeState(teacher.WaitingInClass);
            else
                teacher.ChangeState(teacher.GoingToStaffRoom);
        }

        public void ExitState(TeacherStateManager teacher)
        {
            _requirement.NormalizeMovement();
        }
    }
}
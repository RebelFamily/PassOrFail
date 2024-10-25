namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class WaitingInClass: ITeacherState
    {
        private TeacherRequirement _requirement;
        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.EnableTheTeacher(true);
            _requirement.curTarget = _requirement.GetMyWaitingPos();
            _requirement.MoveTheTargetTo();
        }

        public void UpdateState(TeacherStateManager teacher)
        {
            if(!_requirement.CheckForDistance()) return;
            _requirement.FaceTheTarget();
            teacher.ChangeState(teacher.StandingIdleInClass);
        }

        public void ExitState(TeacherStateManager teacher)
        {
            _requirement.EnableTheTeacher(false);
        }
    }
}
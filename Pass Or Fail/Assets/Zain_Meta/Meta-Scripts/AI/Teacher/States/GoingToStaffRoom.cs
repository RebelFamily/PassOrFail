namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class GoingToStaffRoom :ITeacherState
    {
        private TeacherRequirement _requirement;

        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.EnableTheTeacher(true);
            _requirement.GotoStaffRoom();
        }

        public void UpdateState(TeacherStateManager teacher)
        {
            if(!_requirement.CheckForDistance(.5f)) return;
            
            _requirement.EnableTheTeacher(false);
            _requirement.FaceTheTarget();
            teacher.ChangeState(teacher.SleepyState);
        }

        public void ExitState(TeacherStateManager teacher)
        {
           
        }
    }
}
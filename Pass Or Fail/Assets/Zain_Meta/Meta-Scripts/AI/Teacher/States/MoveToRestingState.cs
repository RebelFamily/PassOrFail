namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class MoveToRestingState:ITeacherState
    {
        private TeacherRequirement _requirement;

        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.EnableTheTeacher(true);
            _requirement.curTarget = _requirement.GetMyClass().teacherChair;
            _requirement.MoveTheTargetTo();

        }

        public void UpdateState(TeacherStateManager teacher)
        {
            if(!_requirement.CheckForDistance()) return;
            
            _requirement.FaceTheTarget();
            _requirement.SitOnDesk();
            teacher.ChangeState(teacher.RestingInClassState);
        }

        public void ExitState(TeacherStateManager teacher)
        {
           
        }
    }
}
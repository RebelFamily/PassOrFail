namespace Zain_Meta.Meta_Scripts.AI.Teacher
{
    public interface ITeacherState
    {
        public void EnterState(TeacherStateManager teacher);
        public void UpdateState(TeacherStateManager teacher);
        public void ExitState(TeacherStateManager teacher);
    }
}
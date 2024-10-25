using UnityEngine;

namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class WaitInStaffRoom:ITeacherState
    {
        private TeacherRequirement _requirement;

        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.EnableTheTeacher(false);
            
        }

        public void UpdateState(TeacherStateManager teacher)
        {
            if(!_requirement.GetMyClass().ClassCanBeTaught()) return;
            
            Debug.Log("Lets Do Some Teaching!!!");
            teacher.ChangeState(teacher.GoingToTeach);
        }

        public void ExitState(TeacherStateManager teacher)
        {
            _requirement.EnableTheTeacher(true);
        }
    }
}
using UnityEngine;

namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class TeachingState: ITeacherState
    {
        private TeacherRequirement _requirement;
        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.EnableTheTeacher(false);
            _requirement.TeachTheClass();
            Debug.Log($"Teach Me");
        }

        public void UpdateState(TeacherStateManager teacher)
        {
            
        }

        public void ExitState(TeacherStateManager teacher)
        {
            _requirement.EnableTheTeacher(true);
        }
    }
}
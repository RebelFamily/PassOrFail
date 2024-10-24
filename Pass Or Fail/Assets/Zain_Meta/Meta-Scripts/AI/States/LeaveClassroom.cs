using UnityEngine;

namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class LeaveClassroom : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(true);
            _requirements.MoveTheTargetTo(Vector3.zero);
        }

        public void ExitState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(false);
        }
    }
}
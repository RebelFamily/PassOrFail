using UnityEngine;

namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class LeaveTheSeat : IState
    {
        private StudentRequirements _requirements;
        private float _gettingUpDelay;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.GetUpFromDesk();
            _gettingUpDelay = 1f;
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            if (_gettingUpDelay < .1f)
            {
                _requirements.ResetTheMovementAnimation();
                stateManager.ChangeState(stateManager.LeaveClassroom);
            }
            else
            {
                _gettingUpDelay -= Time.deltaTime;
            }
        }

        public void ExitState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(true);
        }
    }
}
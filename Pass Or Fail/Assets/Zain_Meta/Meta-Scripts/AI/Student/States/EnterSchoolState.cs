using UnityEngine;

namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class EnterSchoolState : IState
    {
        private StudentRequirements _requirements;
        private Transform _targetToReach;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.EnableTheStudent(false);
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            _targetToReach = _requirements.GetManager().CheckForPointAtReception(stateManager);
            if (!_targetToReach) return;

            _requirements.curTarget = _targetToReach;
            _requirements.MoveTheTargetTo(_targetToReach);
            stateManager.ChangeState(stateManager.MoveToQueuePoint);
        }

        public void ExitState(StudentStateManager stateManager)
        {
        }
    }
}
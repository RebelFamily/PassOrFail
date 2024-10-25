using UnityEngine;

namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class WaitInCorridor : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            Debug.Log("I'll be waiting here!");
            _requirements.MoveToRandomPointInCorridor();
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            if(!_requirements.CheckForDistance(_requirements.randomWaitingPoint)) return;
            stateManager.ChangeState(stateManager.MoveToCorridor);
        }

        public void ExitState(StudentStateManager stateManager)
        {
        }
    }
}
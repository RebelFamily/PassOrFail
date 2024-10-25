using UnityEngine;

namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class GoToClassRoom : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.EnableTheStudent(true);
            Debug.Log("admitted-donee");
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            var seatTarget = _requirements.GetManager().GetSeatAtRequiredClass(stateManager,
                _requirements.classesIndex.ToArray());
            if(!seatTarget) return;
            Debug.Log("admitted-updted");
            _requirements.EnableTheStudent(true);
            _requirements.curTarget = seatTarget;
            _requirements.MoveTheTargetTo(seatTarget);
            stateManager.ChangeState(stateManager.ReachTheSeat);
        }

        public void ExitState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(false);
        }
    }
}
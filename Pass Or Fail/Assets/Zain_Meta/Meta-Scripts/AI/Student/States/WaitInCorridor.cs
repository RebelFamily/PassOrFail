using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.AI.Student.States
{
    public class WaitInCorridor : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.MoveToRandomPointInCorridor();
            EventsManager.StudentLeftTheClassroomEvent(student);
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            var seatTarget = _requirements.GetManager().GetSeatAtRequiredClass(stateManager,
                _requirements.classesIndex.ToArray());
            if (seatTarget)
            {
                _requirements.EnableTheStudent(true);
                _requirements.curTarget = seatTarget;
                _requirements.MoveTheTargetTo(seatTarget);
                stateManager.ChangeState(stateManager.ReachTheSeat);
                return;
            }

            if (_requirements.CheckForDistance(_requirements.randomWaitingPoint))
            {
                stateManager.ChangeState(stateManager.MoveToCorridor);
            }
        }

        public void ExitState(StudentStateManager stateManager)
        {
        }
    }
}
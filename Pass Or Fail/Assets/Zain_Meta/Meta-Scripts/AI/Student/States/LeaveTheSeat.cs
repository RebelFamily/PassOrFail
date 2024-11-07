using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class LeaveTheSeat : IState
    {
        private StudentRequirements _requirements;
        private float _gettingUpDelay;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            EventsManager.StudentLeftTheClassroomEvent(student,_requirements.GetMyClass());
            _requirements.RemoveTheTakenClass();
            _requirements.GetUpFromDesk();
            _requirements.CheckForLeavingTheSchool();
            _gettingUpDelay = 1f;
        }

        public void UpdateState(StudentStateManager stateManager)
        {
            if (_gettingUpDelay < .1f)
            {
                _requirements.ResetTheMovementAnimation();
                if (_requirements.hasTakenAllClasses)
                    stateManager.ChangeState(stateManager.LeaveClassroom);
                else
                    stateManager.ChangeState(stateManager.WaitInCorridor);
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
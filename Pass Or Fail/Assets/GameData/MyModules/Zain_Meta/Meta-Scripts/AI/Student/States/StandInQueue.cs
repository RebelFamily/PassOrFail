﻿namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class StandInQueue : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.EnableTheStudent(false);
        }

        public void UpdateState(StudentStateManager stateManager)
        {
        }

        public void ExitState(StudentStateManager stateManager)
        {
            _requirements.EnableTheStudent(true);
        }
    }
}
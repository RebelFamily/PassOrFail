using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class DrinkCoffeeState : ITeacherState
    {
        private TeacherRequirement _requirement;

        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.EnableTheTeacher(false);
            EventsManager.TeacherEnteredSleepyStateEvent(false);
            DOVirtual.DelayedCall(1f, () =>
            {
                _requirement.DrinkTheCoffee();
                Debug.Log("Drank Coffee");
            });
            DOVirtual.DelayedCall(4f, () =>
            {
                Debug.Log("lets Go!!");
                _requirement.GetUpFromDesk();
                _requirement.NormalizeMovement();
                teacher.ChangeState(teacher.GoingToTeach);
            });
        }

        public void UpdateState(TeacherStateManager teacher)
        {
        }

        public void ExitState(TeacherStateManager teacher)
        {
        }
    }
}
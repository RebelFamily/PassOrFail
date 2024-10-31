using DG.Tweening;
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
            DOVirtual.DelayedCall(1f, () => { _requirement.DrinkTheCoffee(); });
            DOVirtual.DelayedCall(2f, () =>
            {
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
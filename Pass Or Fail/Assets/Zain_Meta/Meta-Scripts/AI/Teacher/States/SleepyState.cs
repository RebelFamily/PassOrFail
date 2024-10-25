using DG.Tweening;

namespace Zain_Meta.Meta_Scripts.AI.Teacher.States
{
    public class SleepyState : ITeacherState
    {
        private TeacherRequirement _requirement;

        public void EnterState(TeacherStateManager teacher)
        {
            _requirement = teacher.GetRequirement();
            _requirement.SitOnDesk();
            DOVirtual.DelayedCall(1f, () =>
            {
                _requirement.PlaySleepyAnim();
                _requirement.ImReadyForCoffee();
            });
        }

        public void UpdateState(TeacherStateManager teacher)
        {
            if (!_requirement.IsMyCoffeeReady()) return;

            teacher.ChangeState(teacher.DrinkCoffeeState);
        }

        public void ExitState(TeacherStateManager teacher)
        {
        }
    }
}
using DG.Tweening;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.AI.States
{
    public class SitOnDesk : IState
    {
        private StudentRequirements _requirements;

        public void EnterState(StudentStateManager student)
        {
            _requirements = student.GetRequirements();
            _requirements.EnableTheStudent(false);
            _requirements.FaceTheTarget();
            _requirements.SitOnDesk();

            DOVirtual.DelayedCall(.5f, EventsManager.StudentSatInClassEvent);
        }

        public void UpdateState(StudentStateManager stateManager)
        {
        }

        public void ExitState(StudentStateManager stateManager)
        {
        }
    }
}
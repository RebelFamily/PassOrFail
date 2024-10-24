namespace Zain_Meta.Meta_Scripts.AI
{
    public interface IState
    {
        public void EnterState(StudentStateManager student);
        public void UpdateState(StudentStateManager stateManager);
        public void ExitState(StudentStateManager stateManager);
    }
}
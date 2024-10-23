using UnityEngine;

namespace Zain_Meta.Meta_Scripts.AI
{
    public class StudentStateManager : MonoBehaviour
    {
        private IState _curState;

        private void LateUpdate()
        {
            _curState?.UpdateState(this);
        }

        public void ChangeState(IState newState)
        {
            if (_curState != null)
            {
                if (_curState == newState) return;
                _curState.ExitState(this);
            }

            _curState = newState;
            _curState.EnterState(this);
        }
    }
}
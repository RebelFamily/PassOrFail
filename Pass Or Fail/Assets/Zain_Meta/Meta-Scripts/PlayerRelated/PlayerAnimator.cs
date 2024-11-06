using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator playerAnim;
        [SerializeField] private AnimatorOverrideController boardAnim, cycleAnim, normalAnim;
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int Teach = Animator.StringToHash("Teach");
        private ArcadeMovement.PlayerState _curState;

        public void SetAnimations(float value)
        {
            playerAnim.SetFloat(MoveBlend, value);
        }

        public void PlayTeachingAnim()
        {
            playerAnim.SetTrigger(Teach);
            AudioManager.Instance.PlaySound("WritingOnBoard");
        }

        public void SetRideAnimations(ArcadeMovement.PlayerState curState)
        {
            _curState = curState;
            switch (curState)
            {
                case ArcadeMovement.PlayerState.ByFoot:
                    playerAnim.runtimeAnimatorController = normalAnim;
                    break;
                case ArcadeMovement.PlayerState.RidingSkateboard:
                    playerAnim.runtimeAnimatorController = boardAnim;
                    break;
                case ArcadeMovement.PlayerState.RidingOneWheelCycle:
                    playerAnim.runtimeAnimatorController = cycleAnim;
                    break;
            }
        }
    }
}
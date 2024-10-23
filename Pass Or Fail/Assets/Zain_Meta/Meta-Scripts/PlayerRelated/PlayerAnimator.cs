using UnityEngine;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator playerAnim;
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int Teach = Animator.StringToHash("Teach");

        public void SetAnimations(float value)
        {
            playerAnim.SetFloat(MoveBlend,value);
        }

        public void PlayTeachingAnim()
        {
            playerAnim.SetTrigger(Teach);
        }
    }
}
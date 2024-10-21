using UnityEngine;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator playerAnim;
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");

        public void SetAnimations(float value)
        {
            playerAnim.SetFloat(MoveBlend,value);
        }
    }
}
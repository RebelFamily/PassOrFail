using UnityEngine;

namespace Zain_Meta.Meta_Scripts.AI
{
    public class StudentAnimation : MonoBehaviour
    {
        [SerializeField] private Animator myAnim;
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int Sit = Animator.StringToHash("Sit");
        private static readonly int SitIndex = Animator.StringToHash("SitIndex");
        private static readonly int Learning = Animator.StringToHash("Learning");
        private static readonly int Normal = Animator.StringToHash("Normal");


        public void StrafeAnim(float val)
        {
            myAnim.SetFloat(MoveBlend, val);
        }

        public void PlaySittingAnimation(bool toSit)
        {
            myAnim.SetFloat(SitIndex, toSit ? 1 : -1);
            myAnim.SetTrigger(Sit);
        }

        public void PlayLearningAnimation()
        {
            myAnim.SetTrigger(Learning);
        }

        public void NormalizeTheMovement()
        {
            myAnim.SetTrigger(Normal);
        }
    }
}
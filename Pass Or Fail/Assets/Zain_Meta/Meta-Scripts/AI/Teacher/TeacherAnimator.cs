using UnityEngine;

namespace Zain_Meta.Meta_Scripts.AI.Teacher
{
    public class TeacherAnimator : MonoBehaviour
    {
        [SerializeField] private Animator teacherAnimator;
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int Normal = Animator.StringToHash("Normal");
        private static readonly int Sit = Animator.StringToHash("Sit");
        private static readonly int SitIndex = Animator.StringToHash("SitIndex");
        private static readonly int Teach = Animator.StringToHash("Teach");
        private static readonly int Sleepy = Animator.StringToHash("Sleepy");
        private static readonly int CoffeeDrink = Animator.StringToHash("CoffeeDrink");

        public void PlayStrafeAnim(float value)
        {
            teacherAnimator.SetFloat(MoveBlend, value);
        }

        public void PlayTeachingAnim()
        {
            teacherAnimator.SetTrigger(Teach);
        }
        public void PlayNormalAnim()
        {
            teacherAnimator.SetTrigger(Normal);
        }
        public void PlaySleepyAnim()
        {
            teacherAnimator.SetTrigger(Sleepy);
        }
        
        public void PlaySittingAnimation(bool toSit)
        {
            teacherAnimator.SetFloat(SitIndex, toSit ? 1 : -1);
            teacherAnimator.SetTrigger(Sit);
        }

        public void PlayCoffeeAnim()
        {
            teacherAnimator.SetTrigger(CoffeeDrink);
        }
    }
}
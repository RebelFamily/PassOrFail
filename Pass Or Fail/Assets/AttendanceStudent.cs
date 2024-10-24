using UnityEngine;
public class AttendanceStudent : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Expressions expressions;
    [SerializeField] private GameObject arrow;
    private static readonly int ActionNo = Animator.StringToHash("ActionNo");
    private static readonly int Action = Animator.StringToHash("Action");
    public void ShowEmotion(Expressions.ExpressionType emotion, bool playAnimation = false)
    {
        expressions.ShowExpression(emotion);
        if(!playAnimation) return;
        switch (emotion)
        {
            case Expressions.ExpressionType.Happy or Expressions.ExpressionType.Excited:
                PlayAction(Random.Range(3, 6));
                break;
            case Expressions.ExpressionType.Sad:
                PlayAction(Random.Range(0, 3));
                break;
        }
    }
    private void PlayAction(int actionNo)
    {
        animator.SetInteger(ActionNo, actionNo);
        animator.SetTrigger(Action);
    }
    public void RaiseUpTheHand()
    {
        animator.SetLayerWeight(2, 1f);
        arrow.SetActive(true);
    }
    public void RaiseDownTheHand()
    {
        animator.SetLayerWeight(2, 0f);
        arrow.SetActive(false);
    }
    public Vector3 GetHeadPosition()
    {
        return animator.GetBoneTransform(HumanBodyBones.Head).position;
    }
}
using UnityEngine;
public class PianoClassStudent : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Expressions expressions;
    private static readonly int Piano = Animator.StringToHash("PlayPiano");
    public void ShowEmotion()
    {
        var r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                expressions.ShowExpression(Expressions.ExpressionType.Normal);
                break;
            case 1:
                expressions.ShowExpression(Expressions.ExpressionType.Happy);
                break;
            case 2:
                expressions.ShowExpression(Expressions.ExpressionType.Excited);
                break;
        }
    }
    public void PlayPiano()
    {
        ShowEmotion();
        animator.SetTrigger(Piano);   
    }
}
using System.Collections;
using UnityEngine;
public class OralQuizStudent : MonoBehaviour
{
    [SerializeField] private OralQuiz oralQuiz;
    [SerializeField] private Animator animator;
    [SerializeField] private Expressions expressions;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject arrowIndication;
    private string _studentAnswer;
    private static readonly int GiveAnswer = Animator.StringToHash("GiveAnswer");
    private readonly WaitForSeconds _expressionDelay = new(1.25f);
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
    private void StandUp()
    {
        animator.SetBool(GiveAnswer, true);
        animator.SetLayerWeight(1, 1f);
        StartCoroutine(Telling());
    }
    private void SitDown()
    {
        animator.SetBool(GiveAnswer, false);
        animator.SetLayerWeight(1, 0f);
    }
    private IEnumerator Telling()
    {
        //ShowEmotion(Expressions.ExpressionType.Happy);
        /*expressions.ShowRandomExpression();
        yield return _expressionDelay;
        expressions.ShowRandomExpression();
        yield return _expressionDelay;
        expressions.ShowRandomExpression();
        yield return _expressionDelay;
        expressions.ShowRandomExpression();*/
        yield return _expressionDelay;
        SitDown();
    }
    private void RaiseUpTheHand()
    {
        animator.SetLayerWeight(2, 1f);
    }
    private void RaiseDownTheHand()
    {
        animator.SetLayerWeight(2, 0f);
    }
    public void ReadyToAnswer(bool flag, string newAnswer = "")
    {
        boxCollider.enabled = flag;
        arrowIndication.SetActive(flag);
        if(newAnswer != "")
            _studentAnswer = newAnswer;
        if (flag)
            RaiseUpTheHand();
        else
            RaiseDownTheHand();
    }
    private void OnMouseDown()
    {
        oralQuiz.OnGivingAnswer(this);
        Answer();
    }
    private void Answer()
    {
        StandUp();
    }
    public void SetStudentAnswer(string newAnswer)
    {
        _studentAnswer = newAnswer;
    }
    public string GetStudentAnswer()
    {
        return _studentAnswer;
    }
    public Vector3 GetHeadPosition()
    {
        return animator.GetBoneTransform(HumanBodyBones.Head).position;
    }
}
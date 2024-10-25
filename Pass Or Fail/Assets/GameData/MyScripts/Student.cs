using DG.Tweening;
using PassOrFail.MiniGames;
using UnityEngine;
public class Student : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private Animator animator;
    [SerializeField] private Expressions expressions;
    [SerializeField] private bool isToNotUseStudentRenders;
    private static readonly int AnimationNo = Animator.StringToHash("AnimationNo");
    private static readonly int ActionNo = Animator.StringToHash("ActionNo");
    private Vector3 _targetPosition, _targetRotation;
    private float _movementDuration = 1f;
    private Gender _gender;
    private Sprite _render;
    [SerializeField] private bool instantiateCharacters = true, setAnimationEvents = true;
    private static readonly int Happy = Animator.StringToHash("Happy");
    private static readonly int MotivationLevel = Animator.StringToHash("MotivationLevel");
    private static readonly int Claim = Animator.StringToHash("Claim");
    private void Start()
    {
        if (instantiateCharacters)
        {
            var index = 0;
            _gender = Gender.FemaleStudent;
            var genderNo = Random.Range(0, 2);
            _gender = genderNo > 0 ? Gender.MaleStudent : Gender.FemaleStudent;
            index = Random.Range(0, 4);
            var path = "Characters/Default/" + _gender + index;
            //var character = Instantiate((GameObject)Resources.Load(path), transform);
            var character = Instantiate(Resources.Load<GameObject>(path), transform);
            //var character = studentsHandler.GetCharacter(gender, index);
            animator = character.GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            expressions = animator.GetComponent<Expressions>();
        }
        if (setAnimationEvents)
        {
            animator.gameObject.AddComponent<UnityAnimationEventTrigger>();
            animator.GetComponent<UnityAnimationEventTrigger>().RegisterAnimationEvent(0);
        }
        animator.gameObject.SetActive(true);
        expressions.ShowRandomExpression();
    }
    public void ShowEmotion(Vector3 position, Vector3 rotation, float duration = 1f, Expressions.ExpressionType emotion = Expressions.ExpressionType.Normal)
    {
        _targetPosition = position;
        _targetRotation = rotation;
        _movementDuration = duration;
        expressions.ShowExpression(emotion);
        switch (emotion)
        {
            case Expressions.ExpressionType.Normal:
                Invoke(nameof(InvokeMoveStudent), 0.5f);
                //GetComponentInParent<StudentsHandler>().AddStudentRender(expressions.GetExpressionRender(Expressions.ExpressionType.Normal));
                break;
            case Expressions.ExpressionType.Happy:
                PlayAction(Random.Range(3, 6), true, Random.Range(0, 2));
                if(!isToNotUseStudentRenders) 
                    GetComponentInParent<StudentsHandler>().AddStudentRender(expressions.GetExpressionRender(Expressions.ExpressionType.Happy));
                break;
            case Expressions.ExpressionType.Sad:
                PlayAction(Random.Range(0, 3), false, Random.Range(0, 2));
                if(!isToNotUseStudentRenders) 
                    GetComponentInParent<StudentsHandler>().AddStudentRender(expressions.GetExpressionRender(Expressions.ExpressionType.Sad));
                break;
            case Expressions.ExpressionType.Excited:
                PlayAction(Random.Range(3, 6), true, Random.Range(0, 2));
                if(!isToNotUseStudentRenders) 
                    GetComponentInParent<StudentsHandler>().AddStudentRender(expressions.GetExpressionRender(Expressions.ExpressionType.Excited));
                break;
            case Expressions.ExpressionType.Angry0:
                if(!isToNotUseStudentRenders) 
                    GetComponentInParent<StudentsHandler>().AddStudentRender(expressions.GetExpressionRender(Expressions.ExpressionType.Angry0));
                break;
            case Expressions.ExpressionType.Angry1:
                if(!isToNotUseStudentRenders) 
                    GetComponentInParent<StudentsHandler>().AddStudentRender(expressions.GetExpressionRender(Expressions.ExpressionType.Angry1));
                break;
            case Expressions.ExpressionType.Frustrated:
                PlayAction(Random.Range(0, 3), false, Random.Range(0, 2));
                if(!isToNotUseStudentRenders) 
                    GetComponentInParent<StudentsHandler>().AddStudentRender(expressions.GetExpressionRender(Expressions.ExpressionType.Frustrated));
                break;
            case Expressions.ExpressionType.Surprised:
                if(!isToNotUseStudentRenders) 
                    GetComponentInParent<StudentsHandler>().AddStudentRender(expressions.GetExpressionRender(Expressions.ExpressionType.Surprised));
                break;
            default:
                break;
        }
    }
    private void InvokeMoveStudent()
    {
        expressions.ShowRandomExpression();
        MoveStudent();
    }
    public void MoveStudent(bool avatarMasking = false)
    {
        transform.DORotate(_targetRotation, 0.5f);
        PlayAnimation(1, avatarMasking);
        transform.DOMove(_targetPosition, _movementDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            PlayAnimation(0);
            EventManager.InvokeStudentReachedDestination(transform);
        });
    }
    private void PlayAnimation(int animationNo, bool avatarMasking = false)
    {
        animator.SetInteger(AnimationNo, animationNo);
        if(avatarMasking)
            animator.SetLayerWeight(1, 1f);
    }
    public void PlayAnimation(string triggerName, Expressions.ExpressionType expression)
    {
        animator.SetTrigger(triggerName);
        expressions.ShowExpression(expression);
    }
    private void PlayAction(int actionNo, bool isHappy, int motivationLevel)
    {
        animator.SetBool(Happy, isHappy);
        animator.SetInteger(MotivationLevel, motivationLevel);
        animator.SetInteger(AnimationNo, 2);
        animator.SetInteger(ActionNo, actionNo);
        animator.SetLayerWeight(1, 1f);
    }
    public void PlayClaimingAnimation()
    {
        animator.SetTrigger(Claim);
        animator.SetInteger(AnimationNo, 3);
        expressions.ShowExpression(Expressions.ExpressionType.Angry1);
    }
    public Animator GetAnimator()
    {
        return animator;
    }
    public Gender GetGender()
    {
        return _gender;
    }
    public void SurprisedLook()
    {
        PlayAnimation(1);
        expressions.ShowExpression(Expressions.ExpressionType.Surprised);
    }
    public Sprite GetRender()
    {
        return _render;
    }
}
public enum Gender
{
    MaleStudent,
    FemaleStudent
}
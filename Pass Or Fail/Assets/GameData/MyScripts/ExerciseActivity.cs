using System.Collections;
using UnityEngine;
public class ExerciseActivity : MonoBehaviour
{
    [SerializeField] private GameObject[] teachers;
    [SerializeField] private Animator[] students;
    private Animator _teacher;
    [SerializeField] private GameObject canvas, firstStep, tutorial;
    private const float Duration = 0.5f;
    private float _timeCounter = 0f;
    private readonly WaitForSeconds _delay = new (2f), _delay1 = new (0.5f);
    private static readonly int SpineMultiplier = Animator.StringToHash(SpineMultiplierString);
    private static readonly int RightHandMultiplier = Animator.StringToHash("RightHandMultiplier");
    private static readonly int LeftHandMultiplier = Animator.StringToHash("LeftHandMultiplier");
    private const string SpineMultiplierString = "SpineMultiplier";
    private void Start()
    {
        _teacher = teachers[PlayerPrefsHandler.currentTeacher].GetComponent<Animator>();
        _teacher.gameObject.SetActive(true);
    }
    public void StartActivity()
    {
        firstStep.SetActive(true);
        tutorial.SetActive(true);
        SharedUI.Instance.gamePlayUIManager.controls.EnableActivityUI(false);
    }
    private void EndActivity()
    {
        canvas.SetActive(false);
        GamePlayManager.Instance.LevelComplete(1f);
    }
    public void SetTeacher(Animator newTeacher)
    {
        _teacher = newTeacher;
    }
    public void SetSpeedMultiplier(float newValue, string parameterName)
    {
        _teacher.SetFloat(parameterName, newValue);
    }
    public void SetAnimatorLayerWeight(int layerNo, float newValue)
    {
        _teacher.SetLayerWeight(layerNo, _teacher.GetLayerWeight(layerNo) + newValue);
    }
    public void SetAnimatorLayerWeight(int layerNo)
    {
        StartCoroutine(SetLayerWeight(layerNo));
    }
    private IEnumerator SetLayerWeight(int layerNo)
    {
        _timeCounter = 0f;
        while (_timeCounter < Duration)
        {
            _timeCounter += Time.deltaTime / Duration;
            _teacher.SetLayerWeight(layerNo, _teacher.GetLayerWeight(layerNo) + _timeCounter);
            yield return null;
        }
    }
    public void Exercise()
    {
        SetStudentsStandingPose();
        StartCoroutine(DoExercise());
    }
    private IEnumerator DoExercise()
    {
        yield return _delay1;
        SetSpeedMultiplier(1.2f, SpineMultiplierString);
        yield return _delay;
        EndActivity();
    }
    public void SetStudentsDownPose()
    {
        foreach (var t in students)
        {
            StartCoroutine(DelayToSetStudentsDownPose(t));
        }
    }
    private IEnumerator DelayToSetStudentsDownPose(Animator studentAnimator)
    {
        studentAnimator.SetFloat(SpineMultiplier, 1.2f);
        studentAnimator.SetFloat(RightHandMultiplier, 1.2f);
        yield return _delay1;
        studentAnimator.SetFloat(SpineMultiplier, 0f);
        studentAnimator.SetFloat(RightHandMultiplier, 0f);
    }
    public void SetStudentsStandingPose()
    {
        foreach (var t in students)
        {
            StartCoroutine(DelayToSetStudentStandingPose(t));
        }
    }
    private IEnumerator DelayToSetStudentStandingPose(Animator studentAnimator)
    {
        studentAnimator.SetFloat(SpineMultiplier, -1.2f);
        studentAnimator.SetFloat(LeftHandMultiplier, 1.2f);
        yield return _delay1;
        studentAnimator.SetFloat(SpineMultiplier, 1.2f);
        studentAnimator.SetFloat(LeftHandMultiplier, 0f);
    }
}
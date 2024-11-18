using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
public class LevelBasedParams : MonoBehaviour
{
    private bool _inProgressFlag = false;
    private int _counter = 0;
    public enum ActivityType
    {
        QuestionAnswer,
        AttendanceMarking,
        LibraryDrill,
        RecessRound,
        SchoolDance,
        OralQuiz,
        UniformChecking,
        BadgesDistribution,
        ExerciseActivity,
        PianoLesson
    }
    [EnumPaging]
    [SerializeField] private EnvironmentManager.Environment environment;
    [EnumPaging]
    [SerializeField] private ActivityType activityType;
    private QuestionAnswer _questionAnswer;
    private LibraryDiscipline _libraryDiscipline;
    private CorridorActivity _corridorActivity;
    private DanceActivity _danceActivity;
    private OralQuiz _oralQuiz;
    private UniformChecking _uniformChecking;
    private BadgesDistribution _badgesDistribution;
    private ExerciseActivity _exerciseActivity;
    private AttendanceMarking _attendanceMarking;
    private PianoLesson _pianoLesson;
    private readonly UnityEvent _onPass = new UnityEvent();
    private readonly UnityEvent _onFail = new UnityEvent();

    private void Start()
    {
        SharedUI.Instance.gamePlayUIManager.controls.SetStreakCounterText();
        switch (activityType)
        {
            case ActivityType.QuestionAnswer:
                _inProgressFlag = true;
                SharedUI.Instance.gamePlayUIManager.controls.EnableQuestionAnswerUI(true);
                SetQuestionAnswer();
                if (_questionAnswer.IsSaveTheEgg())
                {
                    SharedUI.Instance.gamePlayUIManager.controls.SetProtectionText(_questionAnswer.GetMainInstructions(), _questionAnswer.GetDescription0(), 
                        _questionAnswer.GetDescription1());
                    SharedUI.Instance.gamePlayUIManager.controls.EnableProtectTheEggUI();
                }
                _questionAnswer.SetCameraView();
                break;
            case ActivityType.AttendanceMarking:
                if (_attendanceMarking == null)
                    _attendanceMarking = GetComponent<AttendanceMarking>();
                SharedUI.Instance.gamePlayUIManager.controls.EnableTapToPlay(true);
                GamePlayManager.Instance.mainCamera.gameObject.SetActive(false);
                break;
            case ActivityType.LibraryDrill:
                if (_libraryDiscipline == null)
                    _libraryDiscipline = GetComponent<LibraryDiscipline>();
                SharedUI.Instance.gamePlayUIManager.controls.EnableActivityUI(true);
                SharedUI.Instance.gamePlayUIManager.controls.SetActivityInstructionsSprite(PlayerPrefsHandler.ActivitiesNames[0]);
                GamePlayManager.Instance.mainCamera.gameObject.SetActive(false);
                break;
            case ActivityType.RecessRound:
                if (_corridorActivity == null)
                    _corridorActivity = GetComponent<CorridorActivity>();
                SharedUI.Instance.gamePlayUIManager.controls.EnableTapToPlay(true);
                GamePlayManager.Instance.mainCamera.gameObject.SetActive(false);
                break;
            case ActivityType.SchoolDance:
                if (_danceActivity == null)
                    _danceActivity = GetComponent<DanceActivity>();
                SharedUI.Instance.gamePlayUIManager.controls.EnableActivityUI(true);
                SharedUI.Instance.gamePlayUIManager.controls.SetActivityInstructionsSprite(PlayerPrefsHandler.ActivitiesNames[2]);
                GamePlayManager.Instance.mainCamera.gameObject.SetActive(false);
            break;
            case ActivityType.OralQuiz:
                if (_oralQuiz == null)
                    _oralQuiz = GetComponent<OralQuiz>();
                SharedUI.Instance.gamePlayUIManager.controls.EnableTapToPlay(true);
                GamePlayManager.Instance.mainCamera.gameObject.SetActive(false);
                break;
            case ActivityType.UniformChecking:
                if (_uniformChecking == null)
                    _uniformChecking = GetComponent<UniformChecking>();
                SharedUI.Instance.gamePlayUIManager.controls.EnableTapToPlay(true);
                GamePlayManager.Instance.mainCamera.gameObject.SetActive(false);
                break;
            case ActivityType.BadgesDistribution:
                if (_badgesDistribution == null)
                    _badgesDistribution = GetComponent<BadgesDistribution>();
                SharedUI.Instance.gamePlayUIManager.controls.EnableTapToPlay(true);
                GamePlayManager.Instance.mainCamera.gameObject.SetActive(false);
                break;
            case ActivityType.ExerciseActivity:
                if (_exerciseActivity == null)
                    _exerciseActivity = GetComponent<ExerciseActivity>();
                SharedUI.Instance.gamePlayUIManager.controls.EnableActivityUI(true);
                SharedUI.Instance.gamePlayUIManager.controls.EnableInfinityHandUI(false);
                SharedUI.Instance.gamePlayUIManager.controls.SetActivityInstructionsSprite(PlayerPrefsHandler.ActivitiesNames[7]);
                GamePlayManager.Instance.mainCamera.gameObject.SetActive(false);
                break;
            case ActivityType.PianoLesson:
                if (_pianoLesson == null)
                    _pianoLesson = GetComponent<PianoLesson>();
                SharedUI.Instance.gamePlayUIManager.controls.EnableTapToPlay(true);
                GamePlayManager.Instance.mainCamera.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void SetupTutorial()
    {
        if (PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialString)) return;
        if (!PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialStep0String))
        {
            SharedUI.Instance.gamePlayUIManager.controls.SetHandTutorial(_questionAnswer.IsAnswerRight());
        }
    }
    public EnvironmentManager.Environment GetEnvironment()
    {
        return environment;
    }
    private void SetQuestionAnswer()
    {
        if(!PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialString))
            SharedUI.Instance.gamePlayUIManager.controls.DisableHandTutorial();
        if (_questionAnswer == null)
        {
            _questionAnswer = GetComponent<QuestionAnswer>();
            _onPass.AddListener(RegisterPassing);
            _onFail.AddListener(RegisterFailing);
        }
        _questionAnswer.SetQuestion();
        if(!_questionAnswer.IsEarthGlobe())
            Invoke(nameof(DeactivateInProgressFlag), 0.5f);
    }
    private void RegisterPassing()
    {
        _questionAnswer.OnPass();
    }
    private void RegisterFailing()
    {
        _questionAnswer.OnFail();
    }
    public void Pass()
    {
        if(_inProgressFlag) return;
        _inProgressFlag = true;
        //Debug.Log("Pass has been called");
        _counter++;
        SharedUI.Instance.gamePlayUIManager.controls.SetProgress();
        _onPass?.Invoke();
        if (_counter >= 3) return;
        if(!_questionAnswer.IsStudentClaiming()) 
            Invoke(nameof(SetQuestionAnswer), 2f);
    }
    public void Fail()
    {
        if(_inProgressFlag) return;
        _inProgressFlag = true;
        //Debug.Log("Fail has been called");
        _counter++;
        SharedUI.Instance.gamePlayUIManager.controls.SetProgress();
        _onFail?.Invoke();
        if (_counter >= 3) return;
        if(!_questionAnswer.IsStudentClaiming()) 
            Invoke(nameof(SetQuestionAnswer), 2f);
    }
    public void AskQuestionAfterClaim()
    {
        if (_counter >= 3) return;
        Invoke(nameof(SetQuestionAnswer), 2f);
    }
    public void DeactivateInProgressFlag()
    {
        _inProgressFlag = false;
        SetupTutorial();
    }
    public void StartActivity()
    {
        if(_libraryDiscipline)
            _libraryDiscipline.StartActivity();
        else if(_corridorActivity)
            _corridorActivity.StartActivity();
        else if(_danceActivity)
            _danceActivity.StartActivity();
        else if(_oralQuiz)
            _oralQuiz.StartActivity();
        else if(_uniformChecking)
            _uniformChecking.StartActivity();
        else if(_badgesDistribution)
            _badgesDistribution.StartActivity();
        else if(_exerciseActivity)
            _exerciseActivity.StartActivity();
        else if(_attendanceMarking)
            _attendanceMarking.StartActivity();
        else if (_pianoLesson)
            _pianoLesson.StartActivity();
    }
    public void TeacherGoesBackToNormal(string action)
    {
        _corridorActivity.TeacherRotationBackToNormal(action);
    }
    public bool[] GetResults()
    {
        return _questionAnswer.GetResults();
    }
    public int GetGradingValue()
    {
        if (_questionAnswer)
            return _questionAnswer.GetGradingValue();
        else
            return 3;
    }
    public Sprite[] GetRenders()
    {
        return _questionAnswer.GetStudentsRenders();
    }
    public void CorrectMistake()
    {
        _questionAnswer.CorrectMistake();
    }
}
using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
public class QuestionAnswer : MonoBehaviour
{
    public enum QuestionsType
    {
        SimpleQuestions,
        EarthGlobeQuestions,
        ArtQuestions,
        BodyPartsQuestions,
        ScienceQuestions,
        SaveTheEggs
    }
    [SerializeField] private StudentsHandler studentsHandler;
    [EnumPaging]
    [SerializeField] private QuestionsType questionsType;
    [ShowIf("questionsType", QuestionsType.SimpleQuestions)]
    [SerializeField] private SpriteRenderer questionSpriteRenderer;
    [ShowIf("questionsType", QuestionsType.SimpleQuestions)]
    [SerializeField] private ParticleSystem questionParticles;
    [ShowIf("questionsType", QuestionsType.SimpleQuestions)]
    [SerializeField] private Questions simpleQuestions;
    [ShowIf("questionsType", QuestionsType.SimpleQuestions)]
    [SerializeField] private Animator teacherIkAnimator;
    [ShowIf("questionsType", QuestionsType.SimpleQuestions)]
    [SerializeField] private SpriteRenderer statusSpriteRenderer;
    [ShowIf("questionsType", QuestionsType.SimpleQuestions)]
    [SerializeField] private Sprite passStatusSprite, failStatusSprite;
    [SerializeField] private Transform cameraPosition;
    private readonly bool[] _resultFlags = {false, false, false};
    [SerializeField] private int[] questionsIndexes = {-1, -1, -1};
    private int _rightGradingValue = 0;
    private int _questionIndex = 0, _questionCounter = 0;
    private Globe _globe;
    private SaveTheEgg _saveTheEgg;
    private bool _isQuestionsSelected = false, _isStudentClaiming = false;
    
    [Header("For Fixed Questions")]
    [SerializeField] private int[] fixedQuestionsIndexes = {0, 1, 2};
    private void OnEnable()
    {
        Callbacks.OnRewardStreak += RewardStreak;
        Callbacks.OnRewardMistakeCorrection += CorrectMistake;
        if(questionsType == QuestionsType.EarthGlobeQuestions)
            GamePlayManager.Instance.environmentManager.DisableNativeAdOfClass();
        else if(questionsType == QuestionsType.SaveTheEggs)
            Invoke(nameof(DisableGadsmeAds), 0.25f);
    }
    private void OnDisable()
    {
        Callbacks.OnRewardStreak -= RewardStreak;
        Callbacks.OnRewardMistakeCorrection -= CorrectMistake;
    }
    private void DisableGadsmeAds()
    {
        if(GadsmeInit.Instance)
            GadsmeInit.Instance.DisableAds();
    }
    public void SetQuestion()
    {
        if(_questionCounter < 3)
            _questionCounter++;
        if(questionSpriteRenderer)
            questionSpriteRenderer.transform.parent.gameObject.SetActive(true);
        //if(questionCounter > 3) return;
        switch (questionsType)
        {
            case QuestionsType.SimpleQuestions:
                //questionIndex = Random.Range(0, simpleQuestions.questions.Length);
                //_questionIndex = GetQuestionIndex();
                SelectQuestions();
                _questionIndex = questionsIndexes[_questionCounter - 1];
                questionSpriteRenderer.sprite = simpleQuestions.questions[_questionIndex].questionAnswerSprite;
                //Debug.Log("_questionIndex: " + _questionIndex);
                statusSpriteRenderer.sprite = null;
                questionParticles.Play();
                break;
            case QuestionsType.EarthGlobeQuestions:
                //if(questionIndex >= simpleQuestions.questions.Length) return;
                if (_globe == null)
                    _globe = GetComponent<Globe>();
                _globe.SelectCountryToAsk();
                break;
            case QuestionsType.ArtQuestions:
                break;
            case QuestionsType.BodyPartsQuestions:
                break;
            case QuestionsType.ScienceQuestions:
                break;
            case QuestionsType.SaveTheEggs:
                if (_saveTheEgg == null)
                {
                    _saveTheEgg = GetComponent<SaveTheEgg>();
                    _saveTheEgg.InvokeSetEggs();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public bool IsAnswerRight()
    {
        switch (questionsType)
        {
            case QuestionsType.SimpleQuestions:
                return simpleQuestions.questions[_questionIndex].isRight;
            case QuestionsType.EarthGlobeQuestions:
                _questionIndex++;
                return _globe.IsRightAnswer();
            case QuestionsType.ArtQuestions:
                return false;
            case QuestionsType.BodyPartsQuestions:
                return false;
            case QuestionsType.ScienceQuestions:
                return false;
            case QuestionsType.SaveTheEggs:
                return _saveTheEgg.IsRightAnswer();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public void OnPass()
    {
        //Debug.Log("OnPass");
        _resultFlags[_questionCounter - 1] = true;
        if(teacherIkAnimator)
            teacherIkAnimator.Play("Pass");
        studentsHandler.ExitStudent(Expressions.ExpressionType.Happy);
        if (IsAnswerRight())
        {
            //Debug.Log("Good");
            SoundController.Instance.PlayCorrectGradingSound();
            SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Perfects);
            _rightGradingValue++;
            GamePlayManager.Instance.LevelEndRewardValue += 10;
            if (PlayerPrefsHandler.streak <= 0)
                PlayerPrefsHandler.streak = 1;
            else
                PlayerPrefsHandler.streak++;
            SharedUI.Instance.gamePlayUIManager.controls.EnableStreakAdBtn(false);
            Invoke(nameof(ShowGoodEffect), 0.5f);
        }
        else
        {
            //Debug.Log("Bad");
            SoundController.Instance.PlayWrongGradingSound();
            SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Warnings);
            if (PlayerPrefsHandler.streak > 0)
            {
                GamePlayManager.Instance.LastStreakValue = PlayerPrefsHandler.streak;
                PlayerPrefsHandler.streak = 0;
                SharedUI.Instance.gamePlayUIManager.controls.EnableStreakAdBtn(true);
            }
            /*else
                PlayerPrefsHandler.streak--;*/
            Invoke(nameof(ShowBadEffect), 0.5f);
        }
        SharedUI.Instance.gamePlayUIManager.controls.SetStreakCounterText();
    }
    public void OnFail()
    {
        //Debug.Log("OnFail: " + questionIndex);
        _resultFlags[_questionCounter - 1] = false;
        if(teacherIkAnimator)
            teacherIkAnimator.Play("Fail");
        if (IsAnswerRight())
        {
            //Debug.Log("Bad");
            if (PlayerPrefsHandler.LevelCounter > 1)
            {
                StudentClaim();
            }
            else
                studentsHandler.ExitStudent(Expressions.ExpressionType.Sad);
            SoundController.Instance.PlayWrongGradingSound();
            SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Warnings);
            if (PlayerPrefsHandler.streak > 0)
            {
                GamePlayManager.Instance.LastStreakValue = PlayerPrefsHandler.streak;
                PlayerPrefsHandler.streak = 0;
                SharedUI.Instance.gamePlayUIManager.controls.EnableStreakAdBtn(true);
            }
            /*else
                PlayerPrefsHandler.streak--;*/
            Invoke(nameof(ShowBadEffect), 0.5f);
        }
        else
        {
            //Debug.Log("Good");
            studentsHandler.ExitStudent(Expressions.ExpressionType.Sad);
            SoundController.Instance.PlayCorrectGradingSound();
            SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Perfects);
            _rightGradingValue++;
            if (PlayerPrefsHandler.streak <= 0)
                PlayerPrefsHandler.streak = 1;
            else
                PlayerPrefsHandler.streak++;
            SharedUI.Instance.gamePlayUIManager.controls.EnableStreakAdBtn(false);
            Invoke(nameof(ShowGoodEffect), 0.5f);
        }
        SharedUI.Instance.gamePlayUIManager.controls.SetStreakCounterText();
    }
    private void StudentClaim()
    {
        _isStudentClaiming = true;
        if(questionSpriteRenderer)
            questionSpriteRenderer.transform.parent.gameObject.SetActive(false);
        studentsHandler.StudentClaiming();
        SharedUI.Instance.gamePlayUIManager.controls.EnableMistakeUI(true);
    }
    public void CorrectMistake()
    {
        _isStudentClaiming = false;
        studentsHandler.ExitStudent(Expressions.ExpressionType.Excited);
        GamePlayManager.Instance.currentLevel.AskQuestionAfterClaim();
        SharedUI.Instance.gamePlayUIManager.controls.EnableMistakeUI(false);
    }
    public void SetStatusSprite(string status)
    {
        statusSpriteRenderer.sprite = status == PlayerPrefsHandler.Pass ? passStatusSprite : failStatusSprite;
    }
    private void ShowGoodEffect()
    {
        SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Good);
    }
    private void ShowBadEffect()
    {
        SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Bad);
    }
    public int GetGradingValue()
    {
        return _rightGradingValue;
    }
    public bool[] GetResults()
    {
        return _resultFlags;
    }
    public Sprite[] GetStudentsRenders()
    {
        return studentsHandler.GetStudentsRenders();
    }
    private void RewardStreak()
    {
        PlayerPrefsHandler.streak = GamePlayManager.Instance.LastStreakValue;
        GamePlayManager.Instance.LastStreakValue = 0;
        var controls = SharedUI.Instance.gamePlayUIManager.controls;
        controls.SetStreakCounterText();
        controls.EnableStreakAdBtn(false);
    }
    public bool IsSaveTheEgg()
    {
        return questionsType == QuestionsType.SaveTheEggs;
    }
    public bool IsEarthGlobe()
    {
        return questionsType == QuestionsType.EarthGlobeQuestions;
    }
    public void SetCameraView()
    {
        GamePlayManager.Instance.mainCamera.transform.position = cameraPosition.position;
    }
    private void SelectQuestions()
    {
        if(_isQuestionsSelected) return;
        _isQuestionsSelected = true;
        var limit = simpleQuestions.questions.Length;
        for (var j = 0; j < questionsIndexes.Length; j++)
        {
            if (GamePlayManager.Instance.AskFixedQuestions)
            {
                questionsIndexes[j] = fixedQuestionsIndexes[j];
                continue;
            }
            var newIndex = Random.Range(0, limit);
            while (IsIndexExists(newIndex))
            {
                newIndex = Random.Range(0, limit);
            }
            questionsIndexes[j] = newIndex;
        }
    }
    private bool IsIndexExists(int indexToCheck)
    {
        return questionsIndexes.Any(t => indexToCheck == t);
    }
    public bool IsStudentClaiming()
    {
        return _isStudentClaiming;  
    }
    public string GetMainInstructions()
    {
        return _saveTheEgg.GetMainInstructions();
    }
    public string GetDescription0()
    {
        return _saveTheEgg.GetDescription0();
    }
    public string GetDescription1()
    {
        return _saveTheEgg.GetDescription1();
    }
}
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class OralQuiz : MonoBehaviour
{
    private readonly Vector3 _playingPos = new(0f, 1.722f, 0f);
    private readonly Vector3 _endingPos = new(0.112f, 1.384f, 0.130f);
    private readonly Vector3 _endingRot = new(17.336f, 0f, 0f);
    private readonly Vector3 _playingRot = new(17.336f, 0f, 0f);
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private List<OralQuizStudent> oralQuizStudents;
    private List<OralQuizStudent> _tempOralQuizStudents = new List<OralQuizStudent>();
    [SerializeField] private OralQuestion[] oralQuestions;
    private int _questionIndex = 0;
    private OralQuizStudent _currentStudent;
    [Header("UI Elements"), SerializeField]
    private GameObject quizCanvas;
    [SerializeField]
    private GameObject questionUI;
    [SerializeField] private GameObject answerUI, perfects, shouts;
    private Camera _camera;
    private const float MIN_FIELD_OF_VIEW = 50f, MAX_FIELD_OF_VIEW = 55f, TRANSITION_DURATION = 0.5f, INVOKE_DELAY = 2f;
    private const string Camera = "Camera";
    public void StartActivity()
    {
        SetPlayingView();
        AskQuestion();
    }
    private void AskQuestion()
    {
        SetQuestionUI();
        GetStudentsToAnswer();
    }
    private void NextQuestion()
    {
        answerUI.SetActive(false);
        SetPlayingView();
        AskQuestion();
    }
    private void SetPlayingView()
    {
        cameraPivot.DOLocalMove(_playingPos, TRANSITION_DURATION);
        cameraPivot.DOLocalRotate(_playingRot, TRANSITION_DURATION);
        if(!_camera)
            _camera = cameraPivot.Find(Camera).GetComponent<Camera>();
        _camera.DOFieldOfView(MAX_FIELD_OF_VIEW, TRANSITION_DURATION).SetEase(Ease.Linear);
    }
    private void SetFocusView()
    {
        cameraPivot.DOLookAt(_currentStudent.GetHeadPosition(), TRANSITION_DURATION);
        if(!_camera)
            _camera = cameraPivot.Find(Camera).GetComponent<Camera>();
        _camera.DOFieldOfView(MIN_FIELD_OF_VIEW, TRANSITION_DURATION).SetEase(Ease.Linear);
    }
    private void SetQuestionUI()
    {
        questionUI.SetActive(false);
        questionUI.transform.Find(PlayerPrefsHandler.Text).GetComponent<Text>().text = oralQuestions[_questionIndex].questionString;
        questionUI.SetActive(true);
    }
    private void GetStudentsToAnswer()
    {
        _tempOralQuizStudents = new List<OralQuizStudent>(oralQuizStudents);
        var randomValue = Random.Range(0, _tempOralQuizStudents.Count);
        _tempOralQuizStudents.RemoveAt(randomValue);
        randomValue = Random.Range(0, _tempOralQuizStudents.Count);
        _tempOralQuizStudents.RemoveAt(randomValue);
        AssignAnswers();
    }
    private void AssignAnswers()
    {
        var totalStudentsToAnswer = _tempOralQuizStudents.Count;
        for (var i = 0; i < totalStudentsToAnswer; i++)
        {
            var r = Random.Range(0, totalStudentsToAnswer);
            while (IsAlreadyAssigned(oralQuestions[_questionIndex].allAnswers[r], i))
            {
                r = Random.Range(0, totalStudentsToAnswer);
            }
            _tempOralQuizStudents[i].ReadyToAnswer(true, oralQuestions[_questionIndex].allAnswers[r]);
        }
    }
    private bool IsAlreadyAssigned(string valueToCheck, int excludedIndex)
    {
        for (var i = 0; i < _tempOralQuizStudents.Count; i++)
        {
            if(i != excludedIndex)
                if (valueToCheck == _tempOralQuizStudents[i].GetStudentAnswer())
                    return true;
        }
        return false;
    }
    private void ReadyStudentsToAnswer()
    {
        SetPlayingView();
        foreach (var t in _tempOralQuizStudents)
        {
            t.ReadyToAnswer(true);
        }
    }
    public void OnGivingAnswer(OralQuizStudent student)
    {
        _currentStudent = student;
        SetFocusView();
        foreach (var t in _tempOralQuizStudents)
        {
            t.ReadyToAnswer(false);
        }
        var ans = _currentStudent.GetStudentAnswer();
        SetAnswerUI(_currentStudent.GetStudentAnswer());
        _tempOralQuizStudents.Remove(_currentStudent);
        if (IsAnswerRight(ans))
        {
            OnRightAnswer();
            Invoke(nameof(RightAnswerAction), INVOKE_DELAY);
        }
        else
        {
            OnWrongAnswer();
            Invoke(nameof(WrongAnswerAction), INVOKE_DELAY);
        }
    }
    private void SetAnswerUI(string studentAnswer)
    {
        answerUI.transform.Find(PlayerPrefsHandler.Text).GetComponent<Text>().text = studentAnswer;
        answerUI.SetActive(true);
    }
    private bool IsAnswerRight(string ans)
    {
        return ans == oralQuestions[_questionIndex].rightAnswer;
    }
    private void OnRightAnswer()
    {
        _currentStudent.ShowEmotion(Expressions.ExpressionType.Happy, true);
        ShowPerfects(PlayerPrefsHandler.Perfects);
        SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Good);
    }
    private void OnWrongAnswer()
    {
        _currentStudent.ShowEmotion(Expressions.ExpressionType.Sad, true);
        ShowPerfects(PlayerPrefsHandler.Shouts);
        SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Bad);
    }
    private void RightAnswerAction()
    {
        if (_questionIndex == 1)
        {
            // end of activity
            EndActivity();
            return;
        }
        _questionIndex++;
        NextQuestion();
    }
    private void WrongAnswerAction()
    {
        if (_tempOralQuizStudents.Count == 0)
        {
            if (_questionIndex == 1)
            {
                // end of activity
                EndActivity();
                return;
            }
            _questionIndex++;
            NextQuestion();
        }
        else
        {
            ReadyStudentsToAnswer();   
        }
    }
    private void EndActivity()
    {
        quizCanvas.SetActive(false);
        GamePlayManager.Instance.LevelComplete(1f);
    }
    public void ShowPerfects(string type)
    {
        switch (type)
        {
            case PlayerPrefsHandler.Perfects:
                perfects.SetActive(true);
                break;
            default:
                shouts.SetActive(true);
                break;
        }
    }
    [Serializable]
    public class OralQuestion
    {
        public string questionString;
        public string rightAnswer;
        public string[] allAnswers;
    }
}
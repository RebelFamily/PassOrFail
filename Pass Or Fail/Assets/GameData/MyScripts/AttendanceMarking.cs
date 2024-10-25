using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class AttendanceMarking : MonoBehaviour
{
    [SerializeField] private List<AttendanceStudent> attendanceStudents;
    [SerializeField] private AttendanceData[] attendanceData;
    private int _attendanceIndex = 0;
    private AttendanceStudent _currentStudent;
    [Header("UI Elements"), SerializeField]
    private GameObject attendanceCanvas;
    [SerializeField]
    private GameObject attendanceUI, attendanceButtons;
    [SerializeField] private GameObject perfects, shouts;
    private Camera _camera;
    private const string RenderString ="Render", IsString = "Is ", PresentString = " Present?";
    public void StartActivity()
    {
        AskForAttendance();
    }
    private void AskForAttendance()
    {
        SetAttendanceUI();
        _currentStudent = GetStudentToMarkAttendance();
        if (_currentStudent)
        {
            _currentStudent.RaiseUpTheHand();
        }
    }
    private void NextAttendance()
    {
        _attendanceIndex++;
        if (_attendanceIndex >= attendanceData.Length)
        {
            attendanceCanvas.SetActive(false);
            GamePlayManager.Instance.LevelComplete(0.5f);
            return;
        }
        AskForAttendance();
    }
    private void SetAttendanceUI()
    {
        attendanceUI.SetActive(false);
        attendanceUI.transform.Find(PlayerPrefsHandler.Text).GetComponent<Text>().text = IsString + attendanceData[_attendanceIndex].studentName + PresentString;
        attendanceUI.transform.Find(RenderString).GetComponent<Image>().sprite =
            attendanceData[_attendanceIndex].studentRender;
        attendanceUI.SetActive(true);
        attendanceButtons.SetActive(true);
    }
    private AttendanceStudent GetStudentToMarkAttendance()
    {
        var attendance = attendanceData[_attendanceIndex];
        if (attendance.isPresent)
        {
            return attendanceStudents[attendance.studentIndexToMark];
        }
        else
        {
            return attendance.isStudentCheating ? attendanceStudents[Random.Range(0, attendanceStudents.Count)] : null;
        }
    }
    public void MarkPresent()
    {
        RaiseDownTheHand();
        if (attendanceData[_attendanceIndex].isPresent)
        {
            OnRightAnswer();
        }
        else
        {
            OnWrongAnswer();
        }
    }
    public void MarkAbsent()
    {
        RaiseDownTheHand();
        if (!attendanceData[_attendanceIndex].isPresent)
        {
            OnRightAnswer();
        }
        else
        {
            OnWrongAnswer();
        }
    }
    private void OnRightAnswer()
    {
        if (_currentStudent)
        {
            _currentStudent.ShowEmotion(Expressions.ExpressionType.Happy, true);
        }
        ShowPerfects(PlayerPrefsHandler.Perfects);
        SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Good);
    }
    private void OnWrongAnswer()
    {
        if (_currentStudent)
        {
            if(attendanceData[_attendanceIndex].isStudentCheating)
                _currentStudent.ShowEmotion(Expressions.ExpressionType.Happy, true);
            else
                _currentStudent.ShowEmotion(Expressions.ExpressionType.Sad, true);
        }
        ShowPerfects(PlayerPrefsHandler.Shouts);
        SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Bad);
    }
    private void RaiseDownTheHand()
    {
        attendanceButtons.SetActive(false);
        if(_currentStudent)
            _currentStudent.RaiseDownTheHand();
        Invoke(nameof(NextAttendance), 1f);
    }
    private void ShowPerfects(string type)
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
    public class AttendanceData
    {
        public string studentName;
        public Sprite studentRender;
        public int studentIndexToMark;
        public bool isPresent;
        public bool isStudentCheating;
    }
}
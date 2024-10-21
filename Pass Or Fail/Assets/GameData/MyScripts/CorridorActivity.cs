using System;
using System.Collections;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;
public class CorridorActivity : MonoBehaviour
{
    [SerializeField] private SplineFollower splineFollower;
    private ReportCardUI reportCardUI;
    private Transform currentStudent;
    [SerializeField] private ReportCard[] reportCardEntries;
    private int _counter = 0;
    private float _movementSpeed = 1f;
    
    public void StartActivity()
    {
        reportCardUI = SharedUI.Instance.gamePlayUIManager.controls.GetReportCard().GetComponent<ReportCardUI>();
        splineFollower.follow = true;
    }
    private void SetReportUI(int index)
    {
        reportCardUI.SetReportCardUI(reportCardEntries[index]);
    }
    private void LookTowardsTeacher(Transform student)
    {
        //Debug.Log("LookTowardsTeacher");
        currentStudent = student;
        var teacher = splineFollower.transform;
        var lookPos = teacher.position - student.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        var parent = currentStudent.parent;
        currentStudent.GetComponent<Student>().SurprisedLook();
        student.DORotateQuaternion(rotation, 0.75f).OnComplete(() =>
        {
            reportCardUI.gameObject.SetActive(true);
            SetReportUI(_counter);
        });
        var otherStudent = parent.Find("OtherStudent");
        if (otherStudent)
            otherStudent.DOMove(parent.Find("RunAwayPoint").position, 1f);
    }
    public void LookTowardsStudent(Transform student)
    {
        splineFollower.follow = false;
        currentStudent = student;
        var teacher = splineFollower.transform;
        var lookPos = student.position - teacher.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        splineFollower.transform.DORotateQuaternion(rotation, 1f).OnComplete(() =>
        {
            LookTowardsTeacher(student);
        });
    }
    public void TeacherRotationBackToNormal(string action)
    {
        StartCoroutine(DelayForNextAction(action));
    }
    private IEnumerator DelayForNextAction(string action)
    {
        var delay = 1.5f;
        yield return new WaitForSeconds(0.5f);
        var spray = splineFollower.transform.Find("Spray").gameObject;
        SharedUI.Instance.gamePlayUIManager.controls.SetProgress();
        var eulerAngles = splineFollower.transform.eulerAngles;
        var target = new Vector3(eulerAngles.x, 90f, eulerAngles.z);
        var runAwayPoint = currentStudent.parent.Find("RunAwayPoint");
        switch (action)
        {
            case "LetGo":
                delay = 1f;
                currentStudent.GetComponent<Student>().PlayAnimation("LetGo", Expressions.ExpressionType.Happy);
                break;
            case "SendToPrincipal":
                delay = 1.5f;
                currentStudent.GetComponent<Student>().PlayAnimation("Principle", Expressions.ExpressionType.Sad);
                break;
            case "Spray":
                delay = 2.3f;
                spray.SetActive(true);
                yield return new WaitForSeconds(0.7f);
                currentStudent.GetComponent<Student>().PlayAnimation("Spray", Expressions.ExpressionType.Frustrated);
                break;
        }
        yield return new WaitForSeconds(delay);
        spray.SetActive(false);
        currentStudent.DOMove(runAwayPoint.position, 1f).OnComplete(() =>
        {
            //movementSpeed++;
            if (_counter >= reportCardEntries.Length - 1)
            {
                GamePlayManager.Instance.LevelComplete(0f);
                return;
            }
            _counter++;
            splineFollower.transform.DORotate(target, 1f).OnComplete(() =>
            {
                splineFollower.follow = true;
            });
        });
    }
    [Serializable]
    public class ReportCard
    {
        public ReportCardEntry[] reportCardEntries;
        public bool isGood;
    }
    [Serializable]
    public class ReportCardEntry
    {
        public string description;
        public bool isGood;
    }
}
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public class StudentsHandler : MonoBehaviour
{
    [SerializeField] private List<Student> students;
    [SerializeField] private Transform[] studentPositions;
    [SerializeField] private Transform exitPosition;
    [ShowInInspector] private List<int> _selectedStudentsIndex = new ();
    [ShowInInspector] private List<Gender> _selectedStudentsGender = new ();
    private readonly List<Sprite> _studentsRenders = new ();
    private void Start()
    {
        for (var i = 0; i < students.Count; i++)
        {
            SelectStudents(students[i]);
        }
    }
    public void ExitStudent(Expressions.ExpressionType emotion)
    {
        if(students.Count == 0) return;
        students[0].ShowEmotion(exitPosition.position, exitPosition.eulerAngles, 4f, emotion);
        students.RemoveAt(0);
        if (students.Count == 0)
        {
            //Debug.Log("Complete");
            GamePlayManager.Instance.LevelComplete(3f);
        }
        else
        {
            var index = 0;
            foreach (var student in students)
            {
                student.ShowEmotion(studentPositions[index].position, studentPositions[index].eulerAngles, 2f);
                index++;
            }
        }
    }
    public void StudentClaiming()
    {
        students[0].PlayClaimingAnimation();
    }
    public void AddStudentRender(Sprite newRender)
    {
        _studentsRenders.Add(newRender);
    }
    public Sprite[] GetStudentsRenders()
    {
        return _studentsRenders.ToArray();
    }
    public Student[] GetStudents()
    {
        return students.ToArray();
    }
    private void SelectStudents(Student student)
    {
        var genderNo = Random.Range(0, 2);
        var index = Random.Range(0, 4);
        var gender = genderNo == 0 ? Gender.FemaleStudent : Gender.MaleStudent;
        while (_selectedStudentsIndex.Contains(index) && _selectedStudentsGender.Contains(gender))
        {
            genderNo = Random.Range(0, 2);
            index = Random.Range(0, 4);
            gender = genderNo == 0 ? Gender.FemaleStudent : Gender.MaleStudent;
        }
        _selectedStudentsIndex.Add(index);
        _selectedStudentsGender.Add(gender);
        student.SetStudent(gender, index);
    }
}
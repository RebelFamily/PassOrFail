using System;
using System.Collections.Generic;
using UnityEngine;
public class StudentsHandler : MonoBehaviour
{
    [SerializeField] private List<Student> students;
    [SerializeField] private Transform[] studentPositions;
    [SerializeField] private Transform exitPosition;
    [SerializeField] private Character[] femaleCharacters, maleCharacters;
    private readonly List<Sprite> _studentsRenders = new List<Sprite>();
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
    public Character GetCharacter(Gender gender, int index)
    {
        return gender == Gender.MaleStudent ? maleCharacters[index] : femaleCharacters[index];
    }
    public Student[] GetStudents()
    {
        return students.ToArray();
    }
    [Serializable] public class Character
    {
        public GameObject characterObject;
        public Sprite characterRender;
    }
}
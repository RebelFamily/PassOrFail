using System.Collections.Generic;
using UnityEngine;

namespace PassOrFail.MiniGames
{
    public class MiniGameStudentHandler : MonoBehaviour
    {
        [SerializeField] private List<Student> students;
        [SerializeField] private Transform[] studentPositions;
        [SerializeField] private Transform allowExitPosition,dontAllowExitPosition;

        private void Start()
        {
            students[0].ShowEmotion(studentPositions[0].position, studentPositions[0].eulerAngles, 2f);
        }

        public void ExitStudent(Expressions.ExpressionType emotion, bool isAllowed)
        {
            if (students.Count == 0) return;
            if(isAllowed) students[0].ShowEmotion(allowExitPosition.position, allowExitPosition.eulerAngles, 4f, emotion);
            else students[0].ShowEmotion(dontAllowExitPosition.position, dontAllowExitPosition.eulerAngles, 4f, emotion);
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
    }
}
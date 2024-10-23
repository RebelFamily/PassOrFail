using System;
using UnityEngine;

namespace PassOrFail.MiniGames
{
    public class EventManager
    {
        public static event Action OnStopPlayerFight;
        public static event Action<Transform> OnStudentReachedDestination;
        public static event Action OnStudentChecked;

        public static void InvokeStopPlayerFight()
        {
            OnStopPlayerFight?.Invoke();
        } 
        public static void InvokeStudentReachedDestination(Transform student)
        {
            OnStudentReachedDestination?.Invoke(student);
        }
        public static void InvokeStudentChecked()
        {
            OnStudentChecked?.Invoke();
        }
    }
}
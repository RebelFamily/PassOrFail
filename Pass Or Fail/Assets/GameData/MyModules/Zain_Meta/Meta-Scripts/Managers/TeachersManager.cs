using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.AI.Teacher;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class TeachersManager : MonoBehaviour
    {
        public static TeachersManager Instance;

        private void Awake()
        {
            Instance = this;
            sleepyTeacherIcon.gameObject.SetActive(false);
        }


        [SerializeField] private List<TeacherStateManager> availableTeachers = new();
        [SerializeField] private Button sleepyTeacherIcon;

        private void OnEnable()
        {
            sleepyTeacherIcon.onClick.AddListener(ClickOnCoffeeBtn);
            EventsManager.OnTeacherEnterSleepyState += CheckForShowingCoffeeBtn;
        }

        private void OnDisable()
        {
            sleepyTeacherIcon.onClick.RemoveListener(ClickOnCoffeeBtn);
            EventsManager.OnTeacherEnterSleepyState -= CheckForShowingCoffeeBtn;
        }

        private void CheckForShowingCoffeeBtn(bool hasEntered)
        {
            for (var i = 0; i < availableTeachers.Count; i++)
            {
                if (availableTeachers[i].IsSleepy())
                {
                    ShowSleepyTeacherIcon();
                    return;
                }
            }

            HideSleepyTeacherIcon();
        }

        public void AddNewTeacher(TeacherStateManager newTeacher)
        {
            if (availableTeachers.Contains(newTeacher)) return;

            availableTeachers.Add(newTeacher);
        }

        private void ShowSleepyTeacherIcon()
        {
            sleepyTeacherIcon.gameObject.SetActive(true);
        }

        private void HideSleepyTeacherIcon()
        {
            sleepyTeacherIcon.gameObject.SetActive(false);
        }

        private void ClickOnCoffeeBtn()
        {
            EventsManager.ClickedCoffeeButtonEvent();
        }
    }
}
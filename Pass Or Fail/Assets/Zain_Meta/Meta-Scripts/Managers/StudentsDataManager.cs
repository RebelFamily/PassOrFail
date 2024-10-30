using System.Collections.Generic;
using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;
using Zain_Meta.Meta_Scripts.DataRelated;
using Zain_Meta.Meta_Scripts.Helpers;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class StudentsDataManager : MonoBehaviour
    {
        public static StudentsDataManager Instance;


        [SerializeField] private StudentsData studentsData;
        [SerializeField] private Utility utility;
        [SerializeField] private List<StudentRequirements> students = new();
        [SerializeField] private Collider spawningArea;
        [SerializeField] private int initialSpawningCount;
        private const string FileName = "GameData/StudentsData.es3";
        private ES3Settings _settings;

        private void Awake()
        {
            Instance = this;
            LoadData();
        }

        #region Event Callbacks

        private void OnEnable()
        {
            EventsManager.OnStudentStateUpdated += SaveDataInFile;
            EventsManager.OnStudentLeftTheSchool += RemoveThisStudent;
        }

        private void OnDisable()
        {
            EventsManager.OnStudentStateUpdated -= SaveDataInFile;
            EventsManager.OnStudentLeftTheSchool -= RemoveThisStudent;
        }

        private void RemoveThisStudent(StudentRequirements student)
        {
            if (!students.Contains(student)) return;
            students.Remove(student);
            SaveDataInFile();
            SpawnUnAdmittedStudents();
        }

        #endregion


        private void Start()
        {
            SpawnLoadedStudents();
            for (var i = 0; i < initialSpawningCount; i++)
            {
                SpawnUnAdmittedStudents();
            }
        }


        public void AddStudentInTheSchool(StudentRequirements requirements,bool saveDataAlso)
        {
            if (!students.Contains(requirements))
            {
                students.Add(requirements);
            }

            if (saveDataAlso)
                SaveDataInFile();
        }

        private void SpawnLoadedStudents()
        {
            var studentsCount = studentsData.classesData.Count;

            print("Count of Students Is " + studentsCount);

            for (var i = 0; i < studentsCount; i++)
            {
                var student = utility.SpawnStudentAt(PointGenerator.RandomPointInBounds(spawningArea.bounds));
                student.GetRequirements().PopulateStates(studentsData.classesData[i].totalRides.ToArray(),
                    studentsData.classesData[i].curRideIndex);
                student.AdmitMePleaseForcefully();
            }
        }

        private void SpawnUnAdmittedStudents()
        {
            var student = utility.SpawnStudentAt(PointGenerator.RandomPointInBounds(spawningArea.bounds));
            student.ChangeState(student.EnterSchoolState);
        }

        private void SaveDataInFile()
        {
            studentsData.ClearAllStateData();
            for (var i = 0; i < students.Count; i++)
            {
                SaveDataState(students[i]);
            }

            ES3.Save("Students", studentsData, FileName);
        }

        private void SaveDataState(StudentRequirements studentRequirements)
        {
            studentsData.AddEachPersonData(
                studentRequirements.classesIndex.ToArray(),
                studentRequirements.curClassIndex
            );
        }

        private void LoadData()
        {
            studentsData = ES3.Load("Students", FileName, studentsData);
        }
    }
}
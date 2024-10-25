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
        [SerializeField] private Transform studentsSpawningPos;
        [SerializeField] private List<StudentRequirements> students = new();
        [SerializeField] private Collider spawningArea;
        private const string _fileName = "GameData/SaveFile.es3";
        private ES3Settings _settings;

        private void Awake()
        {
            Instance = this;
            LoadData();
        }

        private void Start()
        {
            SpawnLoadedStudents();
        }

        private void Update()
        {
            SpawnUnAdmittedStudents();
        }

        public void AddStudentInTheSchool(StudentRequirements requirements)
        {
            if (students.Contains(requirements)) return;
            students.Add(requirements);
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
            if (Input.GetMouseButtonDown(1))
            {
                var student = utility.SpawnStudentAt(studentsSpawningPos);
                student.ChangeState(student.EnterSchoolState);
                SaveDataInFile();
            }
        }

        private void SaveDataInFile()
        {
            studentsData.ClearAllStateData();
            for (var i = 0; i < students.Count; i++)
            {
                SaveDataState(students[i]);
            }

            ES3.Save("Students", studentsData);
            // ES3.StoreCachedFile();
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
            /*ES3.CacheFile(_fileName);
            _settings = new ES3Settings(ES3.Location.Cache);*/
            studentsData = ES3.Load("Students", studentsData);
        }
    }
}
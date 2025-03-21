﻿using System.Collections;
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
        [SerializeField] private List<StudentRequirements> students = new();
        [SerializeField] private int maxStudentsToAdmit;
        [SerializeField] private Collider spawningArea;
        [SerializeField] private int initialSpawningCount;

        private const string FileName = "GameData/StudentsData.es3";
        private const string SpawningPath = "Characters/Students/Student";
        private string[] _gendersArray = { "Boy", "Girl" };
        private ES3Settings _settings;
        private YieldInstruction _delay = new WaitForSeconds(.15f);

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
            EventsManager.OnStudentAdmitted += SpawnUnAdmittedStudents;
        }

        private void OnDisable()
        {
            EventsManager.OnStudentStateUpdated -= SaveDataInFile;
            EventsManager.OnStudentLeftTheSchool -= RemoveThisStudent;
            EventsManager.OnStudentAdmitted -= SpawnUnAdmittedStudents;
        }

        private void RemoveThisStudent(StudentRequirements student)
        {
            if (!students.Contains(student)) return;
            students.Remove(student);
            SaveDataInFile();
        }

        #endregion


        private void Start()
        {
            StartCoroutine(nameof(InitiateSpawning_CO));
        }

        private IEnumerator InitiateSpawning_CO()
        {
            var studentsCount = studentsData.classesData.Count;

            if (studentsCount > maxStudentsToAdmit)
                studentsCount = maxStudentsToAdmit;

            for (var i = 0; i < studentsCount; i++)
            {
                var gender = Random.Range(0, _gendersArray.Length);
                var index = Random.Range(0, 4);
                var student = Instantiate(Resources.Load<StudentStateManager>
                        (SpawningPath + _gendersArray[gender] + "_" + index),
                    PointGenerator.RandomPointInBounds(spawningArea.bounds),
                    Quaternion.identity);


                student.GetRequirements().PopulateStates(studentsData.classesData[i].totalRides.ToArray()
                    , studentsData.classesData[i].curClassIndex);
                student.AdmitMePleaseForcefully();
                yield return _delay;
            }

            yield return _delay;
            for (var i = 0; i < initialSpawningCount; i++)
            {
                var gender = Random.Range(0, _gendersArray.Length);
                var index = Random.Range(0, 4);
                var student = Instantiate(Resources.Load<StudentStateManager>
                        (SpawningPath + _gendersArray[gender] + "_" + index),
                    PointGenerator.RandomPointInBounds(spawningArea.bounds),
                    Quaternion.identity);
                student.ChangeState(student.EnterSchoolState);
            }
        }


        public void AddStudentInTheSchool(StudentRequirements requirements, bool saveDataAlso)
        {
            if (!students.Contains(requirements))
            {
                students.Add(requirements);
            }

            if (saveDataAlso)
                SaveDataInFile();
        }

        public void SpawnUnAdmittedStudents()
        {
            if (!OnBoardingManager.Instance.CanSpawnStudents()) return;
            if (students.Count >= maxStudentsToAdmit) return;
            var gender = Random.Range(0, _gendersArray.Length);
            var index = Random.Range(0, 4);
            var student = Instantiate(Resources.Load<StudentStateManager>
                    (SpawningPath + _gendersArray[gender] + "_" + index),
                PointGenerator.RandomPointInBounds(spawningArea.bounds),
                Quaternion.identity);
            student.ChangeState(student.EnterSchoolState);
        }

        public void SpawnExtra()
        {
            for (var i = 0; i < initialSpawningCount; i++)
            {
                var gender = Random.Range(0, _gendersArray.Length);
                var index = Random.Range(0, 4);
                var student = Instantiate(Resources.Load<StudentStateManager>
                        (SpawningPath + _gendersArray[gender] + "_" + index),
                    PointGenerator.RandomPointInBounds(spawningArea.bounds),
                    Quaternion.identity);
                student.ChangeState(student.EnterSchoolState);
            }
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
                studentRequirements.classesIndex.ToArray(), studentRequirements.curClassIndex);
        }

        private void LoadData()
        {
            studentsData = ES3.Load("Students", FileName, studentsData);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.MetaRelated;
using Zain_Meta.Meta_Scripts.MetaRelated.Upgrades;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class ClassroomProfilesManager : MonoBehaviour
    {
        public static ClassroomProfilesManager Instance;

        private SharedUI _shared;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _shared = SharedUI.Instance;
            if (_shared)
                _shared.HideAll();
        }

        [SerializeField] private List<ClassroomProfile> allClassrooms = new();
        [SerializeField] private WaitingLine waitingLine;
        [SerializeField] private Transform exitingPoint, leavingCashPoint;
        [SerializeField] private Transform[] randomPointsInCorridor;
        [SerializeField] private RandomPoint[] randomPoints;
        [SerializeField] private CashGenerationSystem graduationCash;
        [SerializeField] private int percentageIncreaseInGraduationAmount;
        private int _curRandomPoint;

        private void OnEnable()
        {
            EventsManager.OnClassReadyToUpgrade += IncreaseGraduationCash;
        }

        private void OnDisable()
        {
            EventsManager.OnClassReadyToUpgrade -= IncreaseGraduationCash;
        }

        private void IncreaseGraduationCash(ClassroomUpgradeProfile profile, bool val)
        {
            if (!val) return;
            var amountInPercent = (percentageIncreaseInGraduationAmount * graduationCash.amountToGive) / 100;
            graduationCash.amountToGive += amountInPercent;
        }

        public void AddClasses(ClassroomProfile newClass)
        {
            if (allClassrooms.Contains(newClass)) return;
            allClassrooms.Add(newClass);
        }


        public void AssignClasses(StudentRequirements student)
        {
            for (var i = 0; i < allClassrooms.Count; i++)
            {
                if (!student.classesIndex.Contains((int)allClassrooms[i].GetClassroomType()))
                    student.classesIndex.Add((int)allClassrooms[i].GetClassroomType());
            }
        }

        public ClassroomProfile GetThisClass(int index)
        {
            for (var i = 0; i < allClassrooms.Count; i++)
            {
                if ((int)allClassrooms[i].GetClassroomType() == index)
                    return allClassrooms[i];
            }

            return null;
        }

        // checking for the seats
        public bool CheckIfAnyClassIsFree()
        {
            if (allClassrooms.Count == 0) return false;
            for (var i = 0; i < allClassrooms.Count; i++)
            {
                if (allClassrooms[i].AnySeatsAvailable())
                    return true;
            }

            return false;
        }

        public Transform GetSeatAtRequiredClass(StudentStateManager student, int[] requiredClasses)
        {
            if (allClassrooms.Count == 0) return null;

            for (var i = student.GetRequirements().curClassIndex; i < requiredClasses.Length; i++)
            {
                for (var j = 0; j < allClassrooms.Count; j++)
                {
                    if ((int)allClassrooms[j].GetClassroomType() == requiredClasses[i])
                    {
                        var seat = allClassrooms[j].GetAvailableSeat(student);
                        if (seat)
                        {
                            student.GetRequirements().SwapClassIndices(i);
                            return seat;
                        }
                    }
                }
            }

            return null;
        }

        public Transform CheckForPointAtReception(StudentStateManager student)
        {
            if (!waitingLine) return null;

            return waitingLine.GetAvailableSpotAtReception(student);
        }
        

        public RandomPoint GetARandomWaitingPoint()
        {
            for (var i = 0; i < randomPoints.Length; i++)
            {
                if (randomPoints[i].IsEmpty())
                {
                    randomPoints[i].OccupyThis();
                    return randomPoints[i];
                }
            }

            return null;
        }

        public Transform GetExitingPoint() => exitingPoint;
        public Transform GetLeavingCashPoint() => leavingCashPoint;

        public void AddCashInGraduationStack(int amount, Transform pos)
        {
            graduationCash.AddCash(amount, pos);
        }
    }
}
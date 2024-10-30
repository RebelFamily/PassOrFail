using System.Collections.Generic;
using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Helpers;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class ClassroomProfilesManager : MonoBehaviour
    {
        public static ClassroomProfilesManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private List<ClassroomProfile> allClassrooms = new();
        [SerializeField] private WaitingLine waitingLine;
        [SerializeField] private Transform exitingPoint;
        [SerializeField] private Collider waitingAreasInCorridor;

        public void AddClasses(ClassroomProfile newClass)
        {
            if (allClassrooms.Contains(newClass)) return;
            allClassrooms.Add(newClass);
        }


        public void AssignClasses(StudentRequirements student)
        {
            for (var i = 0; i < allClassrooms.Count; i++)
            {
                if (!student.classesIndex.Contains(allClassrooms[i].GetClassroomType()))
                    student.classesIndex.Add(allClassrooms[i].GetClassroomType());
            }

            print("Classes Assigned to :" + student.name);
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


        // actually giving the seat
        public Transform GetAvailableSeat(StudentStateManager student)
        {
            if (allClassrooms.Count == 0) return null;
            for (var i = 0; i < allClassrooms.Count; i++)
            {
                var seat = allClassrooms[i].GetAvailableSeat(student);
                if (seat)
                    return seat;
            }

            return null;
        }

        public Transform GetSeatAtRequiredClass(StudentStateManager student, int[] requiredClasses)
        {
            if (allClassrooms.Count == 0) return null;

            for (var i = student.GetRequirements().curClassIndex; i < requiredClasses.Length; i++)
            {
                for (var j = 0; j < allClassrooms.Count; j++)
                {
                    if (allClassrooms[j].GetClassroomType() == requiredClasses[i])
                    {
                        var seat = allClassrooms[i].GetAvailableSeat(student);
                        if (seat)
                        {
                            student.GetRequirements().AdjustRidesIndices(i);
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
        public Vector3 GetRandomPointInCorridor() =>
            PointGenerator.RandomPointInBounds(waitingAreasInCorridor.bounds);

        public Transform GetExitingPoint() => exitingPoint;
    }
}
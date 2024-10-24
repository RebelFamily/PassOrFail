using System.Collections.Generic;
using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;
using Zain_Meta.Meta_Scripts.Components;

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

        public void AddClasses(ClassroomProfile newClass)
        {
            if (allClassrooms.Contains(newClass)) return;
            allClassrooms.Add(newClass);
        }

        // checking for the seats giving the seat
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
                if(seat)
                {
                    return seat;
                }
                
            }

            return null;
        }

        public Transform CheckForPointAtReception(StudentStateManager student)
        {
            if (!waitingLine) return null;

            return waitingLine.GetAvailableSpotAtReception(student);
        }
    }
}
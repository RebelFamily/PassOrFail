using UnityEngine;
using UnityEngine.Serialization;
using Zain_Meta.Meta_Scripts.AI;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class SeatProfile : MonoBehaviour
    {
        [SerializeField] private Transform reachingPoint;
        [SerializeField] private StudentStateManager studentSittingAtThisSpot;
        [SerializeField] private bool isOccupied;
        [SerializeField] private bool isMarked;

        private void OnEnable()
        {
            EventsManager.OnStudentLeftTheClassroom += EmptyMySpot;
        }

        private void OnDisable()
        {
            EventsManager.OnStudentLeftTheClassroom -= EmptyMySpot;
        }

        private void EmptyMySpot(StudentStateManager student)
        {
            if (student != studentSittingAtThisSpot) return;
            EmptyTheSpot();
        }


        public bool IsSeatOccupied() => isOccupied;
        public bool IsThisChairMarked() => isMarked;

        public void MarkForSitting(StudentStateManager newStudent)
        {
            studentSittingAtThisSpot = newStudent;
            isMarked = true;
        }

        public void ActuallySitOnThis()
        {
            isOccupied = true;
        }

        private void EmptyTheSpot()
        {
            isOccupied = false;
            isMarked = false;
            studentSittingAtThisSpot = null;
        }

        public void GiveHomeworkToThisKid()
        {
            if (!studentSittingAtThisSpot) return;
            print("Aa na puttar zara kaam kr!!");
            studentSittingAtThisSpot.StartLearning();
        }

        public Transform GetSeatingPoint() => reachingPoint;
    }
}
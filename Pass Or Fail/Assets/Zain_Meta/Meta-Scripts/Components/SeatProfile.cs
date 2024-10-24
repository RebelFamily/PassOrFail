using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class SeatProfile : MonoBehaviour
    {
        [SerializeField] private Transform reachingPoint;
        [SerializeField] private StudentStateManager studentSittingAtThisSpot;
        [SerializeField] private bool isOccupied;

        public bool IsSeatOccupied() => isOccupied;
        public void MarkForSitting(StudentStateManager newStudent)
        {
            studentSittingAtThisSpot = newStudent;
            isOccupied = true;
        }

        public void EmptyTheSpot()
        {
            isOccupied = false;
            studentSittingAtThisSpot = null;
        }

        public void GiveHomeworkToThisKid()
        {
            if(!studentSittingAtThisSpot) return;
            print("Aa na puttar zara kaam kr!!");
            studentSittingAtThisSpot.StartLearning();
        }

        public Transform GetSeatingPoint() => reachingPoint;
    }
}
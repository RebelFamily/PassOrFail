using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class ReceptionPoint : MonoBehaviour
    {
        [SerializeField] private Transform queuePoint;
        [SerializeField] private StudentStateManager studentAtThisPoint;
        [SerializeField] private bool isOccupied;
        public bool IsOccupied() => isOccupied;

        public void OccupyThis(StudentStateManager newStudent)
        {
            studentAtThisPoint = newStudent;
            isOccupied = true;
        }

        public void FreeTheSpot()
        {
            studentAtThisPoint = null;
            isOccupied = false;
        }

        public Transform GetQueuePoint() => queuePoint;
        public StudentStateManager GetStudentAtThisPoint() => studentAtThisPoint;
    }
}
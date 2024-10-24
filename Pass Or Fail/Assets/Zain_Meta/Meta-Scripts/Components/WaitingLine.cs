using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;
using Zain_Meta.Meta_Scripts.MetaRelated;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class WaitingLine : MonoBehaviour
    {
        [SerializeField] private ReceptionPoint[] receptionPoints;

        public Transform GetAvailableSpotAtReception(StudentStateManager student)
        {
            for (var i = 0; i < receptionPoints.Length; i++)
            {
                if (!receptionPoints[i].IsOccupied())
                {
                    receptionPoints[i].OccupyThis(student);
                    return receptionPoints[i].GetQueuePoint();
                }
            }
            
            return null;
        }
    }
}
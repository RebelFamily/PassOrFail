using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class ClassroomProfile : MonoBehaviour
    {
        [SerializeField] private SeatProfile[] classroomSeats;

      

        public int CheckIfSeatAreAvailable()
        {
            for (var i = 0; i < classroomSeats.Length; i++)
            {
                if (!classroomSeats[i].IsSeatOccupied())
                {
                    classroomSeats[i].MarkForSitting();
                    return i;
                }
            }
            return -1;
            
        }

        public SeatProfile GetSeatAt(int index) => classroomSeats[index];
    }
}
using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Components
{
    [SelectionBase]
    public class ClassroomProfile : MonoBehaviour
    {
        [SerializeField] private SeatProfile[] classroomSeats;
        [SerializeField] private GameObject teachingTriggerArea;


        private void Update()
        {
            teachingTriggerArea.SetActive(CheckIfSeatAreAvailable() == -1);
        }

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
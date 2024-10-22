using UnityEngine;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class ReceptionPoint : MonoBehaviour
    {
        [SerializeField] private bool isOccupied;
        public bool IsOccupied() => isOccupied;
    }
}
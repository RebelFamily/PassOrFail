using UnityEngine;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class CashItem : MonoBehaviour
    {
        public CashGenerationSystem myCashSystem;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCollisionDetection player))
            {
                player.cashStackingSystem.AddCashToPlayerStack(1);
                myCashSystem.RemoveItemFromList(transform);
            }
        }
    }
}
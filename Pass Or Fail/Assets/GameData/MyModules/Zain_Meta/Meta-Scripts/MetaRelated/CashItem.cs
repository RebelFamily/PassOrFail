using UnityEngine;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class CashItem : MonoBehaviour
    {
        public CashGenerationSystem myCashSystem;
        public int myAmount;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCollisionDetection player))
            {
                player.cashStackingSystem.AddCashToPlayerStack(myAmount);
                myCashSystem.RemoveItemFromList(transform);
            }
        }
    }
}
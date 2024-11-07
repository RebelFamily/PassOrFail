using UnityEngine;
using Zain_Meta.Meta_Scripts.MetaRelated;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public class PlayerCollisionDetection : MonoBehaviour
    {
        public PlayerCashSystem cashStackingSystem;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPurchase purchase))
            {
                purchase.StartPurchasing(cashStackingSystem);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IPurchase purchase))
            {
                purchase.StopPurchasing();
            }
        }
    }
}
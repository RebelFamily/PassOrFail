using UnityEngine;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class IPurchase: MonoBehaviour
    {
        public virtual void StartPurchasing(PlayerCashSystem cashSystem)
        {
            
        }

        public virtual int GetRemainingPrice()
        {
            return -1;
        }

        public virtual void StopPurchasing()
        {
            
        }

        public  void Hide()
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.zero;
        }
        public virtual bool IsPurchased()
        {
            return false;
        }

        public virtual void EnableMe(bool value,bool showNow)
        {
            
        }
    }
}
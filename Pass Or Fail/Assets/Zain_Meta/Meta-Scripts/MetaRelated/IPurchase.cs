using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public interface IPurchase
    {
        public void StartPurchasing(PlayerCashSystem cashSystem);
        public int GetRemainingPrice();
        public void StopPurchasing();
        public bool IsPurchased();
    }
}
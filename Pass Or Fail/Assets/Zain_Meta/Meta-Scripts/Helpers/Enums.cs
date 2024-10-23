using System;

namespace Zain_Meta.Meta_Scripts
{
    public enum ItemsName
    {
        ClassRooms,
        Reception,
        RoomUpgrades,
        Cash
    }

    [Serializable]
    public struct UpgradePrice
    {
        public int pricesForEachUpgrade;
        public int remainingPrice;

        public void ResetPrice()
        {

            remainingPrice = pricesForEachUpgrade;
        }
    }
}
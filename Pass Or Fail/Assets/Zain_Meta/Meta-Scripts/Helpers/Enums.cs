using System;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    public enum ItemsName
    {
        ClassRooms,
        Reception,
        RoomUpgrades,
        Cash
    }

    public enum ClassroomType
    {
        Maths,
        Science,
        Arts,
        Geography
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
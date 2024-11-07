using UnityEngine;

namespace Zain_Meta.Meta_Scripts.DataRelated
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "Data/RoomData", order = 0)]
    public class RoomData : SaveClass
    {
        public bool isUnlocked, overrideUnlock;
        public int price, remainingPrice;
        public int roomLevel,roomUpgradeIndex;

        public override void ClearData()
        {
            isUnlocked = false;
            remainingPrice = price;
            roomLevel = 1;
            roomUpgradeIndex = 0;
        }
    }
}
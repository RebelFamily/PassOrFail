using UnityEngine;

namespace Zain_Meta.Meta_Scripts.DataRelated
{
    [CreateAssetMenu(fileName = "Unlock", menuName = "Data/UnlockData", order = 0)]
    public class UnlockData : SaveClass
    {
        public string saveKey;
        public bool isUnlocked, overrideUnlock;
        public int price, remainingPrice;
        public override void ClearData()
        {
            isUnlocked = false;
            remainingPrice = price;
        }
    }
}
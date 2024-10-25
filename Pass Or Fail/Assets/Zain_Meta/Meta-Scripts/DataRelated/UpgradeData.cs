using UnityEngine;
using Zain_Meta.Meta_Scripts.Helpers;

namespace Zain_Meta.Meta_Scripts.DataRelated
{
    [CreateAssetMenu(fileName = "RoomUpgrade", menuName = "Data/UpgradeData", order = 0)]
    public class UpgradeData : SaveClass
    {
        public bool isUpgraded;
        public UpgradePrice[] pricing;
        public int upgradedLevel, upgradeIndex;

        public override void ClearData()
        {
            isUpgraded = false;
            upgradedLevel = 1;
            upgradeIndex = 0;
            for (var i = 0; i < pricing.Length; i++)
            {
                pricing[i].ResetPrice();
            }
           
        }
    }
}
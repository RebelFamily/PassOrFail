using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    public enum ItemsName
    {
        ClassRooms,
        Reception,
        RoomUpgrades,
        Cash,
        Teacher
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
       public Sprite renderToShowA,renderToShowB,renderToShowC;

        public void ResetPrice()
        {

            remainingPrice = pricesForEachUpgrade;
        }
    }

    [Serializable]
    public struct ClassroomUpgrade
    {
        public Mesh[] meshLevels;
    }
}
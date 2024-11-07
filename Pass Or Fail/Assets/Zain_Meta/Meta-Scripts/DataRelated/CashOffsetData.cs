using UnityEngine;

namespace Zain_Meta.Meta_Scripts.DataRelated
{
    [CreateAssetMenu(fileName = "CashOffset", menuName = "Data/CashOffsetData", order = 0)]
    public class CashOffsetData : ScriptableObject
    {
        public float yOffset, xOffset, zOffset;
        public float maxXVal, maxZVal;
    }
}
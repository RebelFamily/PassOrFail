using UnityEngine;

namespace Zain_Meta.Meta_Scripts.DataRelated
{
    [CreateAssetMenu(fileName = "PlayerSpeedData", menuName = "Data/Player", order = 0)]
    public class PlayerSpeedData : ScriptableObject
    {
        public float moveSpeedNormal,turnSpeedNormal;
        public float moveSpeedChopper,turnSpeedChopper;
        public float moveSpeedBoard,turnSpeedBoard;
        public float moveSpeedCycle,turnSpeedCycle;
    }
}
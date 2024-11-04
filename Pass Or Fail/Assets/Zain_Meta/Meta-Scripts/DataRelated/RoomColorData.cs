using System;
using UnityEngine;

namespace Zain_Meta.Meta_Scripts.DataRelated
{
    [CreateAssetMenu(fileName = "ColorsData", menuName = "Data/RoomsColors", order = 0)]
    public class RoomColorData : ScriptableObject
    {
       public RoomColor[] roomColorsDatum;
    }

    [Serializable]
    public struct ColorsData
    {
        public Color wallColor, borderColor, floorColor;
        public Texture floorTexture;
    }

    [Serializable]
    public struct RoomColor
    {
        public ColorsData[] roomColors;
    }
}
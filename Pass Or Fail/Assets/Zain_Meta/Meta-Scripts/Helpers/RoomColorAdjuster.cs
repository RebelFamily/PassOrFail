using UnityEngine;
using Zain_Meta.Meta_Scripts.DataRelated;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    public class RoomColorAdjuster : MonoBehaviour
    {
        [SerializeField] private MeshRenderer wallMesh, groundMesh;

        public void AdjustColors(ColorsData colorsData)
        {
            wallMesh.materials[0].color = colorsData.wallColor;
            wallMesh.materials[2].color = colorsData.borderColor;
            groundMesh.material.mainTexture = colorsData.floorTexture;
            groundMesh.material.color = colorsData.floorColor;
        }
    }
}
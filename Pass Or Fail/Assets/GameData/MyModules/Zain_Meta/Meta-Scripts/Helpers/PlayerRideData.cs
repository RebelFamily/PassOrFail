using UnityEngine;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    [System.Serializable]
    public class PlayerRideData 
    {
        public ArcadeMovement.PlayerState stateToGive;
        public Sprite renderToShow;
        public GameObject objectToShow;
    }
}
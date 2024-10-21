
using UnityEngine;

namespace PassOrFail.MiniGames
{
    public interface IDragAble
    {
        public Vector3 StartingPosition
        {
            get;
        }
        public Quaternion StartingRotation
        {
            get;
        }
    }
}
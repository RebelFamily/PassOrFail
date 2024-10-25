using UnityEngine;

namespace PassOrFail.MiniGames
{
    public class ObjectData : MonoBehaviour, IDragAble
    {
        private Vector3 _startingPosition;
        private Quaternion _startingRotation;

        public Variables.ObjectType objectType;

        private void Start()
        {
            var transform1 = transform;
            _startingPosition = transform1.localPosition;
            _startingRotation = transform1.localRotation;
        }

        public Vector3 StartingPosition => _startingPosition;
        
        public Quaternion StartingRotation => _startingRotation;
    }
}
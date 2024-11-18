using UnityEngine;

namespace Zain_Meta.Meta_Scripts.PlayerRelated.Rides
{
    public class BoardRide : MonoBehaviour, IRideable
    {
        [SerializeField] private Transform wheel;
        [SerializeField] private float tireRotSpeed;

        private float _yRot, _frontTireRot, _frontRotY;

        public void Move(float xVal, Vector3 yVal, float moveSpeed)
        {
            _frontRotY += yVal.magnitude * moveSpeed * 200f * Time.deltaTime;
            var rotWheel = Quaternion.Euler(new Vector3(_frontRotY, _frontTireRot, 0));
            wheel.localRotation =
                Quaternion.Lerp(wheel.localRotation, rotWheel, tireRotSpeed * Time.deltaTime);
        }
    }
}
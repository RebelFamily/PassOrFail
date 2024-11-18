using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    public class PointGenerator
    {
        private static Vector3 _returningVector;

        public static Vector3 RandomPointInBounds(Bounds bounds)
        {
            _returningVector.x = Random.Range(bounds.min.x, bounds.max.x);
            _returningVector.y = 0;
            _returningVector.z = Random.Range(bounds.min.z, bounds.max.z);
            return _returningVector;
        }

        public static Vector3 RandomPointInBounds(Bounds bounds, float y)
        {
            _returningVector.x = Random.Range(bounds.min.x, bounds.max.x);
            _returningVector.y = y;
            _returningVector.z = Random.Range(bounds.min.z, bounds.max.z);
            return _returningVector;
        }
    }
}
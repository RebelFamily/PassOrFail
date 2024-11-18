using UnityEngine;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public interface IRideable
    {
        public void Move(float xVal,Vector3 yVal,float moveSpeed);
    }
}
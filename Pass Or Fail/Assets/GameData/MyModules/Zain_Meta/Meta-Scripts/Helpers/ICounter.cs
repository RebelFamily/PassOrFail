using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    public interface ICounter
    {
        public void OnTriggerEnter(Collider other);
        public void OnTriggerExit(Collider other);
    }
}
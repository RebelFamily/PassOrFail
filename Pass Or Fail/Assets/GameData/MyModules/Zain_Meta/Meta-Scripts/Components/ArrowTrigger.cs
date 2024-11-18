using UnityEngine;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class ArrowTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject visual;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ArcadeMovement _))
            {
                visual.SetActive(false); 
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ArcadeMovement _))
            {
                visual.SetActive(true);
            }
        }
    }
}
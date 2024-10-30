using UnityEngine;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class StaffroomCounterProfile : MonoBehaviour, ICounter
    {
        [SerializeField] private StackingHandler handler;
        [SerializeField] private Collider myCol;
        [SerializeField] private GameObject visual;

        private void Start()
        {
            Hide();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerStackingSystem stackingSystem))
            {
                if (!handler)
                {
                    Debug.LogWarning("No Handler Associated");
                    return;
                }

                handler.isPlayerTriggering = true;

                stackingSystem.StartDropping(handler);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerStackingSystem _))
            {
                handler.isPlayerTriggering = false;
            }
        }

        public void Show()
        {
            visual.SetActive(true);
            myCol.enabled = true;
        }

        public void Hide()
        {
            visual.SetActive(false);
            myCol.enabled = false;
        }
    }
}
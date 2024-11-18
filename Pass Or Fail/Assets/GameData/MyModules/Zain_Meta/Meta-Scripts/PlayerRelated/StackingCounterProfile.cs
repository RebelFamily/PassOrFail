using UnityEngine;
using Zain_Meta.Meta_Scripts.Helpers;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public class StackingCounterProfile : MonoBehaviour,ICounter
    {
        [SerializeField] private StackingHandler handler;
        [SerializeField] private bool toDrop;


        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerStackingSystem stackingSystem))
            {
                if (!handler)
                {
                    Debug.LogError("No Handler Associated");
                    return;
                }
                
                handler.isPlayerTriggering = true;
                if (toDrop)
                {
                    stackingSystem.StartDropping(handler);
                }
                else
                {
                    stackingSystem.StartStacking(handler);
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerStackingSystem _))
            {
                handler.isPlayerTriggering = false;
            }
        }
    }
}
using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class CoffeeProductionUnit : MonoBehaviour
    {
        [SerializeField] private StackingHandler handlerToUse;
        [SerializeField] private Utility utility;
        [SerializeField] private Transform spawningPos;

        private void Start()
        {
            handlerToUse.isReadyToAccept = true;
            handlerToUse.AddToStack(utility.SpawnCoffeeAt(spawningPos));
        }

        public void ProduceMoreCoffee()
        {
            DOVirtual.DelayedCall(1.5f, () => 
                { handlerToUse.AddToStack(utility.SpawnCoffeeAt(spawningPos)); });
        }
    }
}
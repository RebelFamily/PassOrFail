using Lean.Pool;
using UnityEngine;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class TrashCan : MonoBehaviour
    {
        [SerializeField] private StackingHandler stackingHandler;

        private void Start()
        {
            stackingHandler.isReadyToAccept = true;
        }

        public void DisposeItem()
        {
            LeanPool.Despawn(stackingHandler.GetLastStackedItem());
        }
    }
}
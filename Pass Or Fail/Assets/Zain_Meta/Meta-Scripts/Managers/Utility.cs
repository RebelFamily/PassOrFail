using Lean.Pool;
using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Managers
{
    [CreateAssetMenu(fileName = "Utility", menuName = "Singletons/Utility", order = 0)]
    public class Utility : ScriptableObject
    {
        [SerializeField] private GameObject cashPrefab;
        
        public Transform SpawnCashAt(Transform pos)
        {
            var cash = LeanPool.Spawn(cashPrefab, pos.position, pos.rotation);
            return cash.transform;
        }
    }
}
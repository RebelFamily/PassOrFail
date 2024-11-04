using Lean.Pool;
using UnityEngine;
using Zain_Meta.Meta_Scripts.MetaRelated;

namespace Zain_Meta.Meta_Scripts.Managers
{
    [CreateAssetMenu(fileName = "Utility", menuName = "Singletons/Utility", order = 0)]
    public class Utility : ScriptableObject
    {
        [SerializeField] private CashItem cashPrefab;
        [SerializeField] private GameObject givingCashPrefab;
        [SerializeField] private GameObject coffeePrefab;

        public CashItem SpawnCashAt(Transform pos)
        {
            var cash = LeanPool.Spawn(cashPrefab, pos.position, pos.rotation);
            return cash;
        }

        public Transform SpawnGivingCashAt(Transform pos)
        {
            var cash = LeanPool.Spawn(givingCashPrefab, pos.position, pos.rotation);
            return cash.transform;
        }

        public Transform SpawnCoffeeAt(Transform pos)
        {
            var coffee = LeanPool.Spawn(coffeePrefab, pos.position, pos.rotation);
            return coffee.transform;
        }
    }
}
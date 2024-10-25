using Lean.Pool;
using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;

namespace Zain_Meta.Meta_Scripts.Managers
{
    [CreateAssetMenu(fileName = "Utility", menuName = "Singletons/Utility", order = 0)]
    public class Utility : ScriptableObject
    {
        [SerializeField] private GameObject cashPrefab;
        [SerializeField] private GameObject coffeePrefab;
        [SerializeField] private StudentStateManager studentPrefab;

        public Transform SpawnCashAt(Transform pos)
        {
            var cash = LeanPool.Spawn(cashPrefab, pos.position, pos.rotation);
            return cash.transform;
        }
        public Transform SpawnCoffeeAt(Transform pos)
        {
            var coffee = LeanPool.Spawn(coffeePrefab, pos.position, pos.rotation);
            return coffee.transform;
        }

        public StudentStateManager SpawnStudentAt(Transform pos)
        {
            var student = Instantiate(studentPrefab, pos.position, pos.rotation);
            return student;
        }  
        public StudentStateManager SpawnStudentAt(Vector3 pos)
        {
            var student = Instantiate(studentPrefab, pos, Quaternion.identity);
            return student;
        }
    }
}
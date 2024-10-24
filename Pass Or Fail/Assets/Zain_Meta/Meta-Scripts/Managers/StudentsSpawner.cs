using System;
using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class StudentsSpawner : MonoBehaviour
    {
        [SerializeField] private StudentStateManager studentPrefab;
        [SerializeField] private Transform spawningPos;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var student = Instantiate(studentPrefab, spawningPos.position, spawningPos.rotation);
            }
        }
    }
}
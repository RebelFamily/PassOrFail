using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PassOrFail.MiniGames
{
    public class MaskeOverlayer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> maskedObjects;
        [SerializeField] private GameObject maskedObjToSpawn;
        [SerializeField] private int targetToAchieve;
        [SerializeField] private int maxTarget;
        [SerializeField] private UnityEvent objectSpawnEvent;
        [SerializeField] private UnityEvent targetAchievedEvent;
        [SerializeField] private UnityEvent objectiveCompletedEvent;
        [SerializeField] private int currentTargetCount;
        [SerializeField] private float maskDistance = 0.1f;
        [SerializeField] private Transform spawnParent;
        private bool _isMinEventInvoked, _isMaxEventInvoked;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out ObjectData od))
                if (od.objectType == Variables.ObjectType.Maskable)
                {
                    Vector3 pos = other.transform.position;
                    if (IsObjectPresentInRadius(pos)) return;
                    var mo = Instantiate(maskedObjToSpawn, pos, Quaternion.identity, spawnParent);
                    maskedObjects.Add(mo);
                    objectSpawnEvent?.Invoke();
                    currentTargetCount += 1;
                    if (currentTargetCount >= targetToAchieve && !_isMinEventInvoked)
                    {
                        _isMinEventInvoked = true;
                        targetAchievedEvent?.Invoke();
                    }

                    if (currentTargetCount < maxTarget || _isMaxEventInvoked) return;
                    _isMaxEventInvoked = true;
                    objectiveCompletedEvent?.Invoke();
                }
        }

        private bool IsObjectPresentInRadius(Vector3 pos)
        {
            foreach (var t in maskedObjects)
            {
                if (Vector3.Distance(t.transform.position, pos) < maskDistance)
                    return true;
            }

            return false;
        }
    }
}
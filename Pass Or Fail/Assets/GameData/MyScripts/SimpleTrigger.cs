using UnityEngine;
using UnityEngine.Events;
public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] private string tagToDetect = "Pencil";
    [SerializeField] private UnityEvent onTriggerEnter, onTriggerExit;
    private void OnTriggerEnter(Collider other)
    {
        if(!tagToDetect.Equals(other.tag)) return;
        onTriggerEnter?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if(!tagToDetect.Equals(other.tag)) return;
        onTriggerExit?.Invoke();
    }
}
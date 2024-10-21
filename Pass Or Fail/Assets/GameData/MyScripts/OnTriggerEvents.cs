using UnityEngine;
public class OnTriggerEvents : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        other.GetComponent<SchoolArea>().EnableCanvas(true);
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("ObjectName: " + other.gameObject.name);
        other.GetComponent<SchoolArea>().EnableCanvas(false);
    }
}
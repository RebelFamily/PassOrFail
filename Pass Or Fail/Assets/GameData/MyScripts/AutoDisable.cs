using System.Collections;
using UnityEngine;
public class AutoDisable : MonoBehaviour
{
    [SerializeField] private float delay = 2f;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
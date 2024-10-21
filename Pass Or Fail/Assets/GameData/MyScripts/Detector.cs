using CnControls;
using UnityEngine;
public class Detector : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector3 min = new Vector3(-2.5f, 0, 3f), max = new Vector3(2.5f, 0, 13f);
    private void Update()
    {
        var horizontal = CnInputManager.GetAxis(PlayerPrefsHandler.Horizontal);
        var vertical = CnInputManager.GetAxis(PlayerPrefsHandler.Vertical);
        var newPosition = new Vector3(horizontal * Time.deltaTime * speed, 0f, vertical * Time.deltaTime * speed);
        var position = transform.position;
        position += newPosition;
        position = new Vector3(Mathf.Clamp(position.x, min.x, max.x), position.y, Mathf.Clamp(position.z, min.z, max.z));
        transform.position = position;
    }
}
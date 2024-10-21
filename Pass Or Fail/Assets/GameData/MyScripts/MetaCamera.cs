using CnControls;
using UnityEngine;
public class MetaCamera : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform[] canvases;
    private void LateUpdate()
    {
        var horizontal = CnInputManager.GetAxis(PlayerPrefsHandler.Horizontal);
        var newRotation = new Vector3(0f, horizontal * Time.deltaTime * speed, 0f);
        var transform1 = transform;
        var rot = transform1.localEulerAngles;
        rot += newRotation;
        transform1.localEulerAngles = rot;
        CanvasesLookAtCamera();
    }
    private void CanvasesLookAtCamera()
    {
        foreach (var t in canvases)
        {
            t.LookAt(transform.GetChild(0));
        }
    }
}
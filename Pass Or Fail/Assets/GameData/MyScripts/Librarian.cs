using System;
using CnControls;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Librarian : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector3 min = new Vector3(-2.5f, 0, 3f), max = new Vector3(2.5f, 0, 13f);
    [SerializeField] private LayerMask layerMask;
    private readonly Vector3 center = new Vector3(0.5f, 0.5f, 0);
    private Image crossHair, progressFiller;
    private bool flag = false, cameraFlag = false;
    private int studentCount = 0;
    [SerializeField] private GameObject perfects;
    private Animator cameraAnimator;
    private const string CameraZoomInAnimName = "CameraZoomIn", CameraZoomOutAnimName = "CameraFocusOut";
    private readonly UnityEvent onActivityEnd = new UnityEvent();
    private void Start()
    {
        cameraAnimator = cameraPivot.Find("MainCamera").GetComponent<Animator>();
        crossHair = SharedUI.Instance.gamePlayUIManager.controls.GetCrossHair().GetComponent<Image>();
        progressFiller = crossHair.transform.Find("Filler").GetComponent<Image>();
    }
    private void FixedUpdate() 
    {
        if(!flag) return;
        SetView();
        Raycast();
    }
    private void Raycast()
    {
        if (Camera.main == null) return;
        var ray = Camera.main.ViewportPointToRay(center);
        if(Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject.tag.Equals(PlayerPrefsHandler.Student))
            {
                //Debug.Log("Hit " + hit.collider.name);
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
                var student = hit.collider.gameObject.GetComponent<LibraryStudent>();
                crossHair.color = Color.red;
                progressFiller.fillAmount += Time.deltaTime;
                if (progressFiller.fillAmount >= 1f)
                {
                    flag = false;
                    student.GetLost();
                    perfects.SetActive(true);
                    if(student.isAdded) return;
                    student.isAdded = true;
                    studentCount++;
                    if(cameraFlag) return;
                    cameraFlag = true;
                    crossHair.gameObject.SetActive(false);
                    cameraAnimator.Play(CameraZoomInAnimName);
                    if (studentCount < 3) return;
                    flag = false;
                    onActivityEnd?.Invoke();
                }
            }
        }
        else
        {
            //Debug.Log("Miss");
            crossHair.color = Color.white;
            progressFiller.fillAmount = 0f;
        }
    }
    private void SetView()
    {
        var horizontal = CnInputManager.GetAxis(PlayerPrefsHandler.Horizontal);
        var vertical = CnInputManager.GetAxis(PlayerPrefsHandler.Vertical);
        var newRotation = new Vector3(-vertical * Time.deltaTime * speed, horizontal * Time.deltaTime * speed, 0f);
        var rot = cameraPivot.localEulerAngles;
        rot += newRotation;
        rot = new Vector3(Mathf.Clamp(rot.x, min.x, max.x), Mathf.Clamp(rot.y, min.y, max.y), 0f);
        cameraPivot.localEulerAngles = rot;
    }
    public void SetLibrarianFlag(bool newFlag)
    {
        flag = newFlag;
    }
    public void UnpauseCamera()
    {
        cameraFlag = false;
        flag = true;
        crossHair.gameObject.SetActive(true);
    }
    public void RegisterEndEvent(UnityAction action)
    {
        onActivityEnd.AddListener(action);
    }
}
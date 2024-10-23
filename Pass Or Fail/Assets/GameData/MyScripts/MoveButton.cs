using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class MoveButton : MonoBehaviour
{
    public float speed = 5f;
    public float minY = -200f;
    public float maxY = 200f;
    [SerializeField] private float multiplierValue0 = 1.2f, multiplierValue1 = -1.2f;
    public float threshold = 10f;
    private RectTransform _rectTransform;
    private float _lastY;
    [SerializeField] private ExerciseActivity exerciseActivity;
    [SerializeField] private RectTransform target;
    private bool _isReached = false;
    [SerializeField] private string parameterName = "SpineMultiplier";
    [SerializeField] private bool setLayerWeight = false;
    [SerializeField] private int layerNo = 0;
    [SerializeField] private UnityEvent onReached = new();
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _lastY = _rectTransform.anchoredPosition.y;
        /*if(setLayerWeight)
            excerciseActivity.SetAnimatorLayerWeight(layerNo);*/
    }
    private void Update()
    {
        if(_isReached) return;
        if (!Input.GetMouseButton(0))
        {
            exerciseActivity.SetSpeedMultiplier(0f, parameterName);
            return;
        }
        Vector2 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform, mousePosition, null, out Vector2 anchoredPosition);
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, minY, maxY);
        var deltaY = anchoredPosition.y - _lastY;
        if (!(Mathf.Abs(deltaY) > threshold)) 
        {
            exerciseActivity.SetSpeedMultiplier(0f, parameterName);
            return;
        }
        var targetY = anchoredPosition.y;
        var newY = Mathf.MoveTowards(_rectTransform.anchoredPosition.y, targetY, speed * Time.deltaTime);
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, newY);
        _lastY = newY;
        if (deltaY > 0)
        {
            //Debug.Log(1);
            exerciseActivity.SetSpeedMultiplier(multiplierValue1, parameterName);
        }
        else
        {
            //Debug.Log(-1);
            exerciseActivity.SetSpeedMultiplier(multiplierValue0, parameterName);
        }
        if(setLayerWeight)
            exerciseActivity.SetAnimatorLayerWeight(layerNo, Time.deltaTime * 2f);
        var distance = Vector2.Distance(_rectTransform.anchoredPosition, target.anchoredPosition);
        //Debug.Log("Distance: " + distance);
        if (distance < 10f)
        {
            _isReached = true;
            _rectTransform.anchoredPosition = target.anchoredPosition;
            exerciseActivity.SetSpeedMultiplier(0f, parameterName);
            onReached?.Invoke();
            SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Perfects);
        }
    }
    
}
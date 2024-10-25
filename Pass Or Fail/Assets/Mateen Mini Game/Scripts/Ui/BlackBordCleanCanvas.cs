using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BlackBordCleanCanvas : MonoBehaviour
{
    [SerializeField] private Button tapItBtn;
    private bool _isAlreadyGameStart;
    [SerializeField] private UnityEvent onScreenClick;
    
    private void OnEnable()
    {
        tapItBtn.onClick.AddListener(ClickOnScreen);
    }

    private void OnDisable()
    {
        tapItBtn.onClick.RemoveListener(ClickOnScreen);
    }
    private void ClickOnScreen()
    {
        if(_isAlreadyGameStart) return;
        _isAlreadyGameStart = true;
        onScreenClick?.Invoke();
        tapItBtn.gameObject.SetActive(false);
    }

}

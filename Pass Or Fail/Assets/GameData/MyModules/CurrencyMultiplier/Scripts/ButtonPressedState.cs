using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mateen.CrazyJail
{
    [RequireComponent(typeof(Button))]
    public class ButtonPressedState : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        //[SerializeField] private UnityEvent buttonPressedEvent, buttonUpEvent;
        [SerializeField] private bool isToChangePosition;
        [SerializeField] private RectTransform rectTransform; 
        [SerializeField] private Vector2 targetPosition; 
        private Vector2 _normalPosition; 

        private void Start()
        {
            if(isToChangePosition) _normalPosition = rectTransform.anchoredPosition; // Store the initial position
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            //buttonPressedEvent?.Invoke();
            if (isToChangePosition) rectTransform.anchoredPosition = targetPosition;

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //buttonUpEvent?.Invoke();
            if (isToChangePosition) rectTransform.anchoredPosition = _normalPosition;
        }
    }
}

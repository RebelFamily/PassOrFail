using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UnityAnimationEventTrigger : MonoBehaviour
{
    //[SerializeField] private UnityEvent[] animationEvents;
    [SerializeField] private List<UnityEvent> animationEvents = new List<UnityEvent>();

    public void InvokeAnimationEvent(int eventIndex)
    {
        //Debug.Log("InvokeAnimationEvent: " + eventIndex);
        animationEvents[eventIndex]?.Invoke();
    }
    public void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void ShowRectBanner(string bannerName)
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
            return;
        //AdmobManager.Instance.ShowBanner(bannerName);
    }
    public UnityEvent GetAnimationEvent(int index)
    {
        if(animationEvents.Count <= index)
            animationEvents.Add(new UnityEvent());
        return animationEvents[index];
    }
    public void RegisterAnimationEvent(int index)
    {
        var newEvent = new UnityEvent();
        animationEvents.Add(newEvent);
        //Debug.Log("RegisterAnimationEvent: " + index);
        var addedEvent = animationEvents[animationEvents.Count - 1];
        addedEvent.AddListener(Callback);
    }
    private void Callback()
    {
        Debug.Log("Callback");
        transform.parent.GetComponent<Student>().MoveStudent(true);
    }
}

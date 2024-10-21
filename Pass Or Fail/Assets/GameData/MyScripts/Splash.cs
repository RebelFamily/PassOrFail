using UnityEngine;
public class Splash : MonoBehaviour
{
    private void Start()
    {
        //PlayerPrefsHandler.currency = 10000;
        SharedUI.Instance.SetNextSceneIndex(1);
        Invoke(nameof(InvokeSwitchScene), 6f);
    }
    private void InvokeSwitchScene()
    {
        SharedUI.Instance.SwitchScene();
    }
}
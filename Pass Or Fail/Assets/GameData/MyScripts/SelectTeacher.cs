using UnityEngine;
public class SelectTeacher : Splash
{
    private bool isSelected = false;
    [SerializeField] private Animator[] animators;
    private const string AnimationName = "Greet";
    private void Start()
    {
        if (PlayerPrefsHandler.currentTeacher == -1)
        {
            PlayerPrefsHandler.SetBool(PlayerPrefsHandler.SecondPlay, true);
            SharedUI.Instance.SetNextSceneIndex(1);
        }
        else
        {
            gameObject.SetActive(false);
            SharedUI.Instance.SetNextSceneIndex(2);
            SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Splash);
            SharedUI.Instance.OpenSpecialMenu(PlayerPrefsHandler.CurrencyCounter);
        }
        /*if (!PlayerPrefsHandler.GetBool(PlayerPrefsHandler.SecondPlay))
        {
            PlayerPrefsHandler.SetBool(PlayerPrefsHandler.SecondPlay, true);
            SharedUI.Instance.SetNextSceneIndex(1);
        }
        else
        {
            gameObject.SetActive(false);
            SharedUI.Instance.SetNextSceneIndex(2);
            SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Splash);
            SharedUI.Instance.OpenSpecialMenu(PlayerPrefsHandler.CurrencyCounter);
        }*/
    }
    private void Disable()
    {
        SharedUI.Instance.OpenSpecialMenu(PlayerPrefsHandler.CurrencyCounter);
        gameObject.SetActive(false);
    }
    public void HireTeacher(int no)
    {
        if(isSelected) return;
        isSelected = true;
        PlayerPrefsHandler.currentTeacher = no;
        PlayerPrefsHandler.UnlockTeacher(no);
        animators[no].Play(AnimationName);
        animators[no].GetComponent<Expressions>().ShowExpression(Expressions.ExpressionType.Happy);
        SoundController.Instance.PlayGameCompleteSound();
        Invoke(nameof(InvokeSwitch), 1f);
    }
    private void InvokeSwitch()
    {
        SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Loading);
        Invoke(nameof(Disable), 0.5f);
    }
}
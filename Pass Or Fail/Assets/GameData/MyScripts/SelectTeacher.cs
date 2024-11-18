using UnityEngine;
public class SelectTeacher : Splash
{
    private bool _isSelected = false;
    [SerializeField] private Animator[] animators;
    private const string AnimationName = "Greet";
    private void Start()
    {
        //PlayerPrefsHandler.currency = 10000;
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
        }
    }
    private void Disable()
    {
        gameObject.SetActive(false);
    }
    public void HireTeacher(int no)
    {
        if(_isSelected) return;
        _isSelected = true;
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
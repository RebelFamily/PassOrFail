using UnityEngine;
using UnityEngine.UI;
public class Controls : MonoBehaviour
{
    [SerializeField] private GameObject gradingButtons, streakCounter, mistakeUI, crossHair, tapToPlay;
    [SerializeField] private GameObject activityUI, touchPad, reportCard, progressBar, answerImage, timeBar;
    [SerializeField] private GameObject blinkAlert, perfects, warnings, shouts, protectTheEgg;
    [SerializeField] private RectTransform tutorialHand;
    [SerializeField] private Sprite schoolDanceInstructions, libraryInstructions, gymClassInstructions;
    private const string Filler = "Filler", ActivityContainer = "Container", StreakText = "StreakText", AdButton = "AdButton", PassAlert = "PassAlert",
        FailAlert = "FailAlert", ProtectionText = "ProtectionText", ProtectThe = "Protect the ", InfinityHandPath = "Container/InfinityIconBg";
    
    public void EnableQuestionAnswerUI(bool flag)
    {
        gradingButtons.SetActive(flag);
        progressBar.SetActive(flag);
        streakCounter.SetActive(flag);
    }
    public void EnableAnswerImage(bool flag)
    {
        answerImage.SetActive(flag);
    }
    public void EnableAnswerImage(bool flag, string ans)
    {
        answerImage.transform.Find(PlayerPrefsHandler.Text).GetComponent<Text>().text = ans;
        answerImage.SetActive(flag);
    }
    public void SetStreakCounterText()
    {
        var txt = streakCounter.transform.Find(StreakText).GetComponent<Text>();
        if (PlayerPrefsHandler.streak > 0)
        {
            txt.color = Color.green;
            txt.text = PlayerPrefsHandler.streak.ToString();
        }
        else
        {
            txt.color = Color.red;
            txt.text = "-";
        }
        //txt.color = PlayerPrefsHandler.streak > 0 ? Color.green : Color.red;
        
    }
    public void EnableStreakAdBtn(bool flag)
    {
        streakCounter.transform.Find(AdButton).gameObject.SetActive(flag);
    }
    public GameObject GetAnswerImage()
    {
        return answerImage;
    }
    public void EnableGradingButtons(bool flag)
    {
        gradingButtons.SetActive(flag);
    }
    public void EnableStreakCounter(bool flag)
    {
        streakCounter.SetActive(flag);
    }
    public void EnableMistakeUI(bool flag)
    {
        mistakeUI.SetActive(flag);
        gradingButtons.SetActive(!flag);
        streakCounter.SetActive(!flag);
    }
    public void EnableCrossHair(bool flag)
    {
        crossHair.SetActive(flag);
    }
    public void EnableTapToPlay(bool flag)
    {
        tapToPlay.SetActive(flag);
    }
    public void EnableActivityUI(bool flag)
    {
        activityUI.SetActive(flag);
    }
    public void EnableInfinityHandUI(bool flag)
    {
        activityUI.transform.Find(InfinityHandPath).gameObject.SetActive(flag);
    }
    public void EnableTouchPad(bool flag)
    {
        touchPad.SetActive(flag);
    }
    public void EnableReportCard(bool flag)
    {
        reportCard.SetActive(flag);
    }
    public void SetProgress()
    {
        progressBar.transform.Find(Filler).GetComponent<Image>().fillAmount += 0.333f;
    }
    public Image GetTimerFiller()
    {
        return timeBar.transform.Find(Filler).GetComponent<Image>();
    }
    public void SetActivityInstructionsSprite(string activityName)
    {
        if (activityName == PlayerPrefsHandler.ActivitiesNames[0])
        {
            activityUI.transform.Find(ActivityContainer).GetComponent<Image>().sprite = libraryInstructions;
            timeBar.gameObject.SetActive(false);
        }
        else if(activityName == PlayerPrefsHandler.ActivitiesNames[2])
        {
            activityUI.transform.Find(ActivityContainer).GetComponent<Image>().sprite = schoolDanceInstructions;
            timeBar.gameObject.SetActive(true);
        }
        else
        {
            activityUI.transform.Find(ActivityContainer).GetComponent<Image>().sprite = gymClassInstructions;
            timeBar.gameObject.SetActive(false);
        }
    }
    public void ShowBlinkAlert(string status)
    {
        if (status == PlayerPrefsHandler.Good)
        {
            blinkAlert.SetActive(true);
            blinkAlert.GetComponent<Animator>().Play(PassAlert);
        }
        else
        {
            blinkAlert.SetActive(true);
            blinkAlert.GetComponent<Animator>().Play(FailAlert);
        }
    }
    public GameObject GetCrossHair()
    {
        return crossHair;
    }
    public GameObject GetReportCard()
    {
        return reportCard;
    }
    public void SetProtectionText(string newValue)
    {
        protectTheEgg.transform.Find(ProtectionText).GetComponent<Text>().text = ProtectThe + newValue;
    }

    #region Level Based Methods

    public void Pass()
    {
        DisableHandTutorial();
        GamePlayManager.Instance.currentLevel.Pass();
    }
    public void Fail()
    {
        DisableHandTutorial();
        GamePlayManager.Instance.currentLevel.Fail();
    }
    public void EnableLibraryActivityControls()
    {
        crossHair.SetActive(true);
        activityUI.SetActive(false);
        progressBar.SetActive(true);
        touchPad.SetActive(true);
    }
    public void EnableProgressBar(bool flag)
    {
        progressBar.SetActive(flag);
    }
    public void TapToPlay()
    {
        SoundController.Instance.PlayBtnClickSound();
        tapToPlay.SetActive(false);
        GamePlayManager.Instance.currentLevel.StartActivity();
    }
    public void ShowPerfects(string type)
    {
        switch (type)
        {
            case PlayerPrefsHandler.Perfects:
                perfects.SetActive(true);
                break;
            case PlayerPrefsHandler.Warnings:
                warnings.SetActive(true);
                break;
            default:
                shouts.SetActive(true);
                break;
        }
    }
    public void EnableProtectTheEggUI()
    {
        protectTheEgg.SetActive(true);
    }
    public void SetHandTutorial(bool isRight)
    {
        var newPosition = GetGradingBtnPosition(isRight);
        tutorialHand.anchoredPosition = newPosition;
        tutorialHand.gameObject.SetActive(true);
    }
    public void DisableHandTutorial()
    {
        tutorialHand.gameObject.SetActive(false);   
    }
    private Vector2 GetGradingBtnPosition(bool isRight)
    {
        return isRight ? gradingButtons.transform.Find(PlayerPrefsHandler.Pass).GetComponent<RectTransform>().anchoredPosition :
            gradingButtons.transform.Find(PlayerPrefsHandler.Fail).GetComponent<RectTransform>().anchoredPosition;
    }
    
    #endregion
}
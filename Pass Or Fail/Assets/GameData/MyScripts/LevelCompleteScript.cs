using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class LevelCompleteScript : MonoBehaviour
{
    [BoxGroup("Simple GameObjects")]
    [SerializeField] private GameObject container0, container1;
    [BoxGroup("Simple GameObjects")]
    [SerializeField] private GameObject metaBtnLock;
    [BoxGroup("UI References")]
    [SerializeField] private Image gradingStatusImage;
    [BoxGroup("UI References")]
    [SerializeField] private Text levelPaymentText;
    [BoxGroup("UI References")]
    [SerializeField] private Image[] studentsImages;
    [BoxGroup("Sprites")]
    [SerializeField] private Sprite tick, cross, aPlusGrade, bGrade, cGrade, fGrade;
    private const string Status = "Status";
    private void Start()
    {
        IsMetaUnlocked();
        var gradingValue = GamePlayManager.Instance.currentLevel.GetGradingValue();
        gradingStatusImage.sprite = GetGradingStatus(gradingValue);
        levelPaymentText.text = GamePlayManager.Instance.LevelEndRewardValue.ToString();
        var rect = gradingStatusImage.GetComponent<RectTransform>();
        if(GamePlayManager.Instance.IsGamePlayInActivityState())
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 0f);
            GamePlayManager.Instance.LevelEndRewardValue = 300;
            levelPaymentText.text = GamePlayManager.Instance.LevelEndRewardValue.ToString();
            foreach (var t in studentsImages)
            {
                t.gameObject.SetActive(false);
            }
        }
        else
        {
            SetStudentResults(GamePlayManager.Instance.currentLevel.GetResults(), GamePlayManager.Instance.currentLevel.GetRenders());
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -90f);
            foreach (var t in studentsImages)
            {
                t.gameObject.SetActive(true);
            }
        }
    }
    public void NoThanks()
    {
        GamePlayManager.ShowLevelCompleteAd();
        SoundController.Instance.PlayBtnClickSound();
        CurrencyCounter.Instance.SetCurrency(GamePlayManager.Instance.LevelEndRewardValue);
        NextStep();
    }
    public void NextStep()
    {
        AdsCaller.Instance.ShowRectBanner();
        CurrencyCounter.Instance.ShowCashImage(false);
        container0.SetActive(false);
        container1.SetActive(true);
    }
    private Sprite GetGradingStatus(int gradingValue)
    {
        return gradingValue switch
        {
            3 => aPlusGrade,
            2 => bGrade,
            1 => cGrade,
            _ => fGrade
        };
    }
    private void SetStudentResults(IReadOnlyList<bool> results, IReadOnlyList<Sprite> renders)
    {
        for (var i = 0; i < results.Count; i++)
        {
            studentsImages[i].sprite = renders[i];
            studentsImages[i].transform.Find(Status).GetComponent<Image>().sprite = results[i] ? tick : cross;
        }
    }
    private void IsMetaUnlocked()
    {
        metaBtnLock.SetActive(PlayerPrefsHandler.LevelCounter < PlayerPrefsHandler.LevelNoToShowMeta);
    }
}
using UnityEngine;
using UnityEngine.UI;
public class ReportCardUI : MonoBehaviour
{
    [SerializeField] private Color greenColor, redColor;
    [SerializeField] private Image[] cards;
    [SerializeField] private GameObject[] buttons;
    private const string DescriptionText = "DescriptionText", Spray = "Spray";
    private void OnEnable()
    {
        Callbacks.OnRewardSpray += RewardSpray;
    }
    private void OnDisable()
    {
        Callbacks.OnRewardSpray -= RewardSpray;
    }
    public void SetReportCardUI(CorridorActivity.ReportCard reportCard)
    {
        for (var i = 0; i < cards.Length; i++)
        {
            cards[i].color = reportCard.reportCardEntries[i].isGood ? greenColor : redColor;
            cards[i].transform.Find(DescriptionText).GetComponent<Text>().text = reportCard.reportCardEntries[i].description;
        }
    }
    public void WatchVideoToGetSpray()
    {
        Callbacks.rewardType = Callbacks.RewardType.RewardSpray;
        AdsCaller.Instance.ShowRewardedAd();
    }
    public void TeacherGoesBackToNormal(string action)
    {
        gameObject.SetActive(false);
        GamePlayManager.Instance.currentLevel.TeacherGoesBackToNormal(action);
    }
    private void RewardSpray()
    {
        gameObject.SetActive(false);
        GamePlayManager.Instance.currentLevel.TeacherGoesBackToNormal(Spray);
    }
    public void DisableButton(int btnIndex)
    {
        buttons[btnIndex].SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.UI;
public class ActivityPanel : MonoBehaviour
{
    private void Start()
    {
        var activityData = new ActivitiesData.Activity();//GameManager.Instance.GetActivityData();
        if (activityData == null)
        {
            Debug.LogError("No Such Activity Found");
            return;
        }
        transform.Find("Container/ActivityNameText").GetComponent<Text>().text = activityData.activityName;
        transform.Find("Container/ActivityIcon").GetComponent<Image>().sprite = activityData.activityIcon;
        transform.Find("Container/RewardText").GetComponent<Text>().text = activityData.activityReward.ToString();
    }
    private void OnEnable()
    {
        Callbacks.OnRewardActivity += RewardActivity;
    }
    private void OnDisable()
    {
        Callbacks.OnRewardActivity -= RewardActivity;
    }
    private static void RewardActivity()
    {
        //GameManager.Instance.ActivityFlag = true;
        SharedUI.Instance.SetNextSceneIndex(PlayerPrefsHandler.GamePlay);
        SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Loading);
    }
}
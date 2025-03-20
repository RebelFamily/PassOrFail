using Sirenix.OdinInspector;
using UnityEngine;
public class EnvironmentManager : MonoBehaviour
{
    public enum Environment
    {
        ClassRoomWithTable,
        ClassRoomWithoutTable,
        Corridor,
        SportsArea,
        None
    }
    [TabGroup("MainEnvironments")]
    [SerializeField] private GameObject classRoom, corridor, lecturerTable, sportsArea;
    [TabGroup("Decorations")]
    [SerializeField] private GameObject[] classRoomDecorations;
    [SerializeField] private GameObject nativeAdInClassRoom, danceAd;
    [SerializeField] private GameObject[] badgesDistributionAds, exerciseAds, uniformAds;
    public void SetEnvironment(Environment environmentType)
    {
        //Debug.Log("environmentType: " + environmentType);
        switch (environmentType)
        {
            case Environment.ClassRoomWithTable:
                lecturerTable.SetActive(true);
                classRoom.SetActive(true);
                corridor.SetActive(false);
                sportsArea.SetActive(false);
                ShowClassRoomDecorations();
                break;
            case Environment.ClassRoomWithoutTable:
                lecturerTable.SetActive(false);
                classRoom.SetActive(true);
                corridor.SetActive(false);
                sportsArea.SetActive(false);
                ShowClassRoomDecorations();
                break;
            case Environment.Corridor:
                classRoom.SetActive(false);
                corridor.SetActive(true);
                sportsArea.SetActive(false);
                break;
            case Environment.SportsArea:
                classRoom.SetActive(false);
                corridor.SetActive(false);
                sportsArea.SetActive(true);
                break;
            default:
                lecturerTable.SetActive(false);
                classRoom.SetActive(false);
                corridor.SetActive(false);
                sportsArea.SetActive(false);
                break;
        }
    }
    private void ShowClassRoomDecorations()
    {
        var totalDecorations = classRoomDecorations.Length;
        for (var i = 0; i < totalDecorations; i++)
        {
            classRoomDecorations[i].SetActive(false);
        }
        classRoomDecorations[PlayerPrefsHandler.ClassDecorationsIndex].SetActive(true);
        PlayerPrefsHandler.ClassDecorationsIndex++;
        if (PlayerPrefsHandler.ClassDecorationsIndex >= totalDecorations)
            PlayerPrefsHandler.ClassDecorationsIndex = 0;
    }
    public void DisableNativeAdOfClass()
    {
        nativeAdInClassRoom.SetActive(false);
    }
    public void EnableDanceNativeAd()
    {
        danceAd.SetActive(true);
    }
    public void EnableBadgesNativeAd()
    {
        foreach (var t in badgesDistributionAds)
        {
            t.SetActive(true);
        }
    }
    public void EnableExerciseNativeAd()
    {
        foreach (var t in exerciseAds)
        {
            t.SetActive(true);
        }
    }
    public void EnableUniformNativeAd()
    {
        foreach (var t in uniformAds)
        {
            t.SetActive(true);
        }
    }
}
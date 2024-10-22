using System;
using UnityEngine;
public class EnvironmentManager : MonoBehaviour
{
    public enum Environment
    {
        ClassRoomWithTable,
        ClassRoomWithoutTable,
        Corridor,
        SportsArea
    }
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject classRoom, corridor, lecturerTable, sportsArea;
    [SerializeField] private GameObject[] classRoomDecorations;
    public void SetEnvironment(Environment environmentType)
    {
        Debug.Log("environmentType: " + environmentType);
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
                throw new ArgumentOutOfRangeException(nameof(environmentType), environmentType, null);
        }
    }
    public Inventory GetInventory()
    {
        return inventory;
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
}
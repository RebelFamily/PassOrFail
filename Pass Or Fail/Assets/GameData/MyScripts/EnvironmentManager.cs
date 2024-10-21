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
    public void SetEnvironment(Environment environmentType)
    {
        switch (environmentType)
        {
            case Environment.ClassRoomWithTable:
                lecturerTable.SetActive(true);
                classRoom.SetActive(true);
                corridor.SetActive(false);
                sportsArea.SetActive(false);
                break;
            case Environment.ClassRoomWithoutTable:
                lecturerTable.SetActive(false);
                classRoom.SetActive(true);
                corridor.SetActive(false);
                sportsArea.SetActive(false);
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
}
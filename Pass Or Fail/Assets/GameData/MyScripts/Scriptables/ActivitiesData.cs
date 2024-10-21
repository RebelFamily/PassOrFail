using System;
using UnityEngine;
[CreateAssetMenu(fileName = "ActivitiesData", menuName = "ScriptableObjects/ActivitiesData", order = 3)]
public class ActivitiesData : ScriptableObject
{
    public Activity[] activitiesData;
    [Serializable]
    public class Activity
    {
        public string activityName;
        public int activityReward;
        public Sprite activityIcon;
    }
}
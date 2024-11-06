using System;
using GameAnalyticsSDK;
using UnityEngine;
using Zain_Meta.Meta_Scripts.PlayerRelated;

public class Callbacks : MonoBehaviour {
    public delegate void RewardCoins250();
    public static event RewardCoins250 OnRewardCoins250;
    public delegate void RewardTeacher();
    public static event RewardTeacher OnRewardTeacher;
    public delegate void RewardStudentProp();
    public static event RewardStudentProp OnStudentProp;
    public delegate void RewardClassRoomProp();
    public static event RewardClassRoomProp OnRewardClassRoomProp;
    public delegate void RewardItem();
    public static event RewardItem OnRewardItem;
    public delegate void RewardActivity();
    public static event RewardActivity OnRewardActivity;
    public delegate void RewardCashMultiplier();
    public static event RewardCashMultiplier OnRewardCashMultiplier;
    public delegate void RewardStreak();
    public static event RewardStreak OnRewardStreak;
    public delegate void RewardSpray();
    public static event RewardSpray OnRewardSpray;
    public delegate void RewardSchoolBuilding();
    public static event RewardSchoolBuilding OnRewardSchoolBuilding;
    public delegate void RewardMistakeCorrection();
    public delegate void RewardClassroomUpgrade();
    public delegate void RewardGroundCash();
    public delegate void RewardRide(RewardType rideType);
    public static event RewardMistakeCorrection OnRewardMistakeCorrection;
    public static event RewardClassroomUpgrade OnRewardClassroomUpgrade;
    public static event RewardRide OnRewardARide;
    public static event RewardGroundCash OnRewardGroundCashInMeta;
    private const string SdkName = "MaxAdmob";
    public static RewardType rewardType;
    private void Start () 
    {
		DontDestroyOnLoad (gameObject);
	}
    public static void RewardedAdWatched ()
	{
        switch (rewardType)
        {
            case RewardType.RewardItem:
                OnRewardItem?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardTeacher:
                OnRewardTeacher?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardStudentProp:
                OnStudentProp?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardClassRoomProp:
                OnRewardClassRoomProp?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.Coins250:
                CurrencyCounter.Instance.SetCurrency(250);
                OnRewardCoins250?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardActivity:
                OnRewardActivity?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardCashMultiplier:
                OnRewardCashMultiplier?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardStreak:
                OnRewardStreak?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardSpray:
                OnRewardSpray?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardSchoolBuilding:
                OnRewardSchoolBuilding?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.MistakeCorrection:
                OnRewardMistakeCorrection?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardClassroomUpgradeInMeta:
                OnRewardClassroomUpgrade?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardUniCycle:
                OnRewardARide?.Invoke(rewardType);
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.RewardSkateboard:
                OnRewardARide?.Invoke(rewardType);
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            case RewardType.GroundCashInMeta:
                OnRewardGroundCashInMeta?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, SdkName, rewardType.ToString());
                FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + rewardType);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public enum RewardType
    {
        RewardItem,
        RewardTeacher,
        RewardStudentProp,
        RewardClassRoomProp,
        Coins250,
        RewardActivity,
        RewardCashMultiplier,
        RewardStreak,
        RewardSpray,
        RewardSchoolBuilding,
        MistakeCorrection,
        RewardClassroomUpgradeInMeta,
        RewardUniCycle,
        RewardSkateboard,
        GroundCashInMeta
    }
}
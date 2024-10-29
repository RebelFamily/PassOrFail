using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    private void Awake()
    {
        if (Instance != null)
            return;
        DontDestroyOnLoad(gameObject);
        Instance = this;
        OnFireBase();
    }
    #region Firebase

    private DependencyStatus _dependencyStatus = DependencyStatus.UnavailableOther;
    private bool _firebaseInitialized = false;
    private void OnFireBase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            _dependencyStatus = task.Result;
            if (_dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + _dependencyStatus);
            }
        });
    }
    private void InitializeFirebase()
    {
        //Debug.Log("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        //Debug.Log("Set user properties.");
        // Set the user's sign up method.
        FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");
        // Set the user ID.
        //  FirebaseAnalytics.SetUserId("uber_user_510");
        // Set default session duration values.
        //  FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        _firebaseInitialized = true;
        var app = FirebaseApp.DefaultInstance;
        var defaults = new Dictionary<string, object>
        {
            // yet, or if we ask for values that the server doesn't have:
            // server
            // These are the values that are used if we haven't fetched data from the
            {PlayerPrefsHandler.FirstAdIntervalString, 30},
            {PlayerPrefsHandler.FirstAdTypeString, PlayerPrefsHandler.AdType.Simple.ToString()},
            {PlayerPrefsHandler.InterAdIntervalString, 30},
            {PlayerPrefsHandler.InterAdTypeString, PlayerPrefsHandler.AdType.Simple.ToString()},
            {PlayerPrefsHandler.LevelNoForRatingString, 3},
            {PlayerPrefsHandler.ShowAdOnMiniGameString, false}
        };
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task =>
            {
                FetchDataAsync();
            });
    }
    public void ReportEvent(string eventName)
    {
        //Debug.Log(_firebaseInitialized + " " + eventName);
        if (_firebaseInitialized)
        {
            FirebaseAnalytics.LogEvent(eventName);
        }
    }
    public void ReportEvent(string eventName, string parameterName, string parameterValue)
    {
        if (_firebaseInitialized)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
        }
    }
    private static Task FetchDataAsync()
    {
        //Debug.Log("Fetching data...");
        var fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }
    private static void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            //Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            //Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            //Debug.Log("Fetch completed successfully!");
        }
        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:

                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task =>
                    {
                        //Debug.Log($"Remote data loaded and ready (last fetch time {info.FetchTime}).");
                        GetRemoteData();
                    });
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        //Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        //Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                //Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
    private static void GetRemoteData()
    {
        PlayerPrefsHandler.FirstAdInterval = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(
            PlayerPrefsHandler.FirstAdIntervalString).LongValue;
        PlayerPrefsHandler.FirstAdType = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(
            PlayerPrefsHandler.FirstAdTypeString).StringValue;
        PlayerPrefsHandler.InterAdInterval = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(
            PlayerPrefsHandler.InterAdIntervalString).LongValue;
        PlayerPrefsHandler.InterAdType = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(
            PlayerPrefsHandler.InterAdTypeString).StringValue;
        if (!PlayerPrefsHandler.GetBool(PlayerPrefsHandler.FirstAd))
        {
            if(PlayerPrefsHandler.IsTimerFirstAd())
                AdsCaller.Instance.StartFirstAdTimer(PlayerPrefsHandler.FirstAdInterval);
        }
        else
        {
            if(PlayerPrefsHandler.IsTimerFirstAd() && PlayerPrefsHandler.IsTimerInterAd())
                AdsCaller.Instance.StartInterAdTimer();
        }
        PlayerPrefsHandler.LevelNoForRating = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(
            PlayerPrefsHandler.LevelNoForRatingString).LongValue;
        PlayerPrefsHandler.ShowAdOnMiniGame = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(
            PlayerPrefsHandler.ShowAdOnMiniGameString).BooleanValue;
    }
    #endregion
}
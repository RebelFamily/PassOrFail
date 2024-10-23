using System;
using System.Linq;
using UnityEngine;
public class PlayerPrefsHandler : MonoBehaviour
{
    #region Scenes
    public const string Splash = "MySplash";
    public const string GamePlay = "GamePlay";
    public const string Meta = "Meta";
    #endregion
    
    #region Menus
    public const string HUD = "HUD";
    public const string MainMenu = "MainMenu";
    public const string Loading = "Loading";
    public const string LevelComplete = "LevelComplete";
    public const string Settings = "Settings";
    public const string NoVideo = "NoVideo";
    public const string Unlock = "Unlock";
    public const string Reward = "Reward";
    public const string CurrencyCounter = "CurrencyCounter";
    public const string CharactersCustomization = "CharactersCustomization";
    public const string RemoveAds = "RemoveAds";
    #endregion
    
    #region Buttons
    public const string Play = "Play";
    public const string Teachers = "Teachers";
    public const string Students = "Students";
    public const string NextComplete = "NextComplete";
    public const string Home = "Home";
    public const string Replay = "Replay";
    public const string SettingsClose = "SettingsClose";
    public const string Coins250 = "Coins250";
    public const string RewardStreak = "RewardStreak";
    public const string HideSubMenu = "HideSubMenu";
    public const string AdToCorrectMistake = "AdToCorrectMistake";
    public const string PayToCorrectMistake = "PayToCorrectMistake";
    public const string CloseRemoveAds = "CloseRemoveAds";
    #endregion
    
    #region Settings
    public const string Sound = "Sound";
    public const string Music = "Music";
    public const string Vibration = "Vibration";
    #endregion
    
    #region Tags
    public const string Detector = "Detector";
    public const string Student = "Student";
    #endregion
    
    #region Inputs
    public const string Horizontal = "Horizontal", Vertical = "Vertical";
    #endregion
    
    #region Tutorial
    public const string TutorialString = "Tutorial";
    public const string TutorialStep0String = "TutorialStep0";
    public const string TutorialStep1String = "TutorialStep1";
    public const string TutorialStep2String = "TutorialStep2";
    #endregion

    public const int TotalLevels = 28, TotalActivities = 6;
    private const string CurrentLevelString = "currentLevel";
    private const string LevelCounterString = "levelCounter";
    private const string CurrentActivityString = "currentActivity";
    private const string CurrentMiniGameString = "currentMiniGame";
    private const string CurrentTeacherString = "currentTeacher";
    private const string CurrentClassPropString = "currentClassProp";
    private const string StudentPropString = "studentProp";
    private const string Currency = "Currency";
    private const string Streak = "Streak";
    public const string Perfects = "Perfects", Warnings = "Warnings", Shouts = "Shouts", SecondPlay = "SecondPlay", Pass = "Pass", Fail = "Fail", Good = "Good", Bad = "Bad", Text = "Text";
    
    private const string SchoolMainBuildingRank = "SchoolMainBuildingRank";
    private const string SchoolCafeteriaRank = "SchoolCafeteriaRank";
    private const string SchoolPlayGroundRank = "SchoolPlayGroundRank";
    private const string SchoolArenaRank = "SchoolArenaRank";
    
    private const string MetaString = "MetaString";
    private const string School = "School";
    private const string SchoolNo = "SchoolNo";
    private const string SchoolBuildingFiller = "SchoolBuildingFiller";
    private const string SchoolBuildingCost = "SchoolBuildingPrice";

    public static readonly string[] ActivitiesNames = {"Library Drill", "Recess Round", "School Dance", "Oral Quiz", "Uniform Checking", "Badges Distribution", "Exercise Activity"};
    private static readonly int[] LevelNumbersForActivities = {3, 6, 9, 12, 15, 18, 21, 24};
    private static readonly int[] LevelNumbersForMiniGames = {2, 5, 7, 10, 13, 16, 19, 22, 25};
    
    public static int ClassDecorationsIndex = 0;

    public static bool IsFreeSpinAvailable = true, IsGameLaunch = true;
    public const string FirstAd = "FirstAd";
    public const string RateUsString = "RateUs", RemoveAdsFirstShownString = "RemoveAdsFirstShown";

    #region Firebase
    public enum AdType
    {
        Simple,
        Timer
    }
    public static string FirstAdType = AdType.Simple.ToString(), InterAdType = AdType.Simple.ToString();
    public const string FirstAdIntervalString = "FirstAdInterval", FirstAdTypeString = "FirstAdType", LevelNoToShowMetaString = "LevelNoToShowMeta",
        ShowMiniGameString = "ShowMiniGame", LevelNoForRatingString = "LevelNoForRating", InterAdIntervalString = "InterAdInterval", InterAdTypeString = "InterAdType";
    public static float FirstAdInterval = 30, InterAdInterval = 30;
    public static int LevelNoToShowMeta = 1, LevelNoForRating = 3;
    public static bool ShowMiniGame = false;
    public static bool IsTimerFirstAd()
    {
        return FirstAdType == AdType.Timer.ToString();
    }
    public static bool IsTimerInterAd()
    {
        return InterAdType == AdType.Timer.ToString();
    }

    #endregion
    
    public static int streak
    {
        get => PlayerPrefs.GetInt(Streak, 0);
        set => PlayerPrefs.SetInt(Streak, value);
    }
    public static int currency
    {
        get => PlayerPrefs.GetInt(Currency, 100);
        set => PlayerPrefs.SetInt(Currency, value);
    }
    public static int CurrentLevelNo
    {
        get => PlayerPrefs.GetInt(CurrentLevelString, 0);
        set => PlayerPrefs.SetInt(CurrentLevelString, value);
    }
    public static int LevelCounter
    {
        get => PlayerPrefs.GetInt(LevelCounterString, 1);
        set => PlayerPrefs.SetInt(LevelCounterString, value);
    }
    public static int CurrentActivityNo
    {
        get => PlayerPrefs.GetInt(CurrentActivityString, 0);
        set => PlayerPrefs.SetInt(CurrentActivityString, value);
    }
    public static int CurrentMiniGameNo
    {
        get => PlayerPrefs.GetInt(CurrentMiniGameString, 0);
        set => PlayerPrefs.SetInt(CurrentMiniGameString, value);
    }
    public static void SetSoundControllerBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value == false ? 0 : 1);
    }
    public static bool GetSoundControllerBool(string key)
    {
        var value = PlayerPrefs.GetInt(key, 1);
        return value != 0;
    }
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value == false ? 0 : 1);
    }

    public static bool GetBool(string key)
    {
        var value = PlayerPrefs.GetInt(key, 0);
        return value != 0;
    }
    public static void UnlockTeacher(int teacherIndex)
    {
        PlayerPrefs.SetInt(CurrentTeacherString + "_" + teacherIndex, 1);
    }
    public static bool IsTeacherLocked(int teacherIndex)
    {
        return PlayerPrefs.GetInt(CurrentTeacherString + "_" + teacherIndex, 0) == 0;
    }
    public static int currentTeacher
    {
        get => PlayerPrefs.GetInt(CurrentTeacherString, -1);
        set => PlayerPrefs.SetInt(CurrentTeacherString, value);
    }
    public static int schoolMainBuildingRank
    {
        get => PlayerPrefs.GetInt(SchoolMainBuildingRank, 0);
        set => PlayerPrefs.SetInt(SchoolMainBuildingRank, value);
    }
    public static int schoolCafeteriaRank
    {
        get => PlayerPrefs.GetInt(SchoolCafeteriaRank, 0);
        set => PlayerPrefs.SetInt(SchoolCafeteriaRank, value);
    }
    public static int schoolPlayGroundRank
    {
        get => PlayerPrefs.GetInt(SchoolPlayGroundRank, 0);
        set => PlayerPrefs.SetInt(SchoolPlayGroundRank, value);
    }
    public static int schoolArenaRank
    {
        get => PlayerPrefs.GetInt(SchoolArenaRank, 0);
        set => PlayerPrefs.SetInt(SchoolArenaRank, value);
    }
    public static void UnlockClassProp(int propIndex)
    {
        PlayerPrefs.SetInt(CurrentClassPropString + "_" + propIndex, 1);
    }
    public static bool IsClassPropLocked(int propIndex)
    {
        return PlayerPrefs.GetInt(CurrentClassPropString + "_" + propIndex, 0) == 0;
    }
    public static void UnlockStudentProp(int studentIndex, int hatIndex)
    {
        PlayerPrefs.SetInt(StudentPropString + studentIndex + "_" + hatIndex, 1);
    }
    public static bool IsStudentPropLocked(int studentIndex, int hatIndex)
    {
        return PlayerPrefs.GetInt(StudentPropString + studentIndex + "_" + hatIndex, 0) == 0;
    }
    public static void SetStudentCurrentProp(int studentIndex, int hatIndex)
    {
        PlayerPrefs.SetInt(StudentPropString + "_" + studentIndex, hatIndex);
    }
    public static int GetStudentCurrentProp(int studentIndex)
    {
        return PlayerPrefs.GetInt(StudentPropString + "_" + studentIndex, -1);
    }
    public static void SetBuildingFillerValue(string buildingName, float newValue)
    {
        PlayerPrefs.SetFloat(SchoolBuildingFiller + "_" + buildingName, newValue);
    }
    public static float GetBuildingFillerValue(string buildingName, float defaultValue)
    {
        return PlayerPrefs.GetFloat(SchoolBuildingFiller + "_" + buildingName, defaultValue);
    }
    public static int GetBuildingCostValue(string buildingName, int defaultValue)
    {
        return PlayerPrefs.GetInt(SchoolBuildingCost + "_" + buildingName, defaultValue);
    }
    public static void SetBuildingCostValue(string buildingName, int newValue)
    {
        PlayerPrefs.SetInt(SchoolBuildingCost + "_" + buildingName, newValue);
    }
    public static bool IsBuildingUnlocked(string buildingName)
    {
        return GetBool(buildingName);
    }
    public static void UnlockBuilding(string buildingName)
    {
        SetBool(buildingName, true);
    }
    public static int schoolNo
    {
        get => PlayerPrefs.GetInt(SchoolNo, 0);
        set => PlayerPrefs.SetInt(SchoolNo, value);
    }
    public static bool IsSchoolUnlocked(string schoolName, bool defaultValue)
    {
        var value = PlayerPrefs.GetInt(schoolName, Convert.ToInt32(defaultValue));
        return value != 0;
    }
    public static void UnlockSchool(string schoolName)
    {
        SetBool(schoolName, true);
    }
    public static bool IsSchoolFinished(int no)
    {
        return GetBool(School + no);
    }
    public static void FinishSchool(int no)
    {
        SetBool(School + no, true);
    }
    public static bool IsMetaFinished()
    {
        return GetBool(MetaString);
    }
    public static void FinishMeta()
    {
        SetBool(MetaString, true);
    }
    public static bool IsActivityTime()
    {
        return LevelNumbersForActivities.Any(t => CurrentLevelNo == t);
    }
    public static bool IsMiniGameTime()
    {
        return LevelNumbersForMiniGames.Any(t => CurrentLevelNo == t);
    }
}
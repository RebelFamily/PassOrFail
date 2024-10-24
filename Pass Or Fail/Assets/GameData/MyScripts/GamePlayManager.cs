using System;
using System.Collections;
using GameAnalyticsSDK;
using UnityEngine;
public class GamePlayManager : MonoBehaviour
{
    [HideInInspector]
    public bool gameStartFlag = false, gamePauseFlag = false, gameOverFlag = false, gameCompleteFlag = false, gameContinueFlag = false;
    [ReadOnly] public EnvironmentManager environmentManager;
    [ReadOnly] public LevelBasedParams currentLevel;
    [ReadOnly] public MiniGame currenMiniGame;
    public GameObject mainCamera;
    public bool AskFixedQuestions { get; set; } = true;
    public int LevelEndRewardValue { get; set; } = 100;
    public int LastStreakValue { get; set; } = 0;
    public bool MiniGameFlag { get; set; } = false;
    private GamePlayState _gamePlayState;
    public string CurrentActivity { get; private set; } = PlayerPrefsHandler.ActivitiesNames[0];
    private const string LevelsPath = "Levels/Level", ActivitiesPath = "Activities/", BeforeAdString = "BeforeAd", AfterAdString = "AfterAd",
        LevelString = "Level", UnderScoreString = "_", MiniGamesPath = "MiniGames/MiniGame", MiniGameString = "MiniGame";
    private enum GamePlayState
    {
        Level,
        Activity,
        MiniGame
    }
    public static GamePlayManager Instance;
    private void Awake()
    {
        Instance = this;
        environmentManager = GetComponent<EnvironmentManager>();
        AdsCaller.Instance.ShowBanner();
        AdsCaller.Instance.HideRectBanner();
        CurrencyCounter.Instance.ShowCashImage(true);
    }
    private void Start()
    {
        SoundController.Instance.PlayGamePlayBackgroundMusic();
        ShowRemoveAds();
        CurrentLevelSettings();
    }
    public bool IsGameReadyToPlay()
    {
        return gameStartFlag == true && gamePauseFlag == false && gameOverFlag == false && gameCompleteFlag == false;
    }
    public bool IsLevelCompleted()
    {
        return gameCompleteFlag;
    }
    public void SetGameToPlay(int no)
    {
        gameStartFlag = false;
        gamePauseFlag = false;
        gameOverFlag = false;
        gameCompleteFlag = false;
        switch (no)
        {
            // game start
            case 0:
                gameStartFlag = true;
                break;
            // game pause
            case 1:
                gamePauseFlag = true;
                break;
            // game over
            case 2:
                gameOverFlag = true;
                break;
            // game complete or level complete
            default:
                gameCompleteFlag = true;
                break;
        }
    }
    private void ShowRemoveAds()
    {
        if(PlayerPrefsHandler.GetBool(PlayerPrefsHandler.RemoveAds)) return;
        if (PlayerPrefsHandler.LevelCounter > 2)
        {
            if (PlayerPrefsHandler.GetBool(PlayerPrefsHandler.RemoveAdsFirstShownString))
            {
                if (PlayerPrefsHandler.IsGameLaunch)
                {
                    PlayerPrefsHandler.IsGameLaunch = false;
                    SharedUI.Instance.SubMenu(PlayerPrefsHandler.RemoveAds);
                }
            }
            else
            {
                PlayerPrefsHandler.IsGameLaunch = false;
                PlayerPrefsHandler.SetBool(PlayerPrefsHandler.RemoveAdsFirstShownString, true);
                SharedUI.Instance.SubMenu(PlayerPrefsHandler.RemoveAds);
            }
        }
    }
    private void CurrentLevelSettings()
    {
        var path = LevelsPath + PlayerPrefsHandler.CurrentLevelNo;
        //Debug.Log(GameManager.Instance.ActivityFlag+ " CurrentLevelSettings " + GameManager.Instance.CurrentActivity);
        if (PlayerPrefsHandler.ShowMiniGame)
        {
            if (!GameManager.Instance.MiniGameFlag)
            {
                if (PlayerPrefsHandler.IsMiniGameTime())
                {
                    GameManager.Instance.MiniGameFlag = true;
                    _gamePlayState = GamePlayState.MiniGame;
                    path = MiniGamesPath + PlayerPrefsHandler.CurrentMiniGameNo;
                    var mini = Instantiate((GameObject) Resources.Load(path));
                    currenMiniGame = mini.GetComponent<MiniGame>();
                    environmentManager.SetEnvironment(currenMiniGame.GetEnvironment());
                    currenMiniGame.StartMiniGame();
                    mainCamera.SetActive(false);
                    Invoke(nameof(LevelStart), 0.1f);
                    return;
                }
            }
            GameManager.Instance.MiniGameFlag = false;
        }
        if (!GameManager.Instance.ActivityFlag)
        {
            if (PlayerPrefsHandler.IsActivityTime())
            {
                GameManager.Instance.ActivityFlag = true;
                CurrentActivity = PlayerPrefsHandler.ActivitiesNames[PlayerPrefsHandler.CurrentActivityNo];
                _gamePlayState = GamePlayState.Activity;
                path = ActivitiesPath + CurrentActivity;
            }
        }
        if(_gamePlayState == GamePlayState.Level)
            GameManager.Instance.ActivityFlag = false;
        var level = Instantiate((GameObject)Resources.Load(path));
        currentLevel = level.GetComponent<LevelBasedParams>();
        environmentManager.SetEnvironment(currentLevel.GetEnvironment());
        Invoke(nameof(LevelStart), 0.1f);
    }
    private void LevelStart()
    {
        SetGadsmeAds();
        SetGameToPlay(0);
        SendProgressionEvent(GAProgressionStatus.Start);
        SendAppMetricaProgressionEvent(GAProgressionStatus.Start);
    }
    public void LevelComplete(float delay)
    {
        //Debug.Log("LevelComplete");
        SetGameToPlay(3);
        EndGamePlayTutorial();
        SendProgressionEvent(GAProgressionStatus.Complete, BeforeAdString);
        SendAppMetricaProgressionEvent(GAProgressionStatus.Complete);
        if (_gamePlayState == GamePlayState.MiniGame)
        {
            SetNextMiniGame();
            SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Loading);
            return;
        }
        StartCoroutine(DelayForLevelComplete(delay));
    }
    private IEnumerator DelayForLevelComplete(float delay)
    {
        SetNextLevel();
        yield return new WaitForSeconds(delay);
        GadsmeInit.Instance.DisableAds();
        SoundController.Instance.PlayGameCompleteSound();
        SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.LevelComplete);
    }
    private static void EndGamePlayTutorial()
    {
        if(PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialString)) return;
        PlayerPrefsHandler.SetBool(PlayerPrefsHandler.TutorialStep0String, true);
    }
    public void PlayNextLevel()
    {
        SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Loading);
    }
    private void SetNextLevel()
    {
        if (IsGamePlayInActivityState())
        {
            PlayerPrefsHandler.CurrentActivityNo++;
            if (PlayerPrefsHandler.CurrentActivityNo >= PlayerPrefsHandler.TotalActivities)
                PlayerPrefsHandler.CurrentActivityNo = 0;
        }
        else
        {
            if (PlayerPrefsHandler.LevelCounter < PlayerPrefsHandler.LevelNoToShowMeta)
            {
                //Debug.Log(GameManager.Instance.GetActivityData() + " SetNextLevel " + GameManager.Instance.ActivityFlag);
                SharedUI.Instance.SetNextSceneIndex(PlayerPrefsHandler.GamePlay);
            }
            else
            {
                if((PlayerPrefsHandler.CurrentLevelNo == 0 || (PlayerPrefsHandler.CurrentLevelNo + 1) % 3 == 0) && 
                   PlayerPrefsHandler.CurrentLevelNo != 1) SharedUI.Instance.SetNextSceneIndex(PlayerPrefsHandler.Meta);
                else
                    SharedUI.Instance.SetNextSceneIndex(PlayerPrefsHandler.GamePlay);
            }
            PlayerPrefsHandler.CurrentLevelNo++;
            PlayerPrefsHandler.LevelCounter++;
            if (PlayerPrefsHandler.CurrentLevelNo >= PlayerPrefsHandler.TotalLevels)
                PlayerPrefsHandler.CurrentLevelNo = 0;
        }
    }
    private void SetNextMiniGame()
    {
        PlayerPrefsHandler.CurrentMiniGameNo++;
        if (PlayerPrefsHandler.CurrentMiniGameNo >= PlayerPrefsHandler.TotalMiniGames)
            PlayerPrefsHandler.CurrentMiniGameNo = 0;
    }
    public static void ShowLevelCompleteAd()
    {
        //Debug.Log(PlayerPrefsHandler.FirstAdType + " : " + PlayerPrefsHandler.InterAdType);
        if (PlayerPrefsHandler.IsTimerFirstAd())
        {
            if (!PlayerPrefsHandler.GetBool(PlayerPrefsHandler.FirstAd))
            {
                AdsCaller.Instance.ShowFirstTimerAd();
            }
            else
            {
                if (PlayerPrefsHandler.IsTimerInterAd())
                {
                    AdsCaller.Instance.ShowInterTimerAd();
                }
                else
                {
                    AdsCaller.Instance.ShowInterstitialAd();
                }
            }
        }
        else
        {
            //Debug.Log("PlayerPrefsHandler.LevelCounter: " + PlayerPrefsHandler.LevelCounter);
            if (PlayerPrefsHandler.LevelCounter <= 2)
            {
                if (PlayerPrefsHandler.IsTimerInterAd())
                    AdsCaller.Instance.StartInterAdTimer();
                return;
            }
            if (PlayerPrefsHandler.IsTimerInterAd())
            {
                AdsCaller.Instance.ShowInterTimerAd();
            }
            else
            {
                AdsCaller.Instance.ShowInterstitialAd();
            }
        }
    }
    public bool IsGamePlayInActivityState()
    {
        return _gamePlayState == GamePlayState.Activity;
    }
    private void SetGadsmeAds()
    {
        switch (_gamePlayState)
        {
            case GamePlayState.Level:
                GadsmeInit.Instance.EnableAds(0);
                break;
            case GamePlayState.Activity:
                switch (CurrentActivity)
                {
                    case "Library Drill":
                        GadsmeInit.Instance.EnableAds(1);
                        break;
                    case "Recess Round":
                        GadsmeInit.Instance.EnableAds(2);
                        break;
                    case "School Dance" or "Uniform Checking" or "Badges Distribution":
                        GadsmeInit.Instance.EnableAds(3);
                        break;
                    case "Oral Quiz":
                        GadsmeInit.Instance.EnableAds(4);
                        break;
                }
                break;
            case GamePlayState.MiniGame:
                GadsmeInit.Instance.DisableAds();
                break;
            default:
                break;
        }   
    }
    #region Progression Methods

    private void SendProgressionEvent(GAProgressionStatus status, string extra = "")
    {
        var eventMsg =  LevelString + PlayerPrefsHandler.LevelCounter + extra;
        
        switch (_gamePlayState)
        {
            case GamePlayState.Level:
                if (extra == AfterAdString)
                {
                    eventMsg =  LevelString + (PlayerPrefsHandler.LevelCounter - 1) + extra;
                }
                break;
            case GamePlayState.Activity:
                eventMsg = CurrentActivity + extra;
                break;
            case GamePlayState.MiniGame:
                eventMsg = MiniGameString + (PlayerPrefsHandler.CurrentMiniGameNo + 1) + extra;
                break;
            default:
                break;
        }
        GameAnalytics.NewProgressionEvent(status, eventMsg);
        FirebaseManager.Instance.ReportEvent(status + UnderScoreString + eventMsg);
    }
    private void SendAppMetricaProgressionEvent(GAProgressionStatus status)
    {
        var appMetricaEventMsg = LevelString + UnderScoreString + PlayerPrefsHandler.LevelCounter;
        switch (_gamePlayState)
        {
            case GamePlayState.Level:
                break;
            case GamePlayState.Activity:
                appMetricaEventMsg = CurrentActivity;
                break;
            case GamePlayState.MiniGame:
                appMetricaEventMsg = MiniGameString + UnderScoreString + (PlayerPrefsHandler.CurrentMiniGameNo + 1);
                break;
            default:
                break;
        }
        AppmetricaAnalytics.ReportCustomEvent(AnalyticsType.GameData, appMetricaEventMsg, status.ToString());
    }
    public void SendProgressionEvent()
    {
        SendProgressionEvent(GAProgressionStatus.Complete, AfterAdString);
    }

    #endregion
}
using System;
using UnityEngine;
public class AdsManager : MonoBehaviour
{
    [SerializeField] private string maxSdkKey = "6AQkyPv9b4u7yTtMH9PT40gXg00uJOTsmBOf7hDxa_-FnNZvt_qTLnJAiKeb5-2_T8GsI_dGQKKKrtwZTlCzAR";
    [SerializeField] private string interstitialAdUnitId = "0bf5dd259a7babe3";
    [SerializeField] private  string rewardedAdUnitId = "5d75002bbc4126b9";
    [SerializeField] private string bannerAdUnitId = "YOUR_BANNER_AD_UNIT_ID";
    public string mRecAdUnitId = "ENTER_MREC_AD_UNIT_ID_HERE";
    private bool _isBannerShowing, _isBannerReady, _isBannerInitialized, _isFlooringBannerReady, _isFlooringBannerShowing, _isFlooringBannerInitialized;
    private bool _isMRecShowing,_isRectBannerReady, _isRectBannerInitialized;
    private int _interstitialRetryAttempt;
    private int _rewardedRetryAttempt;
    private const string ADBanner = "ad_banner";
    public static AdsManager Instance;
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this.gameObject);
        InitializeMax();
    }
    private void InitializeMax()
    {
        CheckMemoryState.Instance.CheckMemory();
        if(!CheckMemoryState.Instance.IsEnoughMemory()) return;
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            // AppLovin SDK is initialized, configure and start loading ads.
            //Debug.Log("MAX SDK Initialized");
            RegisterPaidAdEvent();
            InitializeInterstitialAd();
            InitializeRewardedAds();
            InitializeBannerAds();
            InitializeMRecAds();
        };
        MaxSdk.SetSdkKey(maxSdkKey);
        MaxSdk.InitializeSdk();
    }

    #region Simple Interstitial Ad Methods

    private void InitializeInterstitialAd()
    {
        // Attach callbacks
        MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialHiddenEvent += OnSimpleInterstitialDismissedEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    public void LoadInterstitial()
    {
        //interstitialStatusText.text = "Loading...";
        MaxSdk.LoadInterstitial(interstitialAdUnitId);
        // Debug.Log(InterstitialAdUnitId+ "InterstitialAdUnitId");
    }

    public void ShowInterstitial()
    {
        MaxSdk.ShowInterstitial(interstitialAdUnitId);
    }

    public bool IsInterstitialReady()
    {
        return MaxSdk.IsInterstitialReady(interstitialAdUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
        // interstitialStatusText.text = "Loaded";
        //Debug.Log("Interstitial loaded");

        // Reset retry attempt
        _interstitialRetryAttempt = 0;
    }

    private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
    {
        //Debug.Log(adUnitId + "InterstitialAdUnitId");
        // interstitialStatusText.text = "Failed load: " + errorCode + "\nRetrying in 3s...";
        //Debug.Log("Interstitial failed to load with error code: " + errorCode);

        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays.

        _interstitialRetryAttempt++;
        var retryDelay = Math.Pow(2, _interstitialRetryAttempt);

        Invoke(nameof(LoadInterstitial), (float)retryDelay);
        //AdmobManager.Instance.LoadInterstitial_High();
    }

    private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        //Debug.Log("Interstitial failed to display with error code: " + errorCode);
        //AdmobManager.Instance.LoadInterstitial_High();
        LoadInterstitial();
    }

    private void OnSimpleInterstitialDismissedEvent(string adUnitId)
    {
        // Interstitial ad is hidden. Pre-load the next ad
        //Debug.Log("Interstitial dismissed");
        //LoadFlooringInterstitial();
        LoadInterstitial();
    }

    #endregion

    #region Rewarded Ad Methods

    private void InitializeRewardedAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;



        // Load the first RewardedAd
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        
        //  rewardedStatusText.text = "Loading...";
        MaxSdk.LoadRewardedAd(rewardedAdUnitId);
    }

    public void ShowRewardedAd()
    {
        MaxSdk.ShowRewardedAd(rewardedAdUnitId);
    }
    public bool IsRewardedAdAvailable()
    {
        return MaxSdk.IsRewardedAdReady(rewardedAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId)
    {
   
        //Debug.Log("Rewarded ad loaded");
        // Reset retry attempt
        _rewardedRetryAttempt = 0;
    }

    private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
    {
        // rewardedStatusText.text = "Failed load: " + errorCode + "\nRetrying in 3s...";
        //Debug.Log("Rewarded ad failed to load with error code: " + errorCode);

        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays.

        _rewardedRetryAttempt++;
        var retryDelay = Math.Pow(2, _rewardedRetryAttempt);
        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {

        // Rewarded ad failed to display. We recommend loading the next ad
        //Debug.Log("Rewarded ad failed to display with error code: " + errorCode);
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId)
    {
        AppOpenAdCaller.IsInterstitialAdPresent = true;
        Debug.Log("Rewarded ad displayed");
    }

    private void OnRewardedAdDismissedEvent(string adUnitId)
    {

        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded ad dismissed");
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
    {
        Callbacks.RewardedAdWatched();
        // Rewarded ad was displayed and user should receive the reward
        Debug.Log("Rewarded ad received reward");
    }

    #endregion

    #region Banner Ad Methods

    private void InitializeBannerAds()
    {
        //Debug.Log("InitializeSimpleBannerAds Max");
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, new Color(0, 0, 0, 1));
    }
    public void ShowBanner()
    {
        MaxSdk.ShowBanner(bannerAdUnitId);
    }
    public void HideBanner()
    {
        MaxSdk.HideBanner(bannerAdUnitId);
    }
    public void DestroyBanner()
    {
        _isBannerReady = false;
        MaxSdk.DestroyBanner(bannerAdUnitId);
    }
    #endregion

    #region MREC Ad Methods

    private void InitializeMRecAds()
    {
        // MRECs are automatically sized to 300x250.
        MaxSdk.CreateMRec(mRecAdUnitId, MaxSdkBase.AdViewPosition.TopCenter);
        
        if (_isRectBannerInitialized) return;
        _isRectBannerInitialized = true;

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnRectBannerAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnRectBannerAdLoadFailedEvent;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnRectBannerAdCollapsedEvent;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnRectBannerAdExpandedEvent;
    }
    public void ShowMREC()
    {
        if(IsRectBannerAdAvailable())
            MaxSdk.ShowMRec(mRecAdUnitId);
        else
            InitializeMRecAds();
    }

    public void HideMREC()
    {
        MaxSdk.HideMRec(mRecAdUnitId);
    }
    public bool IsRectBannerAdAvailable()
    {
        return _isRectBannerReady;
    }
    private void OnRectBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Max simple banner is loaded");
        _isRectBannerReady = true;
    }
    private void OnRectBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        Debug.Log("Max simple banner is failed");
        _isRectBannerReady = false;
        //if(!GameManager.Instance.isAdmobRectBanner) AdmobManager.Instance.RequestRectBanner();
    }
    private void OnRectBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Max simple banner is collapsed");
        _isRectBannerReady = false;
    }
    private void OnRectBannerAdExpandedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log("Max simple banner is Expanded");
    }
    #endregion

    private void RegisterPaidAdEvent()
    {
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += MaxHandleInterstitialPaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += MaxHandleRewardedPaidEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += MaxHandleBannerPaidEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += MaxHandleMRecPaidEvent;
    }
    private void MaxHandleInterstitialPaidEvent(string sender, MaxSdkBase.AdInfo adInfo)
    {
        AdjustManager.Instance.Max(adInfo);
        AppmetricaAnalytics.ReportRevenue_Applovin(adInfo, AppmetricaAnalytics.AdFormat.Interstitial, interstitialAdUnitId);
    }
    private void MaxHandleRewardedPaidEvent(string sender, MaxSdkBase.AdInfo adInfo)
    {
        AdjustManager.Instance.Max(adInfo);
        AppmetricaAnalytics.ReportRevenue_Applovin(adInfo, AppmetricaAnalytics.AdFormat.Rewarded, rewardedAdUnitId);
    }
    private void MaxHandleBannerPaidEvent(string sender, MaxSdkBase.AdInfo adInfo)
    {
        AdjustManager.Instance.Max(adInfo);
        FirebaseManager.Instance.ReportEvent(ADBanner);
        AppmetricaAnalytics.ReportRevenue_Applovin(adInfo, AppmetricaAnalytics.AdFormat.Banner, bannerAdUnitId);
    }
    private void MaxHandleMRecPaidEvent(string sender, MaxSdkBase.AdInfo adInfo)
    {
        AdjustManager.Instance.Max(adInfo);
        AppmetricaAnalytics.ReportRevenue_Applovin(adInfo, AppmetricaAnalytics.AdFormat.MREC, mRecAdUnitId);
    }
}
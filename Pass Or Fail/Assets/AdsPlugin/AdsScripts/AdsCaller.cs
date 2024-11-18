using GameAnalyticsSDK;
using UnityEngine;
public class AdsCaller : MonoBehaviour
{
    [SerializeField] private GameObject adsUI;
    public static AdsCaller Instance;
    private float _firstAdTime = 0, _interAdTime = 0;
    private bool _firstAdStartTimer = false, _interAdStartTimer = false;
    private bool _firstAdReady = false, _interAdReady = false;
    private const string AD_INTER = "ad_inter", MAX = "Max", ADMOB = "Admob", InterMax = "InterMax", InterAdmob = "InterAdmob", 
        MaxAdmob = "MaxAdmob", InterstitialFailed = "InterstitialFailed", RewardedMax = "RewardedMax", RewardedAdmob = "RewardedAdmob",
        AD_REWARDED = "ad_rewarded", RewardedFailed = "RewardedFailed";
    private void Start()
    {
        if(Instance != null) return;
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (_firstAdStartTimer)
        {
            if (!_firstAdReady)
            {
                _firstAdTime -= Time.deltaTime;
                if (_firstAdTime <= 0)
                {
                    _firstAdReady = true;
                    _firstAdStartTimer = false;
                }
            }
        }
        if (!_interAdStartTimer) return;
        if (_interAdReady) return;
        _interAdTime -= Time.deltaTime;
        if (!(_interAdTime <= 0)) return;
        _interAdReady = true;
        _interAdStartTimer = false;
    }
    public void StartFirstAdTimer(float interval)
    {
        _firstAdTime = interval;
        _firstAdStartTimer = true;
    }
    public void EndFirstAdTimer()
    {
        _firstAdStartTimer = false;
    }
    public void ShowFirstTimerAd()
    {
        if(!_firstAdReady) return;
        _firstAdStartTimer = false;
        _firstAdReady = false;
        PlayerPrefsHandler.SetBool(PlayerPrefsHandler.FirstAd, true);
        ShowInterstitialAd();
    }
    public void StartInterAdTimer()
    {
        _interAdTime = PlayerPrefsHandler.InterAdInterval;
        _interAdStartTimer = true;
    }
    public void EndInterAdTimer()
    {
        _interAdStartTimer = false;
    }
    public void ShowInterTimerAd()
    {
        if(!_interAdReady) return;
        _interAdStartTimer = false;
        _interAdReady = false;
        ShowInterstitialAd();
    }
    public void ShowInterstitialAd()
    {
        if(PlayerPrefsHandler.GetBool(PlayerPrefsHandler.RemoveAds)) return;
        if(GameManager.Instance.IsTesting()) return;
        CheckMemoryState.Instance.CheckMemory();
        if (AdsManager.Instance.IsInterstitialReady())
        {
            AppOpenAdCaller.IsInterstitialAdPresent = true;
            AdsManager.Instance.ShowInterstitial();
            GameAnalytics.NewAdEvent(GAAdAction.Show , GAAdType.Interstitial , MAX , InterMax);
            FirebaseManager.Instance.ReportEvent(AD_INTER);
        }
        else if (AdmobManager.Instance.IsInterstitialReady())
        {
            AppOpenAdCaller.IsInterstitialAdPresent = true;
            AdmobManager.Instance.ShowInterstitial();
            GameAnalytics.NewAdEvent(GAAdAction.Show , GAAdType.Interstitial , ADMOB , InterAdmob);
            FirebaseManager.Instance.ReportEvent(AD_INTER);
        }
        else
        {
            GameAnalytics.NewAdEvent(GAAdAction.FailedShow , GAAdType.Interstitial , MaxAdmob , InterstitialFailed);
            FirebaseManager.Instance.ReportEvent(GAAdAction.FailedShow.ToString() + GAAdType.Interstitial);
        }
        if(PlayerPrefsHandler.IsTimerInterAd())
            StartInterAdTimer();
    }
    public bool IsInterstitialAdAvailable()
    {
        return AdsManager.Instance.IsInterstitialReady() || AdmobManager.Instance.IsInterstitialReady();
    }
    public void ShowBanner()
    {
        AdsManager.Instance.ShowBanner();
    }
    public void HideBanner()
    {
        AdsManager.Instance.HideBanner();
        //AdManager.Instance.applovin.HideBanner();
    }
    public void ShowRectBanner()
    {
        AdsManager.Instance.ShowMREC();
    }
    public void HideRectBanner()
    {
        AdsManager.Instance.HideMREC();
    }
    private bool _isRewardedAdCall = false;
    public void ShowRewardedAd()
    {
        CheckMemoryState.Instance.CheckMemory();
        if(!CheckMemoryState.Instance.IsEnoughMemory()) return;
        if (_isRewardedAdCall) return;
        _isRewardedAdCall = true;
        Invoke(nameof(SetRewardedBool),0.5f);
        if (AdsManager.Instance.IsRewardedAdAvailable())
        {
            AppOpenAdCaller.IsInterstitialAdPresent = true;
            AdsManager.Instance.ShowRewardedAd();
            GameAnalytics.NewAdEvent(GAAdAction.Show , GAAdType.RewardedVideo , MAX, RewardedMax);
            FirebaseManager.Instance.ReportEvent(AD_REWARDED);
        }
        else if (AdmobManager.Instance.IsRewardedAdReady())
        {
            AppOpenAdCaller.IsInterstitialAdPresent = true;
            AdmobManager.Instance.ShowRewardedAd();
            GameAnalytics.NewAdEvent(GAAdAction.Show , GAAdType.RewardedVideo , ADMOB, RewardedAdmob);
            FirebaseManager.Instance.ReportEvent(AD_REWARDED);
        }
        else
        {
            SharedUI.Instance.SubMenu(PlayerPrefsHandler.NoVideo);
            GameAnalytics.NewAdEvent(GAAdAction.FailedShow , GAAdType.RewardedVideo , MaxAdmob , RewardedFailed);
            FirebaseManager.Instance.ReportEvent(GAAdAction.FailedShow.ToString() + GAAdType.RewardedVideo);
        }
    }
    
    private void SetRewardedBool()
    {
        _isRewardedAdCall = false;
    }
    public bool IsRewardedAdAvailable()
    {
        return AdsManager.Instance.IsRewardedAdAvailable() || AdmobManager.Instance.IsRewardedAdReady();
    }
    public void ShowAdUI()
    {
        CheckMemoryState.Instance.CheckMemory();
        if(!CheckMemoryState.Instance.IsEnoughMemory()) return;
        adsUI.SetActive(IsInterstitialAdAvailable());
    }
}
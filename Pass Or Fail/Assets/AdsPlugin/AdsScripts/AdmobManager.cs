﻿using GameAnalyticsSDK;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
public class AdmobManager : MonoBehaviour
{
    public static AdmobManager Instance;

    private RewardedAd rewardedAd;

    private BannerView banner, _rectBanner;
    private AdSize rectangleAdSize;

    private InterstitialAd interstitialAd;
    
    private bool isAdmobInitialized = false, isBannerReady, _isRectBannerReady;

    [SerializeField] public string bannerID, rectBannerID, interstitialID_All, rewardedAdID;
    [SerializeField] private AppOpenAdCaller appOpenAdCaller;
    [SerializeField] private NativeAdsController nativeAdsController;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        GameAnalytics.Initialize();
        MobileAds.Initialize(initStatus =>
        {
            //Debug.Log("Admob Initialize Successfully");
            isAdmobInitialized = true;
            LoadAds();
            // Initialize the Google Mobile Ads Immersive In-game Ads Plugin.
            ImmersiveInGameDisplayAd.Initialize(() =>
            {
                //nativeAdsController.RequestImmersiveInGameDisplayAd();
            });
        });
    }
    private void LoadAds()
    {
        CheckMemoryState.Instance.CheckMemory();
        if(!CheckMemoryState.Instance.IsEnoughMemory()) return;
        //CreateAllBanner();
        CreateInterstitial();
        RequestRewarded();
        //RequestRectBanner();
        appOpenAdCaller.InitAppOpen();
    }

    #region RectBanner
    public void RequestRectBanner()
    {

        if (IsRectBannerReady())
        {
            ShowRectBanner();
            return;
        }


#if UNITY_ANDROID
        var adUnitId = rectBannerID;
#elif UNITY_IPHONE
                        string adUnitId = rectBannerID;
#else
                        string adUnitId = "unexpected_platform";
#endif

        Debug.Log("Admob RequestRectBanner");
        _rectBanner = new BannerView(adUnitId, AdSize.MediumRectangle, AdPosition.BottomLeft);

        _rectBanner.OnBannerAdLoaded += () =>
        {
            Debug.Log("RectBanner ad loaded.");
            _isRectBannerReady = true;
        };
        _rectBanner.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("RectBanner ad failed to load with error: " + error.GetMessage());
            _isRectBannerReady = false;
        };
        _rectBanner.OnAdImpressionRecorded += () =>
        {
            Debug.Log("RectBanner ad recorded an impression.");
        };
        _rectBanner.OnAdClicked += () =>
        {
            Debug.Log("RectBanner ad recorded a click.");
        };
        _rectBanner.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("RectBanner ad opening.");
        };
        _rectBanner.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("RectBanner ad closed.");
        };
        _rectBanner.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("RectBanner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            SendPaidEvent(adValue, AppmetricaAnalytics.AdFormat.MREC, adUnitId);
        };


        // Load a banner ad
        _rectBanner.LoadAd(CreateAdRequest());
    }
    public void RequestRectBanner(int x, int y)
    {

        if (IsRectBannerReady())
        {
            ShowRectBanner();
            return;
        }


#if UNITY_ANDROID
        var adUnitId = rectBannerID;
#elif UNITY_IPHONE
                        string adUnitId = rectBannerID;
#else
                        string adUnitId = "unexpected_platform";
#endif

        Debug.Log("Admob RequestRectBanner");
        Debug.Log("ScreenDPI: " + Screen.dpi);
        //_rectBanner = new BannerView(adUnitId, AdSize.MediumRectangle, AdPosition.BottomLeft);

        _rectBanner = new BannerView(adUnitId, AdSize.MediumRectangle, x, y);

        _rectBanner.OnBannerAdLoaded += () =>
        {
            Debug.Log("RectBanner ad loaded.");
            _isRectBannerReady = true;
        };
        _rectBanner.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("RectBanner ad failed to load with error: " + error.GetMessage());
            _isRectBannerReady = false;
        };
        _rectBanner.OnAdImpressionRecorded += () =>
        {
            Debug.Log("RectBanner ad recorded an impression.");
        };
        _rectBanner.OnAdClicked += () =>
        {
            Debug.Log("RectBanner ad recorded a click.");
        };
        _rectBanner.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("RectBanner ad opening.");
        };
        _rectBanner.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("RectBanner ad closed.");
        };
        _rectBanner.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("RectBanner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            SendPaidEvent(adValue, AppmetricaAnalytics.AdFormat.MREC, adUnitId);
        };


        // Load a banner ad
        _rectBanner.LoadAd(CreateAdRequest());
    }


    public void HideRectBanner()
    {
        Debug.Log("Admob HideRectBanner");
        _rectBanner?.Hide();

    }

    public void DestroyRectBanner()
    {
        Debug.Log("Admob DestroyRectBanner");
        _isRectBannerReady = false;
        _rectBanner?.Destroy();
    }

    public void ShowRectBanner()
    {
        Debug.Log("Admob ShowRectBanner:" + IsRectBannerReady());
        if (IsRectBannerReady())
        {
            _rectBanner?.Show();
        }
    }

    public bool IsRectBannerReady()
    {
        return _isRectBannerReady;
    }

    #endregion

    #region Banner

    private void RequestBanner()
    {
#if UNITY_ANDROID
        var adUnitId = bannerID;
#elif UNITY_IPHONE
                        string adUnitId = bannerID;
#else
                        string adUnitId = "unexpected_platform";
#endif

        Debug.Log("Admob RequestBanner");

        if (banner != null)
        {
            banner.Destroy();
        }

        //banner = new BannerView(adUnitId, AdSize.SmartBanner,  AdPosition.Top);
        var widthInPixels = Screen.safeArea.width > 0 ? Screen.safeArea.width : Screen.width;
        var width = (int)(widthInPixels / MobileAds.Utils.GetDeviceScale());
        rectangleAdSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(width);
        banner = new BannerView(adUnitId, rectangleAdSize, AdPosition.Top);

        banner.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner ad loaded.");
            isBannerReady = true;
        };
        banner.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("Banner ad failed to load with error: " + error.GetMessage());
            isBannerReady = false;
        };
        banner.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner ad recorded an impression.");
        };
        banner.OnAdClicked += () =>
        {
            Debug.Log("Banner ad recorded a click.");
        };
        banner.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner ad opening.");
        };
        banner.OnAdFullScreenContentClosed += () =>
        {
            RequestBanner();
            Debug.Log("Banner ad closed.");
        };
        banner.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            SendPaidEvent(adValue, AppmetricaAnalytics.AdFormat.Banner, adUnitId);
        };


        // Load a banner ad
        banner.LoadAd(CreateAdRequest());
    }
    public void HideBanner()
    {
        Debug.Log("Admob HideBanner");
        banner?.Hide();
    }
    public void DestroyBanner()
    {
        Debug.Log("Admob DestroyBanner");
        isBannerReady = false;
        banner?.Destroy();
    }
    public void ShowBanner()
    {
        Debug.Log("Admob ShowBanner:" + IsBannerReady());
        if (IsBannerReady())
        {
            banner?.Show();
        }
        else
        {
            DestroyBanner();
            isBannerReady = false;
            RequestBanner();
        }
    }
    public bool IsBannerReady()
    {
        return isBannerReady;
        //return banner != null;
    }

    #endregion

    #region Interstitial

    private void CreateInterstitial()
    {
        var adUnitId = interstitialID_All;

        //Debug.Log("Admob RequestInterstitial");
        // Initialize an InterstitialAd.
        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }


        // Load an interstitial ad
        InterstitialAd.Load(adUnitId, CreateAdRequest(),
            (InterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    /*Debug.Log("Interstitial ad failed to load with error: " +
                        loadError.GetMessage());*/
                    return;
                }
                else if (ad == null)
                {
                    //Debug.Log("Interstitial ad failed to load.");
                    return;
                }

                //Debug.Log("Interstitial ad loaded.");
                interstitialAd = ad;

                ad.OnAdFullScreenContentOpened += () =>
                {
                    //Debug.Log("Interstitial ad opening.");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    //Debug.Log("Interstitial ad closed.");
                    CreateInterstitial();
                };
                ad.OnAdImpressionRecorded += () =>
                {
                    //Debug.Log("Interstitial ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    //Debug.Log("Interstitial ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    /*Debug.Log("Interstitial ad failed to show with error: " +
                                error.GetMessage());*/
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    /*string msg = string.Format("{0} (currency: {1}, value: {2}",
                                               "Interstitial ad received a paid event.",
                                               adValue.CurrencyCode,
                                               adValue.Value);*/
                    SendPaidEvent(adValue, AppmetricaAnalytics.AdFormat.Interstitial, adUnitId);
                };
            });
    }
    public void ShowInterstitial()
    {
        //Debug.Log("Admob Show Interstitial Low");
        AppOpenAdCaller.IsInterstitialAdPresent = true;
        interstitialAd.Show();
    }
    

    public bool IsInterstitialReady()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
            return true;
        else
        {
            CreateInterstitial();
            return false;
        }
    }

    #endregion

    #region Rewarded
    private void RequestRewarded()
    {
        var adUnitId = rewardedAdID;

        // Initialize an InterstitialAd.
        
        //Debug.Log("Admob RequestRewarded");
        RewardedAd.Load(adUnitId, CreateAdRequest(),
            (RewardedAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    /*Debug.Log("Rewarded ad failed to load with error: " +
                                loadError.GetMessage());*/
                    return;
                }
                else if (ad == null)
                {
                    //Debug.Log("Rewarded ad failed to load.");
                    return;
                }

                //Debug.Log("Rewarded ad loaded.");
                rewardedAd = ad;

                ad.OnAdFullScreenContentOpened += () =>
                {
                    //Debug.Log("Rewarded ad opening.");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    RequestRewarded();
                    //Debug.Log("Rewarded ad closed.");
                };
                ad.OnAdImpressionRecorded += () =>
                {
                    //Debug.Log("Rewarded ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    //Debug.Log("Rewarded ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    /*Debug.Log("Rewarded ad failed to show with error: " +
                               error.GetMessage());*/
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    /*string msg = string.Format("{0} (currency: {1}, value: {2}",
                                               "Rewarded ad received a paid event.",
                                               adValue.CurrencyCode,
                                               adValue.Value);*/
                    SendPaidEvent(adValue, AppmetricaAnalytics.AdFormat.Rewarded, adUnitId);
                };
            });
    }
    
    public void ShowRewardedAd()
    {
        if (!isAdmobInitialized)
        {
            //Debug.Log("No Video Ad Available");
            return;
        }
        
        //Debug.Log("Admob ShowRewardedAd:"+IsRewardedAdReady());
        
        if (rewardedAd!=null && rewardedAd.CanShowAd())
        {
            AppOpenAdCaller.IsInterstitialAdPresent = true;
            GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, "admob", "Rewarded_Admob");
            FirebaseManager.Instance.ReportEvent(GAAdAction.Show + "_" + GAAdType.RewardedVideo + "_Admob_" + "Rewarded_Admob");
            rewardedAd.Show((Reward reward) =>
            {
                Callbacks.RewardedAdWatched();
            });
        }
    }

    public bool IsRewardedAdReady()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
            return true;
        else
        {
            RequestRewarded();
            return false;
        }
    }

    #endregion

    private AdRequest CreateAdRequest()
    {
        return new AdRequest();
    }
    private static void SendPaidEvent(AdValue adValue, AppmetricaAnalytics.AdFormat adFormat, string adUnit, string placementName = null)
    {
        AdjustManager.Instance.Admob(adValue);
        AppmetricaAnalytics.ReportRevenue_Admob(adValue, adFormat, adUnit, placementName);
    }
}
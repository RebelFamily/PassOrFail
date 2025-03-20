using System;
using GoogleMobileAds.Api;
using UnityEngine;
public class NativeAdsController : MonoBehaviour
{
    [SerializeField] private string adUnitId = "ca-app-pub-3940256099942544/2247696110";
    private ImmersiveInGameDisplayAd _inGameAd;
    private GameObject _parentObject;
    private Material _customAdMaterial;
    public void RequestImmersiveInGameDisplayAd()
    {
        var adSize = new ImmersiveInGameDisplayAdAspectRatio(1, 1);
        var adLoader = new AdLoader.Builder(adUnitId)
            .ForImmersiveInGameDisplayAd()
            .SetImmersiveInGameDisplayAdAspectRatio(adSize)
            .Build();
        adLoader.OnImmersiveInGameDisplayAdLoaded += this.HandleImmersiveInGameDisplayAdLoaded;
        adLoader.OnAdLoadFailed += this.HandleAdLoadFailed;
        adLoader.OnImmersiveInGameDisplayAdClicked += this.HandleAdClick;
        var request = new AdRequest();
        adLoader.LoadAd(request);
    }
    private void HandleImmersiveInGameDisplayAdLoaded(object sender,
        ImmersiveInGameDisplayAdEventArgs args)
    {
        Debug.Log("Immersive in-game display ad loaded.");
        this._inGameAd = args.ImmersiveInGameDisplayAd;
        this._inGameAd.SetParent(_parentObject);
        this._inGameAd.SetLocalPosition(new Vector3(0.0f, 0.0f, 0.5f));
        this._inGameAd.SetLocalRotation(Quaternion.Euler(0.0f, 90.0f, 0.0f));
        this._inGameAd.SetLocalScale(0.75f);
        this._inGameAd.ShowAd();
        this._inGameAd.DontDestroyOnLoad();
        this._inGameAd.SetMaterial(_customAdMaterial);
    }
    private void HandleAdLoadFailed(LoadAdError error)
    {
        var message = error.GetMessage();
        Debug.Log("Immersive in-game display ad failed to load: " + message);
    }
    private void HandleAdClick(object sender, EventArgs args)
    {
        Debug.Log("Immersive in-game display ad click recorded.");
    }
}
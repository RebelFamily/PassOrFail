using UnityEngine;
using Gadsme;
public class GadsmeInit : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject[] adsAccordingToCameraViews;
    public static GadsmeInit Instance;
    private void Start()
    {
        if(Instance) return;
        Instance = this;
        DontDestroyOnLoad(this);
        
        // Placement events
        GadsmeEvents.PlacementLoadedEvent += OnPlacementLoaded;
        GadsmeEvents.PlacementVisibleEvent += OnPlacementVisible;
        GadsmeEvents.PlacementViewableEvent += OnPlacementViewable;
        GadsmeEvents.PlacementClickedEvent += OnPlacementClicked;
        GadsmeEvents.PlacementFailedEvent += OnPlacementFailed;
        GadsmeEvents.ImpressionEvent += SendImpressionData;

        GadsmeSDK.SetMainCamera(mainCamera); // Register Main Camera
        GadsmeSDK.Init(); // Init Gadsme SDK
        GadsmeSDK.SetUserAge(21);
        //GadsmeSDK.SetUserGender(Gender.MALE);
    }

    private void OnDisable()
    {
        GadsmeEvents.PlacementLoadedEvent -= OnPlacementLoaded;
        GadsmeEvents.PlacementVisibleEvent -= OnPlacementVisible;
        GadsmeEvents.PlacementViewableEvent -= OnPlacementViewable;
        GadsmeEvents.PlacementClickedEvent -= OnPlacementClicked;
        GadsmeEvents.PlacementFailedEvent -= OnPlacementFailed;
        GadsmeEvents.ImpressionEvent -= SendImpressionData;
    }
    private void OnGamePhaseChange(Camera newCamera)
    {
        GadsmeSDK.SetMainCamera(newCamera); // Update Main camera
    }
    private void OnPlacementLoaded(GadsmePlacement placement)
    {
        //Debug.Log("Placement Loaded (" + placement.placementId + ")");
        //EnableAd(placement, true);
    }

    private void OnPlacementVisible(GadsmePlacement placement)
    {
        //Debug.Log("Placement Visible (" + placement.placementId + ")");
        //EnableAd(placement, true);
    }

    private void OnPlacementViewable(GadsmePlacement placement)
    {
        //Debug.Log("Placement Viewable (" + placement.placementId + ")");
        //EnableAd(placement, true);
    }

    private void OnPlacementClicked(GadsmePlacement placement)
    {
        //Debug.Log("Placement Clicked (" + placement.placementId + ")");
        //EnableAd(placement, true);
    }

    private void OnPlacementFailed(GadsmePlacement placement)
    {
        //Debug.Log("Placement Failed (" + placement.placementId + ")");
        //EnableAd(placement, false);
    }

    private void SendImpressionData(GadsmeImpressionData impressionData)
    {
        /*Debug.Log("IMPRESSION EVENT:");
        Debug.Log("  placementId: " + impressionData.placementId);
        Debug.Log("  gameId: " + impressionData.gameId);
        Debug.Log("  countryCode: " + impressionData.countryCode);
        Debug.Log("  currency: " + impressionData.currency);
        Debug.Log("  netRevenue: " + impressionData.netRevenue);
        Debug.Log("  lineItemType: " + impressionData.lineItemType);
        Debug.Log("  platform: " + impressionData.platform);*/
    }
    public void EnableAds(int viewIndex)
    {
        DisableAds();
        adsAccordingToCameraViews[viewIndex].SetActive(true);
    }
    public void DisableAds()
    {
        foreach (var t in adsAccordingToCameraViews)
        {
            t.SetActive(false);
        }
    }
}
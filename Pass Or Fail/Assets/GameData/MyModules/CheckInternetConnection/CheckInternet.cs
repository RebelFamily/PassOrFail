using System.Collections;
using UnityEngine;

public enum InternetConnection  {Connected,NotConnected}

public class CheckInternet : MonoBehaviour
{
    public GameObject panelToShow;
    public InternetConnection currentConnection;
    private bool _firstEvent = false;
    private float _waitTime = 5f;
    private bool _retryClicked = false;
    private bool _running = false;
    private void Awake() {

        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        {
            StartCoroutine(CheckInternetConnection());
            Invoke(nameof(FirstEvent),5f);
        }
    }
    public void FirstEvent()
    {
        if(Application.internetReachability==NetworkReachability.NotReachable)
        {
            FirebaseManager.Instance.ReportEvent("Internet_NotConnected_at_Start");
            currentConnection = InternetConnection.NotConnected;
        }
        else
        {
            FirebaseManager.Instance.ReportEvent("Internet_Connected_at_Start");
            currentConnection = InternetConnection.Connected;
        }
    }
    private IEnumerator CheckInternetConnection()
    {
        _running = true;
        yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(_waitTime));
        //  UnityWebRequest request = new UnityWebRequest("https://google.com");
       // yield return request.SendWebRequest();
       // if (request.error != null)
       if(Application.internetReachability==NetworkReachability.NotReachable)
        {
           // Debug.Log("Error");
           // if (AdsIds_GP.ButtonChoice==1)
           {      panelToShow.SetActive(true);}
           // else
           //  {
             //  PanelToShow.SetActive(false);
           //}
            // if (AdsMediatorManagerZR.instance.isRectBannerShowing)
            // {
            //  //   isrecthide = true;
            //     AdsMediatorManagerZR.instance.HideRectBanner();
            //
            // }
            // if (AdsMediatorManagerZR.instance.isSmartBannerShowing)
            // {
            //     //isbannerhide = true;
            //     AdsMediatorManagerZR.instance.hideBanner();
            //
            // }   if (AdsMediatorManagerZR.instance.is2ndSmartBannerShowing)
            // {
            //     //isbannerhide = true;
            //     AdsMediatorManagerZR.instance.hide2ndBanner();
            //
            // }
            _running = false;

            if (currentConnection ==InternetConnection.Connected)
            {
                currentConnection = InternetConnection.NotConnected;
                FirebaseManager.Instance.ReportEvent("Internet_Not_Connected_InGame");
            }
            _retryClicked = false;
            //waitTime = 1f;
            StopCoroutine(CheckInternetConnection());
            
            //  action(false);
        }
        else
        {
            _running = true;
            //Debug.Log("Success");
            panelToShow.SetActive(false);
            
            _waitTime = 10f;
            if (currentConnection ==InternetConnection.NotConnected)
            {
                currentConnection = InternetConnection.Connected;
               // firebasecall.Instance.Event("Internet_Connected");
                if (_retryClicked)
                {
                    FirebaseManager.Instance.ReportEvent("Internet_Connected_Retry");
                    _retryClicked = false;
                }
            }
            //  if (AdsIds_GP.ButtonChoice==1)
            {
                StartCoroutine(CheckInternetConnection());
            }
            //  action(true);
        }
    }
    
    public void Retry() {

        if (!_running)
        {
            _retryClicked = true;
            _waitTime = 0.1f;
          //  if (AdsIds_GP.ButtonChoice==1)
            {
                StartCoroutine(CheckInternetConnection());

            }
            //  else
            //  {
            //    PanelToShow.SetActive(false);
            // }

        }
    }
    public static class CoroutineUtil
    {
        public static IEnumerator WaitForRealSeconds(float time)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }
    }
}

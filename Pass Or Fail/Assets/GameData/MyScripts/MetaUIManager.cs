using System.Linq;
using DG.Tweening;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.UI;

public class MetaUIManager : MonoBehaviour
{
    #region Properties
    
    [SerializeField] private AllMenus[] allMenus;
    [SerializeField] private AllMenus[] subMenus;
    [SerializeField] private GameObject lowerButtons;
    [SerializeField] private GameObject handTutorial0;
    [Header("Meta")]
    [SerializeField] private School currentSchool;
    [SerializeField] private GameObject handTutorial1;
    [SerializeField] private GameObject fillerButtons, buildingSelectionButtons;
    [SerializeField] private School[] schools;
    [SerializeField] private Animator metaCamera;
    [SerializeField] private Transform[] cameraPositions;
    [SerializeField] private Button rightBtn, leftBtn;
    [SerializeField] private GameObject confettiEffect;
    [SerializeField] private Text schoolNameText;
    public bool isInTransition = false, currentlyFilling = false;
    private int _schoolIndex = 0;
    private const string FILLING_CAMERA_STATE = "cameraFillingState", NORMAL_CAMERA_STATE = "cameraNormalState";
    
    #endregion
    private void Awake()
    {
        AdsCaller.Instance.ShowBanner();
        AdsCaller.Instance.HideRectBanner();
        if(GadsmeInit.Instance)
            GadsmeInit.Instance.DisableAds();
        CurrencyCounter.Instance.ShowCashImage(true);
        SharedUI.Instance.HideAll();
        SharedUI.Instance.MetaUIActivated(this);
        SoundController.Instance.PlayMetaBackgroundMusic();
        if (PlayerPrefsHandler.IsMetaFinished())
        {
            CloseMeta();
        }
        else
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, PlayerPrefsHandler.Meta);
            FirebaseManager.Instance.ReportEvent(GAProgressionStatus.Start + PlayerPrefsHandler.Meta);
        }
        _schoolIndex = PlayerPrefsHandler.schoolNo;
        for (var i = 0; i < schools.Length; i++)
        {
            if(_schoolIndex == i)
                continue;
            schools[i].FinishSchool();
        }
        SetCurrentSchool();
        SetupTutorial();
    }
    public bool GetActiveMenu(string menuName)
    {
        return allMenus.Where(t => t.menu.gameObject.activeSelf).Any(t => menuName == t.name);
    }
    public void HideAll()
    {
        foreach (var t in allMenus)
        {
            t.menu.SetActive(false);
        }
    }
    public GameObject GetMenu(string menuName)
    {
        return (from t in allMenus where t.name.Equals(menuName) select t.menu).FirstOrDefault();
    }
    public void CloseMenu(string menuToClose)
    {
        foreach (var t in allMenus)
        {
            if (!t.name.Equals(menuToClose)) continue;
            t.menu.SetActive(false);
        }
    }
    public GameObject GetSubMenu(string menuName)
    {
        return (from t in subMenus where t.name.Equals(menuName) select t.menu).FirstOrDefault();
    }
    public void HideAllSubMenus()
    {
        foreach (var t in subMenus)
        {
            t.menu.SetActive(false);
        }
    }
    public void EnableLowerButtons(bool flag)
    {
        lowerButtons.SetActive(flag);
    }
    public void EnableMainMenuTutorial(bool flag)
    {
        handTutorial0.SetActive(flag);
    }
    private void SetupTutorial()
    {
        if (PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialString)) return;
        if (!PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialStep1String))
        {
            handTutorial0.SetActive(true);
        }
        if (PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialStep2String)) return;
        SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.MainMenu);
        handTutorial1.SetActive(true);
        PlayerPrefsHandler.SetBool(PlayerPrefsHandler.TutorialStep2String, true);
    }

    private void EndMetaTutorial()
    {
        if(PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialString)) return;
        PlayerPrefsHandler.SetBool(PlayerPrefsHandler.TutorialStep1String, true);
    }

    #region Meta Methods
    
    public void HideFillingMenu()
    {
        fillerButtons.SetActive(false);
        lowerButtons.SetActive(true);
        buildingSelectionButtons.SetActive(false);
        metaCamera.Play(NORMAL_CAMERA_STATE);
    }
    public void ShowSchoolSelectionMenu()
    {
        SoundController.Instance.PlayBtnClickSound();
        buildingSelectionButtons.SetActive(true);
        lowerButtons.SetActive(false);
        metaCamera.Play(FILLING_CAMERA_STATE);
    }
    public void NoThanksBtnClicked()
    {
        if(currentlyFilling) return;
        SoundController.Instance.PlayBtnClickSound();
        if(isInTransition) return;
        CloseMeta();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, PlayerPrefsHandler.Meta);
        FirebaseManager.Instance.ReportEvent(GAProgressionStatus.Complete + PlayerPrefsHandler.Meta);
    }
    public void CloseMeta()
    {
        HideFillingMenu();
        EndMetaTutorial();
    }
    private void SetCurrentSchool()
    {
        metaCamera.transform.DOMove(cameraPositions[_schoolIndex].position, 0.5f).OnComplete(() =>
        {
            leftBtn.interactable = true;
            rightBtn.interactable = true;
        });
        currentSchool = schools[_schoolIndex];
        schoolNameText.text = currentSchool.GetSchoolName();
        currentSchool.StartFilling();
    }
    public void FillSchool(bool flag)
    {
        currentlyFilling = flag;
        currentSchool.SetFillingFlag(flag);
    }
    public void OnSchoolCompletion()
    {
        //Debug.Log("OnSchoolCompletion: " + _schoolIndex);
        isInTransition = true;
        PlayerPrefsHandler.FinishSchool(_schoolIndex);
        _schoolIndex++;
        if (_schoolIndex >= schools.Length)
        {
            _schoolIndex = schools.Length - 1;
            PlayerPrefsHandler.FinishMeta();
            CloseMeta();
            return;
        }
        confettiEffect.SetActive(true);
        SoundController.Instance.PlayBuySound();
        leftBtn.interactable = false;
        rightBtn.interactable = false;
        Invoke(nameof(MoveToNextSchool), 3f);
    }
    private void MoveToNextSchool()
    {
        PlayerPrefsHandler.schoolNo = _schoolIndex;
        PlayerPrefsHandler.UnlockSchool(schools[_schoolIndex].GetSchoolName());
        currentSchool = schools[_schoolIndex];
        currentSchool.StartFilling();
        metaCamera.transform.DOMove(cameraPositions[_schoolIndex].position, 0.5f).OnComplete(() =>
        {
            leftBtn.interactable = true;
            rightBtn.interactable = true;
            isInTransition = false;
            //blastEffect.SetActive(true);
        });
    }
    public void RightBtnClicked()
    {
        SoundController.Instance.PlayBtnClickSound();
        leftBtn.interactable = false;
        rightBtn.interactable = false;
        if (_schoolIndex < schools.Length - 1)
        {
            _schoolIndex++;
        }
        rightBtn.gameObject.SetActive(_schoolIndex != schools.Length - 1);
        leftBtn.gameObject.SetActive(true);
        SetCurrentSchool();
    }
    public void LeftBtnClicked()
    {
        SoundController.Instance.PlayBtnClickSound();
        leftBtn.interactable = false;
        rightBtn.interactable = false;
        if (_schoolIndex > 0)
        {
            _schoolIndex--;
        }
        leftBtn.gameObject.SetActive(_schoolIndex != 0);
        rightBtn.gameObject.SetActive(true);
        SetCurrentSchool();
    }
    public void EnableSchools(bool flag)
    {
        foreach (var school in schools)
        {
            school.gameObject.SetActive(flag);
        }   
    }

    #endregion
}
//using AssetKits.ParticleImage;

using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.UI;
public class School : MonoBehaviour
{
    [SerializeField] private int schoolId = 0;
    [SerializeField] private string schoolName = "American School";
    private int buildingIndex = 0;
    //[SerializeField] private Building currentBuilding;
    [SerializeField] private bool isSchoolUnlocked = false;
    [SerializeField] private MeshRenderer ground, bg;
    [SerializeField] private Building[] buildings;
    [SerializeField] private Image fillerImage;
    [SerializeField] private Text cashText;
    //[SerializeField] private ParticleImage particles;
    private bool isBuilding = false;
    [SerializeField] private CashEffect cashEffect;
    private float maxValue, minValue, currentValue;
    private int currentCashValue = 18, maxCashValue;
    [SerializeField] private GameObject fogParticles;
    [SerializeField] private Color color1, color2;
    private const string CashEffectTarget = "CashTarget";
    private static readonly int FillRate = Shader.PropertyToID("_FillRate");
    private static readonly int Color1 = Shader.PropertyToID("_Color1");
    private static readonly int Color2 = Shader.PropertyToID("_Color2");
    public void StartFilling()
    {
        //Callbacks.OnRewardCoins250 += RewardCoins;       
        if (PlayerPrefsHandler.IsSchoolUnlocked(schoolName, isSchoolUnlocked))
        {
            ground.material.SetFloat(FillRate, 1.6f);
            fogParticles.SetActive(true);
        }
        else
        {
            ground.material.SetFloat(FillRate, -0.761f);
            fogParticles.SetActive(false);
        }
        cashText.text = PlayerPrefsHandler.currency.ToString();
        SetBackgroundColor();
        if (PlayerPrefsHandler.currency <= 0)
            fillerImage.fillAmount = 0f;
        for (var i = 0; i < buildings.Length; i++)
        {
            //Debug.Log(buildings[i].GetBuildingName() + PlayerPrefsHandler.IsBuildingUnlocked(buildings[i].GetBuildingName()));
            if (PlayerPrefsHandler.IsBuildingUnlocked(buildings[i].GetBuildingName()))
            {
                //Debug.Log("buildingIndex: " + (buildings.Length));
                buildings[i].onFillingComplete?.Invoke();
                buildingIndex++;
                if (buildingIndex >= buildings.Length) return;
                //Debug.Log("buildingIndex: " + buildingIndex);
                buildings[buildingIndex].gameObject.SetActive(true);
            }
        }
        maxValue = buildings[buildingIndex].GetMaxFillValue();
        minValue = buildings[buildingIndex].GetMinFillValue();
        maxCashValue = PlayerPrefsHandler.currency;
        cashEffect.SetTarget(buildings[buildingIndex].transform.Find(CashEffectTarget));
    }
    private void EndFilling()
    {
        Callbacks.OnRewardCoins250 -= RewardCoins;
    }
    public string GetSchoolName()
    {
        return schoolName;
    }
    public void FinishSchool()
    {
        if(!PlayerPrefsHandler.IsSchoolFinished(schoolId)) return;
        foreach (var building in buildings)
        {
            building.onFillingComplete?.Invoke();
            building.gameObject.SetActive(true);
        }
        ground.material.SetFloat(FillRate, 0.5f);
    }
    private void Update()
    {
        if (PlayerPrefsHandler.IsMetaFinished()) return;
        if(!isBuilding) return;
        if(buildingIndex >= buildings.Length) return;
        if (PlayerPrefsHandler.currency <= 0)
        {
            isBuilding = false;
            cashEffect.gameObject.SetActive(false);
            CurrencyCounter.Instance.UpdateCoinsText();
            SharedUI.Instance.metaUIManager.HideFillingMenu();
            return;
        }
        currentValue = buildings[buildingIndex].GetCurrentFillValue();
        if (currentValue >= maxValue)
        {
            BuildingFillingCompletion();
            return;
        }
        buildings[buildingIndex].Fill();
        var fillAmount = Mathf.InverseLerp(0, maxCashValue, PlayerPrefsHandler.currency);
        fillerImage.fillAmount = fillAmount;
        cashText.text = PlayerPrefsHandler.currency.ToString();
    }
    public void Fill(bool flag)
    {
        /*if (flag)
        {
            particles.loop = true;
            particles.Play();
        }
        else
        {
            particles.loop = false;
        }*/
    }
    public void SetFillingFlag(bool flag)
    {
        if(SharedUI.Instance.metaUIManager.isInTransition) return;
        //Debug.Log("IsMetaFinished: " + PlayerPrefsHandler.IsMetaFinished());
        if (PlayerPrefsHandler.IsMetaFinished()) return;
        //Debug.Log("IsSchoolUnlocked: " + PlayerPrefsHandler.IsSchoolUnlocked(schoolName, isSchoolUnlocked));
        if (!PlayerPrefsHandler.IsSchoolUnlocked(schoolName, isSchoolUnlocked)) return;
        if (PlayerPrefsHandler.currency <= 0) return;
        isBuilding = flag;
        cashEffect.gameObject.SetActive(flag);
        if(!flag)
            CurrencyCounter.Instance.UpdateCoinsText();
    }
    private void BuildingFillingCompletion()
    {
        //Debug.Log("BuildingFillingCompletion");
        isBuilding = false;
        cashEffect.gameObject.SetActive(false);
        buildings[buildingIndex].onFillingComplete?.Invoke();
        PlayerPrefsHandler.UnlockBuilding(buildings[buildingIndex].GetBuildingName());
        buildingIndex++;
        if (buildingIndex >= buildings.Length)
        {
            buildingIndex = buildings.Length - 1;
            SharedUI.Instance.metaUIManager.OnSchoolCompletion();
            return;
        }
        //fillerImage.fillAmount = 1f;
        buildings[buildingIndex].gameObject.SetActive(true);
        cashEffect.SetTarget(buildings[buildingIndex].transform.Find(CashEffectTarget));
        //maxCashValue = buildings[buildingIndex].GetTotalCost();
        maxValue = buildings[buildingIndex].GetMaxFillValue();
        minValue = buildings[buildingIndex].GetMinFillValue();
        CurrencyCounter.Instance.UpdateCoinsText();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, schoolName);
        FirebaseManager.Instance.ReportEvent(GAProgressionStatus.Complete + schoolName);
    }
    private void RewardCoins()
    {
        cashText.text = PlayerPrefsHandler.currency.ToString();
        fillerImage.fillAmount = 1f;
        maxCashValue = PlayerPrefsHandler.currency;
    }

    private void SetBackgroundColor()
    {
        //Debug.Log("SetBackgroundColor");
        bg.material.SetColor(Color1, color1);
        bg.material.SetColor(Color2, color2);
    }
}
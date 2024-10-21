using System;
using UnityEngine;
using UnityEngine.UI;

public class SchoolBuilding : MonoBehaviour
{
    [Serializable]
    private class Building
    {
        public GameObject buildingMesh;
        public int buildingPrice;
    }
    [SerializeField] private Building[] mainBuilding, cafeteria, playGround, arena;
    [SerializeField] private SchoolArea[] schoolAreas;
    [SerializeField] private GameObject perfects;
    [SerializeField] private GameObject tutorialHighlighter;
    [SerializeField] private Sprite cashSprite, adSprite;
    private string buildingToUnlock = BuildingNames.MainBuilding.ToString();
    private const string Smoke = "Smoke", Free = "Free", CashIcon = "CashIcon";
    public enum BuildingNames
    {
        MainBuilding,
        Cafeteria,
        PlayGround,
        Arena
    }
    private void Start()
    {
        BuildCompleteSchool();
        SetupTutorial();
    }
    private void OnEnable()
    {
        Callbacks.OnRewardSchoolBuilding += RewardSchoolBuilding;
    }
    private void OnDisable()
    {
        Callbacks.OnRewardSchoolBuilding -= RewardSchoolBuilding;
    }
    private void SetupTutorial()
    {
        if (PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialString)) return;
        if (!PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialStep1String))
        {
            SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.TutorialString);
            tutorialHighlighter.SetActive(true);
            return;
        }
        if (PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialStep2String)) return;
        SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.MainMenu);
        tutorialHighlighter.SetActive(false);
        SharedUI.Instance.metaUIManager.EnableMainMenuTutorial(true);
        PlayerPrefsHandler.SetBool(PlayerPrefsHandler.TutorialStep2String, true);
    }

    private void EndMetaTutorial()
    {
        if(PlayerPrefsHandler.GetBool(PlayerPrefsHandler.TutorialString)) return;
        PlayerPrefsHandler.SetBool(PlayerPrefsHandler.TutorialStep1String, true);
        SetupTutorial();
    }
    private void BuildCompleteSchool()
    {
        BuildSchool(BuildingNames.MainBuilding.ToString());
        BuildSchool(BuildingNames.Cafeteria.ToString());
        BuildSchool(BuildingNames.PlayGround.ToString());
        BuildSchool(BuildingNames.Arena.ToString());
    }
    private void BuildSchool(string buildingName, bool isBuying = false)
    {
        //Debug.Log("BuildingName: " + buildingName);
        if (buildingName == BuildingNames.MainBuilding.ToString())
        {
            if (isBuying)
            {
                if (PlayerPrefsHandler.schoolMainBuildingRank < 3)
                {
                    PlayerPrefsHandler.schoolMainBuildingRank++;
                    if (mainBuilding[PlayerPrefsHandler.schoolMainBuildingRank].buildingPrice <=
                        PlayerPrefsHandler.currency)
                    {
                        CurrencyCounter.Instance.CurrencyDeduction(mainBuilding[PlayerPrefsHandler.schoolMainBuildingRank].buildingPrice);
                        /*perfects.SetActive(true);
                        schoolAreas[0].transform.Find(Smoke).gameObject.SetActive(true);*/
                    }
                    perfects.SetActive(true);
                    schoolAreas[0].transform.Find(Smoke).gameObject.SetActive(true);
                    /*else
                    {
                        PlayerPrefsHandler.schoolMainBuildingRank--;
                        return;
                    }*/
                }
                if (PlayerPrefsHandler.schoolMainBuildingRank == 3)
                {
                    schoolAreas[0].EnableCanvas(false);
                }
            }
            foreach (var t in mainBuilding)
            {
                t.buildingMesh.SetActive(false);
            }
            mainBuilding[PlayerPrefsHandler.schoolMainBuildingRank].buildingMesh.SetActive(true);
            SetButtons();
            /*var priceIndex = PlayerPrefsHandler.schoolMainBuildingRank;
            if (priceIndex < 3)
                priceIndex++;
            var price = mainBuilding[priceIndex].buildingPrice;
            var txt = schoolAreas[0].GetPriceTextTransform().GetComponent<Text>();
            var cashIcon = txt.transform.Find(CashIcon).GetComponent<Image>();
            if (price > PlayerPrefsHandler.currency)
            {
                txt.text = Free;
                cashIcon.sprite = adSprite;
            }
            else
            {
                txt.text = price.ToString();
                cashIcon.sprite = cashSprite;
            }*/
            //schoolAreas[0].GetPriceTextTransform().GetComponent<Text>().text = mainBuilding[priceIndex].buildingPrice.ToString();
        }
        else if (buildingName == BuildingNames.Cafeteria.ToString())
        {
            if (isBuying)
            {
                if (PlayerPrefsHandler.schoolCafeteriaRank < 3)
                {
                    PlayerPrefsHandler.schoolCafeteriaRank++;
                    if (cafeteria[PlayerPrefsHandler.schoolCafeteriaRank].buildingPrice <=
                        PlayerPrefsHandler.currency)
                    {
                        CurrencyCounter.Instance.CurrencyDeduction(cafeteria[PlayerPrefsHandler.schoolCafeteriaRank].buildingPrice);
                        /*perfects.SetActive(true);
                        schoolAreas[1].transform.Find(Smoke).gameObject.SetActive(true);*/
                    }
                    perfects.SetActive(true);
                    schoolAreas[1].transform.Find(Smoke).gameObject.SetActive(true);
                    /*else
                    {
                        PlayerPrefsHandler.schoolCafeteriaRank--;
                        return;
                    }*/
                }
                if (PlayerPrefsHandler.schoolCafeteriaRank == 3)
                {
                    schoolAreas[1].EnableCanvas(false);
                }
            }
            foreach (var t in cafeteria)
            {
                t.buildingMesh.SetActive(false);
            }
            cafeteria[PlayerPrefsHandler.schoolCafeteriaRank].buildingMesh.SetActive(true);
            SetButtons();
            /*var priceIndex = PlayerPrefsHandler.schoolCafeteriaRank;
            if (priceIndex < 3)
                priceIndex++;
            var price = mainBuilding[priceIndex].buildingPrice;
            var txt = schoolAreas[1].GetPriceTextTransform().GetComponent<Text>();
            var cashIcon = txt.transform.Find(CashIcon).GetComponent<Image>();
            if (price > PlayerPrefsHandler.currency)
            {
                txt.text = Free;
                cashIcon.sprite = adSprite;
            }
            else
            {
                txt.text = price.ToString();
                cashIcon.sprite = cashSprite;
            }*/
            //schoolAreas[1].GetPriceTextTransform().GetComponent<Text>().text = mainBuilding[priceIndex].buildingPrice.ToString();
        }
        else if (buildingName == BuildingNames.PlayGround.ToString())
        {
            if (isBuying)
            {
                if (PlayerPrefsHandler.schoolPlayGroundRank < 3)
                {
                    PlayerPrefsHandler.schoolPlayGroundRank++;
                    if (playGround[PlayerPrefsHandler.schoolPlayGroundRank].buildingPrice <=
                        PlayerPrefsHandler.currency)
                    {
                        CurrencyCounter.Instance.CurrencyDeduction(playGround[PlayerPrefsHandler.schoolPlayGroundRank].buildingPrice);
                        /*perfects.SetActive(true);
                        schoolAreas[2].transform.Find(Smoke).gameObject.SetActive(true);*/
                    }
                    perfects.SetActive(true);
                    schoolAreas[2].transform.Find(Smoke).gameObject.SetActive(true);
                    /*else
                    {
                        PlayerPrefsHandler.schoolPlayGroundRank--;
                        return;
                    }   */
                }
                if (PlayerPrefsHandler.schoolPlayGroundRank == 3)
                {
                    schoolAreas[2].EnableCanvas(false);
                }
            }
            foreach (var t in playGround)
            {
                t.buildingMesh.SetActive(false);
            }
            playGround[PlayerPrefsHandler.schoolPlayGroundRank].buildingMesh.SetActive(true);
            SetButtons();
            /*var priceIndex = PlayerPrefsHandler.schoolPlayGroundRank;
            if (priceIndex < 3)
                priceIndex++;
            var price = mainBuilding[priceIndex].buildingPrice;
            var txt = schoolAreas[2].GetPriceTextTransform().GetComponent<Text>();
            var cashIcon = txt.transform.Find(CashIcon).GetComponent<Image>();
            if (price > PlayerPrefsHandler.currency)
            {
                txt.text = Free;
                cashIcon.sprite = adSprite;
            }
            else
            {
                txt.text = price.ToString();
                cashIcon.sprite = cashSprite;
            }*/
            //schoolAreas[2].GetPriceTextTransform().GetComponent<Text>().text = mainBuilding[priceIndex].buildingPrice.ToString();
        }
        else
        {
            if (isBuying)
            {
                if (PlayerPrefsHandler.schoolArenaRank < 3)
                {
                    PlayerPrefsHandler.schoolArenaRank++;
                    if (arena[PlayerPrefsHandler.schoolArenaRank].buildingPrice <=
                        PlayerPrefsHandler.currency)
                    {
                        CurrencyCounter.Instance.CurrencyDeduction(arena[PlayerPrefsHandler.schoolArenaRank].buildingPrice);
                        /*perfects.SetActive(true);
                        schoolAreas[3].transform.Find(Smoke).gameObject.SetActive(true);*/
                    }
                    perfects.SetActive(true);
                    schoolAreas[3].transform.Find(Smoke).gameObject.SetActive(true);
                    /*else
                    {
                        PlayerPrefsHandler.schoolArenaRank--;
                        return;
                    }*/
                }
                if (PlayerPrefsHandler.schoolArenaRank == 3)
                {
                    schoolAreas[3].EnableCanvas(false);
                }
            }
            foreach (var t in arena)
            {
                t.buildingMesh.SetActive(false);
            }
            arena[PlayerPrefsHandler.schoolArenaRank].buildingMesh.SetActive(true);
            SetButtons();
            /*var priceIndex = PlayerPrefsHandler.schoolArenaRank;
            if (priceIndex < 3)
                priceIndex++;
            var price = mainBuilding[priceIndex].buildingPrice;
            var txt = schoolAreas[3].GetPriceTextTransform().GetComponent<Text>();
            var cashIcon = txt.transform.Find(CashIcon).GetComponent<Image>();
            if (price > PlayerPrefsHandler.currency)
            {
                txt.text = Free;
                cashIcon.sprite = adSprite;
            }
            else
            {
                txt.text = price.ToString();
                cashIcon.sprite = cashSprite;
            }*/
            //schoolAreas[3].GetPriceTextTransform().GetComponent<Text>().text = mainBuilding[priceIndex].buildingPrice.ToString();
        }
    }
    public void HireBuilder(string buildingName)
    {
        BuildSchool(buildingName, true);
        EndMetaTutorial();
    }
    public void BuyBtnClicked(string buildingName)
    {
        buildingToUnlock = buildingName;
        if (buildingName == BuildingNames.MainBuilding.ToString())
        {
            Debug.Log(mainBuilding[PlayerPrefsHandler.schoolMainBuildingRank + 1].buildingPrice + " : " + PlayerPrefsHandler.currency);
            if (mainBuilding[PlayerPrefsHandler.schoolMainBuildingRank + 1].buildingPrice > PlayerPrefsHandler.currency)
            {
                Callbacks.rewardType = Callbacks.RewardType.RewardSchoolBuilding;
                AdsCaller.Instance.ShowRewardedAd();
            }
            else
            {
                HireBuilder(buildingName);
            }
        }
        else if(buildingName == BuildingNames.Cafeteria.ToString())
        {
            if (cafeteria[PlayerPrefsHandler.schoolCafeteriaRank + 1].buildingPrice > PlayerPrefsHandler.currency)
            {
                Callbacks.rewardType = Callbacks.RewardType.RewardSchoolBuilding;
                AdsCaller.Instance.ShowRewardedAd();
            }
            else
            {
                HireBuilder(buildingName);
            }
        }
        else if(buildingName == BuildingNames.PlayGround.ToString())
        {
            if (playGround[PlayerPrefsHandler.schoolPlayGroundRank + 1].buildingPrice > PlayerPrefsHandler.currency)
            {
                Callbacks.rewardType = Callbacks.RewardType.RewardSchoolBuilding;
                AdsCaller.Instance.ShowRewardedAd();
            }
            else
            {
                HireBuilder(buildingName);
            }
        }
        else
        {
            if (arena[PlayerPrefsHandler.schoolArenaRank + 1].buildingPrice > PlayerPrefsHandler.currency)
            {
                Callbacks.rewardType = Callbacks.RewardType.RewardSchoolBuilding;
                AdsCaller.Instance.ShowRewardedAd();
            }
            else
            {
                HireBuilder(buildingName);
            }
        }
    }
    private void RewardSchoolBuilding()
    {
        HireBuilder(buildingToUnlock);
    }
    private void SetButtons()
    {
        var priceIndex = PlayerPrefsHandler.schoolMainBuildingRank;
        if (priceIndex < 3)
            priceIndex++;
        var price = mainBuilding[priceIndex].buildingPrice;
        var txt = schoolAreas[0].GetPriceTextTransform().GetComponent<Text>();
        var cashIcon = txt.transform.Find(CashIcon).GetComponent<Image>();
        if (price > PlayerPrefsHandler.currency)
        {
            txt.text = Free;
            cashIcon.sprite = adSprite;
        }
        else
        {
            txt.text = price.ToString();
            cashIcon.sprite = cashSprite;
        }
        priceIndex = PlayerPrefsHandler.schoolCafeteriaRank;
        if (priceIndex < 3)
            priceIndex++;
        price = cafeteria[priceIndex].buildingPrice;
        txt = schoolAreas[1].GetPriceTextTransform().GetComponent<Text>();
        cashIcon = txt.transform.Find(CashIcon).GetComponent<Image>();
        if (price > PlayerPrefsHandler.currency)
        {
            txt.text = Free;
            cashIcon.sprite = adSprite;
        }
        else
        {
            txt.text = price.ToString();
            cashIcon.sprite = cashSprite;
        }
        priceIndex = PlayerPrefsHandler.schoolPlayGroundRank;
        if (priceIndex < 3)
            priceIndex++;
        price = playGround[priceIndex].buildingPrice;
        txt = schoolAreas[2].GetPriceTextTransform().GetComponent<Text>();
        cashIcon = txt.transform.Find(CashIcon).GetComponent<Image>();
        if (price > PlayerPrefsHandler.currency)
        {
            txt.text = Free;
            cashIcon.sprite = adSprite;
        }
        else
        {
            txt.text = price.ToString();
            cashIcon.sprite = cashSprite;
        }
        priceIndex = PlayerPrefsHandler.schoolArenaRank;
        if (priceIndex < 3)
            priceIndex++;
        price = arena[priceIndex].buildingPrice;
        txt = schoolAreas[3].GetPriceTextTransform().GetComponent<Text>();
        cashIcon = txt.transform.Find(CashIcon).GetComponent<Image>();
        if (price > PlayerPrefsHandler.currency)
        {
            txt.text = Free;
            cashIcon.sprite = adSprite;
        }
        else
        {
            txt.text = price.ToString();
            cashIcon.sprite = cashSprite;
        }
    }
}
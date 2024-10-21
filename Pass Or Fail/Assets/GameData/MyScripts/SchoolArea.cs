using UnityEngine;
public class SchoolArea : MonoBehaviour
{
    [SerializeField] private SchoolBuilding.BuildingNames buildingName;
    [SerializeField] private GameObject canvas;
    public void EnableCanvas(bool flag)
    {
        if (IsBuildingFullyUpgraded())
        {
            canvas.SetActive(false);
            gameObject.SetActive(false);
            return;
        }
        canvas.SetActive(flag);
    }
    private bool IsBuildingFullyUpgraded()
    {
        return buildingName switch
        {
            SchoolBuilding.BuildingNames.MainBuilding => PlayerPrefsHandler.schoolMainBuildingRank >= 3,
            SchoolBuilding.BuildingNames.Cafeteria => PlayerPrefsHandler.schoolCafeteriaRank >= 3,
            SchoolBuilding.BuildingNames.PlayGround => PlayerPrefsHandler.schoolPlayGroundRank >= 3,
            SchoolBuilding.BuildingNames.Arena => PlayerPrefsHandler.schoolArenaRank >= 3,
            _ => false
        };
    }
    public Transform GetPriceTextTransform()
    {
        return canvas.transform.Find("UpgradeBtn/PriceText");
    }
}
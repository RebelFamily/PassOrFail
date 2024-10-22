[System.Serializable]
public class InAppProduct
{
    public UnityEngine.Purchasing.ProductType purchaseableType = UnityEngine.Purchasing.ProductType.Consumable;
    public string purchaseID = "";
    public InAppProductType itemType;
    public string price;
    public string localPrice;
    public enum InAppProductType
    {
        RemoveAds
    }
}
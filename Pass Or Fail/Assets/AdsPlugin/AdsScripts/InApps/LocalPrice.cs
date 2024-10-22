using UnityEngine;
using UnityEngine.UI;
public class LocalPrice : MonoBehaviour
{
    private Text _priceText;
    public InAppProduct.InAppProductType itemType;
    private void Start()
    {
        _priceText = GetComponent<Text>();
        foreach (var t in IAPManager.Instance.purchaseIDController)
        {
            if (itemType != t.itemType) continue;
            _priceText.text = t.localPrice;
            break;
        }
    }
}
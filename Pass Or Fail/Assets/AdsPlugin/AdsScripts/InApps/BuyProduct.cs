using UnityEngine;
using UnityEngine.UI;
namespace Mateen.OneLine
{
    [RequireComponent(typeof(Button))]
    public class BuyProduct : MonoBehaviour
    {
        [SerializeField] private InAppProduct.InAppProductType purchaseType;
        private Button _button;
        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(BuyButtonClick);
            Callbacks.OnInAppProductPurchased += ProductBought;
        }
        private void OnDisable()
        {
            _button.onClick.RemoveListener(BuyButtonClick);
            Callbacks.OnInAppProductPurchased -= ProductBought;
        }
        private void BuyButtonClick()
        {
            IAPManager.Instance.InAppCaller(purchaseType);
        }
        private void ProductBought(InAppProduct.InAppProductType product)
        {
            switch (product)
            {
                case InAppProduct.InAppProductType.RemoveAds:
                    PlayerPrefsHandler.SetBool(PlayerPrefsHandler.RemoveAds,true);
                    SharedUI.Instance.CloseSubMenu();
                    break;
            }
        }
    }
}
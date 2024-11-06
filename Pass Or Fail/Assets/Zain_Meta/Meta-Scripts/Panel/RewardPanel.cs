using UnityEngine;
using UnityEngine.UI;

namespace Zain_Meta.Meta_Scripts.Panel
{
    public class RewardPanel : MonoBehaviour
    {
        public static RewardPanel Instance;

        private void Awake()
        {
            Instance = this;
            canvasGroup.HideCanvas();
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Text priceText;

        public void PopulateThePanel(int rewardAmount, Transform targetCase, Collider col)
        {
            rewardAmount.SetFloatingPoint(priceText, " CASH");
            canvasGroup.SmoothShowCanvas();
        }


        public void CloseThePanel(bool toHideTheReward)
        {
            canvasGroup.SmoothHideCanvas();
        }

        public void RewardInvestment()
        {
            canvasGroup.HideCanvas();
            Callbacks.rewardType = Callbacks.RewardType.GroundCashInMeta;
            AdsCaller.Instance.ShowRewardedAd();
        }
    }
}
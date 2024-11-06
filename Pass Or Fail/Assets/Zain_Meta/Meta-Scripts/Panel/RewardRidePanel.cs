using UnityEngine;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.Panel
{
    public class RewardRidePanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup panelCanvas;
        [SerializeField] private RectTransform timerBar;
        [SerializeField] private Image rideImage;
        [SerializeField] private Text timerText;
        [SerializeField] private Image fillerImage;

        [SerializeField] private ArcadeMovement.PlayerState curStateToGive;
        private float _curTimer;


        public void SetRideRender(PlayerRideData rideData)
        {
            panelCanvas.SmoothShowCanvas();
            rideImage.sprite = rideData.renderToShow;
            curStateToGive = rideData.stateToGive;
        }

        public void HideThePanel()
        {
            panelCanvas.SmoothHideCanvas();
        }

        public void ShowTheTimerBar(bool val)
        {
            timerBar.ScaleUI(val, .15f);
        }

        public void SetTheTimer(float timer, float totalTime)
        {
            timer.CountDownTimer(timerText);
          //  fillerImage.fillAmount = Mathf.InverseLerp(totalTime, 0, timer);
            fillerImage.fillAmount = Mathf.InverseLerp(0, totalTime, timer);
        }

        public void ClickedWatchAd()
        {
            switch (curStateToGive)
            {
                case ArcadeMovement.PlayerState.RidingSkateboard:
                    Callbacks.rewardType = Callbacks.RewardType.RewardSkateboard;
                    break;
                case ArcadeMovement.PlayerState.RidingOneWheelCycle:
                    Callbacks.rewardType = Callbacks.RewardType.RewardUniCycle;
                    break;
            }

            AdsCaller.Instance.ShowRewardedAd();
        }
    }
}
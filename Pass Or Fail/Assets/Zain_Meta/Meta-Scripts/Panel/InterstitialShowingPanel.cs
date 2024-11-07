using UnityEngine;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.MetaRelated.Upgrades;

namespace Zain_Meta.Meta_Scripts.Panel
{
    public class InterstitialShowingPanel : MonoBehaviour
    {
        [SerializeField] private float interPanelShowingDelay;
        [SerializeField] private CanvasGroup panel;

        private void Awake()
        {
            _curTimer = interPanelShowingDelay;
            _panelShown = true;
        }


        private float _curTimer;
        private bool _stopTimer, _panelShown;

        private void LateUpdate()
        {
            if (_stopTimer) return;
            if (_panelShown) return;

            if (_curTimer < .1f)
            {
                _curTimer = interPanelShowingDelay;
                ShowInterPanel();
            }
            else
            {
                _curTimer -= Time.deltaTime;
            }
        }

        private void OnEnable()
        {
            EventsManager.OnTriggerTeaching += AdjustTimer;
            EventsManager.OnClassReadyToUpgrade += AdjustTimer;
            EventsManager.OnTutComplete += StartTimer;
            Callbacks.OnRewardGroundCashInMeta += HidePanel;
            Callbacks.OnRewardARide += HidePanel;
            EventsManager.OnTriggerWithReward += AdjustTimer;
        }

        private void OnDisable()
        {
            EventsManager.OnTriggerTeaching -= AdjustTimer;
            EventsManager.OnClassReadyToUpgrade -= AdjustTimer;
            EventsManager.OnTutComplete -= StartTimer;
            Callbacks.OnRewardGroundCashInMeta -= HidePanel;
            Callbacks.OnRewardARide -= HidePanel;
            EventsManager.OnTriggerWithReward -= AdjustTimer;
        }

        private void AdjustTimer(bool inTrigger)
        {
            _stopTimer = inTrigger;
        }

        private void HidePanel(Callbacks.RewardType rideType)
        {
            HidePanel();
        }

        private void HidePanel()
        {
            _panelShown = false;
            _stopTimer = false;
            panel.HideCanvas();
        }

        private void StartTimer()
        {
            _panelShown = false;
        }

        private void AdjustTimer(ClassroomUpgradeProfile arg1, bool startingToUpgrade)
        {
            _stopTimer = startingToUpgrade;
        }

        private void AdjustTimer(bool hasInitiated, Vector3 arg2, Vector3 arg3, ClassroomProfile arg4)
        {
            _stopTimer = hasInitiated;
        }

        private void ShowInterPanel()
        {
            if (AdsCaller.Instance.IsInterstitialAdAvailable())
            {
                panel.ShowCanvas();
                _panelShown = true;
                EventsManager.InterPopupShown(true);
            }
        }

        public void ShowInter()
        {
            HidePanel();
            EventsManager.InterPopupShown(false);
            AdsCaller.Instance.ShowInterstitialAd();
        }

        public void GiveMeCycle()
        {
            Callbacks.rewardType = Callbacks.RewardType.RewardUniCycle;
            AdsCaller.Instance.ShowRewardedAd();
            EventsManager.InterPopupShown(false);
        }

        public void RewardInvestment()
        {
            Callbacks.rewardType = Callbacks.RewardType.GroundCashInMeta;
            AdsCaller.Instance.ShowRewardedAd();
            EventsManager.InterPopupShown(false);
        }
    }
}
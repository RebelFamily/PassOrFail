using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PassOrFail.MiniGames
{
    public class BreakFightCanvas : MonoBehaviour
    {
        [SerializeField] private Button fightBreakBtn, sprayBtn ,tapItBtn;

        [SerializeField] private  Image fillBar;
        private bool _isAlreadyStoppedFight,_isAlreadyGameStart;
        [SerializeField] private GameObject stopUi;
        [SerializeField] private UnityEvent onScreenClick;
        [SerializeField] private GameObject spray;
        private void OnEnable()
        {
            fightBreakBtn.onClick.AddListener(BreakFightClick);
            sprayBtn.onClick.AddListener(SprayClick);
            tapItBtn.onClick.AddListener(ClickOnScreen);
            Callbacks.OnRewardSpray += ReceiveSprayReward;
        }

        private void OnDisable()
        {
            fightBreakBtn.onClick.RemoveListener(BreakFightClick);
            sprayBtn.onClick.RemoveListener(SprayClick);
            tapItBtn.onClick.RemoveListener(ClickOnScreen);
            Callbacks.OnRewardSpray -= ReceiveSprayReward;
        }

        private void BreakFightClick()
        {
            var filledAmount = fillBar.fillAmount + .05f;
            fillBar.fillAmount = filledAmount;
            CheckIfFilled(filledAmount);
        }

        private void SprayClick()
        {
            if(_isAlreadyStoppedFight) return; 
            Callbacks.rewardType = Callbacks.RewardType.RewardSpray;
            AdsCaller.Instance.ShowRewardedAd();
        }

        private void ClickOnScreen()
        {
            if(_isAlreadyGameStart) return;
            _isAlreadyGameStart = true;
            onScreenClick?.Invoke();
            tapItBtn.gameObject.SetActive(false);
            Invoke(nameof(ActivateStopGameUi),1f);
        }

        private void ActivateStopGameUi()
        {
            stopUi.SetActive(true);
        }

        private void ReceiveSprayReward()
        {
            spray.SetActive(true);
            fillBar.DOFillAmount(1, .5f).OnComplete((() =>
            {
                CheckIfFilled(1);
            }));
        }
        private void CheckIfFilled(float amount)
        {
            if(amount < 1) return;
            //Game Complete
            _isAlreadyStoppedFight = true;
            EventManager.InvokeStopPlayerFight();
            //onStopFight?.Invoke();
            fightBreakBtn.interactable = false;
            sprayBtn.interactable = false;
            gameObject.SetActive(false);
        }
    }
}
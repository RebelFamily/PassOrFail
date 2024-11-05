using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.Panel;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.Components
{
    [SelectionBase]
    public class RideReward : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Transform placingPos;
        [SerializeField] private PlayerRideData[] ridesData;
        [SerializeField] private float itemChangingTime, timeForRideToRemainActive;
        [SerializeField] private RewardRidePanel rewardRidePanel;
        [SerializeField] private Transform itemsParent;
        [SerializeField] private Image timerFiller;
        [SerializeField] private float changeInSize, triggerDuration;
        [SerializeField] private Color collisionColor, normalColor;
        private Vector3 _actualSize;
        private float _curTimerChanging, _curTimerActivation;
        private Collider _myCollider;
        private bool _isShowing, _isUsing, _canShow, _work;
        private bool _isTriggering;
        private int _curItemShowingIndex;

        private void Awake()
        {
            var index = PlayerPrefs.GetInt("IsShowingRide", 0);
            _isUsing = index == 1;
        }

        private void Start()
        {
            _actualSize = targetTransform.localScale;
            _myCollider = GetComponent<Collider>();
            timerFiller.fillAmount = 0;
            _curTimerChanging = itemChangingTime;
            _curTimerActivation = timeForRideToRemainActive;
            _curTimerActivation = PlayerPrefs.GetFloat("timerRideReward", _curTimerActivation);
            rewardRidePanel.ShowTheTimerBar(false);
            Show(OnBoardingManager.TutorialComplete);
            if (!_isUsing) return;
            Show(false);
            rewardRidePanel.ShowTheTimerBar(true);
        }


        private void LateUpdate()
        {
            if (!_work) return;
            if (_isUsing)
                TimerForActivating();

            TimerForShowing();
        }

        private void TimerForShowing()
        {
            if (_isUsing || !_canShow)
            {
                Show(false);
                return;
            }

            if (_isShowing || _isTriggering) return;

            if (_curTimerChanging < .1f)
            {
                _curTimerChanging = itemChangingTime;
                ChangeItem();
            }
            else
            {
                _curTimerChanging -= Time.deltaTime;
            }
        }

        private void TimerForActivating()
        {
            if (_curTimerActivation < .1f)
            {
                _curTimerActivation = timeForRideToRemainActive;
                _isUsing = false;
                _isShowing = false;
                _canShow = true;
                Show(true);
                rewardRidePanel.ShowTheTimerBar(false);
                PlayerPrefs.SetInt("IsShowingRide", 0);
                EventsManager.BackToFootEvent();
            }
            else
            {
                _curTimerActivation -= Time.deltaTime;
                PlayerPrefs.SetFloat("timerRideReward", _curTimerActivation);
            }

            rewardRidePanel.SetTheTimer(_curTimerActivation);
        }

        private void ChangeItem()
        {
            ridesData[_curItemShowingIndex].objectToShow.transform.DOScale(0, .15f).OnComplete(() =>
            {
                _curItemShowingIndex++;
                _curItemShowingIndex %= ridesData.Length;
                ridesData[_curItemShowingIndex].objectToShow.transform.DOScale(1, .15f);
            });
        }


        private void CheckForShowing(Callbacks.RewardType rideType)
        {
            if (rideType == Callbacks.RewardType.RewardUniCycle || rideType == Callbacks.RewardType.RewardSkateboard)
            {
                _curTimerActivation = timeForRideToRemainActive;
                Show(false);
                rewardRidePanel.ShowTheTimerBar(true);
                rewardRidePanel.HideThePanel();
                PlayerPrefs.SetInt("IsShowingRide", 1);
                _isUsing = true;
                _work = true;
            }
        }


        private void Show(bool val)
        {
            _myCollider.enabled = val;
            itemsParent.gameObject.SetActive(val);
            targetTransform.gameObject.SetActive(val);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCollisionDetection _))
            {
                _isTriggering = true;
                DOTween.Kill(targetTransform);
                timerFiller.color = collisionColor;
                targetTransform.DOScale(_actualSize, 0).SetEase(Ease.Linear);
                targetTransform.DOScale(changeInSize, .1f).SetEase(Ease.Linear);
                _isShowing = true;
                DOVirtual.DelayedCall(1f, () => { StartCoroutine(nameof(FillingCoroutine)); });
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerCollisionDetection _))
            {
                _isTriggering = false;
                DOTween.Kill(targetTransform);
                timerFiller.color = normalColor;
                timerFiller.fillAmount = 0;
                _isShowing = false;
                targetTransform.DOScale(_actualSize, 0.1f).SetEase(Ease.Linear);
                StopCoroutine(nameof(FillingCoroutine));
            }
        }

        private IEnumerator FillingCoroutine()
        {
            if (!_isTriggering) yield break;

            float timeElapsed = 0;
            while (timeElapsed < triggerDuration)
            {
                var time = timeElapsed / triggerDuration;
                timerFiller.fillAmount = Mathf.Lerp(0, 1, time);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            timerFiller.fillAmount = 0;
            rewardRidePanel.SetRideRender(ridesData[_curItemShowingIndex]);
        }

        private void OnEnable()
        {
            EventsManager.OnTutComplete += ShowTheRides;
            Callbacks.OnRewardARide += CheckForShowing;
            EventsManager.OnTriggeredWithRoom += PlaceWithThere;
        }

        private void OnDisable()
        {
            EventsManager.OnTutComplete -= ShowTheRides;
            Callbacks.OnRewardARide -= CheckForShowing;
            EventsManager.OnTriggeredWithRoom -= PlaceWithThere;
        }


        private void PlaceWithThere(Transform newPlacingPos)
        {
            if (newPlacingPos == null) return;
            if (newPlacingPos == placingPos) return;

            placingPos = newPlacingPos;
            transform.DOScale(0, .15f).SetEase(Ease.Linear)
                .OnComplete(() => { transform.position = placingPos.position; });

            DOVirtual.DelayedCall(0.2f, () => { transform.DOScale(1, .15f).SetEase(Ease.Linear); });
        }

        private void ShowTheRides()
        {
            Show(false);
            _canShow = false;
            print("Can Show The RW Ride");
            _canShow = true;
            _work = true;
            Show((true));
        }
    }
}
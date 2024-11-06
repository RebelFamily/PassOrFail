using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.Panel;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class OnGroundReward : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform, gfx;
        [SerializeField] private float changeInSize, reappearingDelay;
        private Vector3 _actualSize;
        private bool _hasTriggered;
        private bool _isUsed;
        private float _curTimer;
        private RewardPanel _rewardPanel;
        private Collider _myCol;

        private void Awake()
        {
            _myCol = GetComponent<Collider>();
            _actualSize = targetTransform.localScale;
            _myCol.enabled = false;
            gfx.gameObject.SetActive(false);
            _hasTriggered = false;
        }


        private void OnEnable()
        {
            Callbacks.OnRewardGroundCashInMeta += Hide;
            EventsManager.OnTutComplete += Show;
            EventsManager.OnInterPopupShown += StopTriggeringWithThis;
        }

      

        private void OnDisable()
        {
            Callbacks.OnRewardGroundCashInMeta -= Hide;
            EventsManager.OnTutComplete -= Show;
            EventsManager.OnInterPopupShown -= StopTriggeringWithThis;
        }

        private void Hide()
        {
            _actualSize = targetTransform.localScale;
            _myCol.enabled = false;
            gfx.gameObject.SetActive(false);
            _hasTriggered = false;
            _curTimer = reappearingDelay;
            _isUsed = true;
        }

        private void StopTriggeringWithThis()
        {
            StopCoroutine(nameof(RewardCoroutine));
            _rewardPanel.CloseThePanel(false);
            _hasTriggered = false;
            EventsManager.TriggerWithRewardEvent(false);
        }
        private void Show()
        {
            _myCol.enabled = true;
            gfx.gameObject.SetActive(true);
            _hasTriggered = false;
            _curTimer = reappearingDelay;
        }

        private void Start()
        {
            _rewardPanel = RewardPanel.Instance;
        }

        private void LateUpdate()
        {
            if (!_isUsed) return;

            if (_curTimer < .1f)
            {
                _curTimer = reappearingDelay;
                Show();
                _isUsed = false;
            }
            else
            {
                _curTimer -= Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ArcadeMovement _))
            {
                DOTween.Kill(targetTransform);
                targetTransform.DOScale(_actualSize, 0).SetEase(Ease.Linear);
                targetTransform.DOScale(changeInSize, .25f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                StartCoroutine(nameof(RewardCoroutine));
                EventsManager.TriggerWithRewardEvent(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ArcadeMovement _))
            {
                StopCoroutine(nameof(RewardCoroutine));
                _rewardPanel.CloseThePanel(false);
                DOTween.Kill(targetTransform);
                targetTransform.DOScale(_actualSize, 0.1f).SetEase(Ease.Linear);
                _hasTriggered = false;
                EventsManager.TriggerWithRewardEvent(false);
            }
        }

        private IEnumerator RewardCoroutine()
        {
            yield return null;
            if (_hasTriggered) yield break;

            _hasTriggered = true;

            _rewardPanel.PopulateThePanel(200, gfx, _myCol);
        }
    }
}
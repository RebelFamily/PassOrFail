using System.Collections;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.DataRelated;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.PlayerRelated;
using Utility = Zain_Meta.Meta_Scripts.Managers.Utility;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class UnlockComponent : MonoBehaviour, IPurchase
    {
        [SerializeField] private ItemsName fileName;
        [SerializeField] private UnlockData unlockData;
        [SerializeField] private Utility utility;
        [SerializeField] private int itemPrice;
        [SerializeField] private Image priceFiller;
        [SerializeField] private GameObject fillerCanvas;
        [SerializeField] private Collider unlockCol;
        [SerializeField] private float sizeChange, originalSize;
        [SerializeField] private Text priceText;
        private IUnlocker _unlockingItem;
        private bool _isPlayerTriggering, _isUnlocked;
        private readonly YieldInstruction _delayLong = new WaitForSeconds(1f);
        private CashManager _cashManager;
        private int _curRemainingPrice;

        private ES3Settings _settings;
        private string _fileString;

        private void Awake()
        {
            fillerCanvas.SetActive(true);
            unlockData.remainingPrice = unlockData.price;
            _curRemainingPrice = unlockData.price;

            _unlockingItem = GetComponent<IUnlocker>();
            //loading...
            _fileString = "GameData/" + fileName + ".es3";
            ES3.CacheFile(_fileString);
            _settings = new ES3Settings(_fileString, ES3.Location.Cache);
            unlockData = ES3.Load(unlockData.name, unlockData, _settings);

            ReloadData();
        }


        private void Start()
        {
            _cashManager = CashManager.Instance;
        }

        private void UnlockTheCounter()
        {
            _isUnlocked = true;
            fillerCanvas.SetActive(false);
            unlockCol.enabled = false;
            _unlockingItem?.UnlockWithAnimation();
            //EventsManager.ItemUnlockedEvent(this);
            SaveTheData();
            gameObject.SetActive(false);
        }

        private IEnumerator StartTakingCashFromPlayer(PlayerCashSystem player)
        {
            if (!_cashManager) _cashManager = CashManager.Instance;
            //if (!_audioManager) _audioManager = AudioManager.Instance;

            while (_isPlayerTriggering)
            {
                if (!player.GetController().IsMoving())
                {
                    if (_cashManager.GetTotalCash() <= 0) break;

                    var amountToUse = 1 * (itemPrice / 40);

                    if (_cashManager.GetTotalCash() >= amountToUse)
                    {
                        _curRemainingPrice -= amountToUse;
                        _cashManager.RemoveCash(amountToUse);
                    }

                    else
                    {
                        _curRemainingPrice -= _cashManager.GetTotalCash();
                        _cashManager.RemoveCash(_cashManager.GetTotalCash());
                    }

                    var normalValue = Mathf.InverseLerp(itemPrice, 0, _curRemainingPrice);
                    priceFiller.fillAmount = normalValue;
                    _curRemainingPrice.SetFloatingPoint(priceText);
                    var cash = utility.SpawnCashAt(player.spawnPos);
                    cash.transform.parent = transform;
                    cash.transform.DOLocalJump(Vector3.zero, 1.25f, 1, .35f).SetEase(Ease.Linear)
                        .OnComplete(() => { LeanPool.Despawn(cash); });

                    if (_curRemainingPrice <= 0)
                    {
                        UnlockTheCounter();
                        break;
                    }

                    unlockData.remainingPrice = _curRemainingPrice;
                    SaveTheData();
                    yield return null;
                }
                else
                {
                    yield return _delayLong;
                }

                if (!_isPlayerTriggering)
                    break;
            }

            SaveTheData();
        }

        public void StopPurchasing()
        {
            _isPlayerTriggering = false;
            DOTween.Kill(fillerCanvas);
            fillerCanvas.transform.DOScale(originalSize, .15f).SetEase(Ease.InBack);
            StopCoroutine(nameof(StartTakingCashFromPlayer));
        }

        public void StartPurchasing(PlayerCashSystem cashPos)
        {
            _isPlayerTriggering = true;
            DOTween.Kill(fillerCanvas);
            fillerCanvas.transform.DOScale(sizeChange, .15f).SetEase(Ease.InBack);
            StartCoroutine(StartTakingCashFromPlayer(cashPos));
        }

        private void ReloadData()
        {
            _isUnlocked = unlockData.isUnlocked;
            _curRemainingPrice = unlockData.remainingPrice;
            itemPrice = unlockData.price;

            if (unlockData.overrideUnlock)
                _isUnlocked = true;
            if (_isUnlocked)
            {
                unlockCol.enabled = false;
                fillerCanvas.SetActive(false);

                _unlockingItem?.UnlockWithoutAnimation();
            }
            else
            {
                fillerCanvas.SetActive(true);
                var normalValue = Mathf.InverseLerp(itemPrice, 0, _curRemainingPrice);
                priceFiller.fillAmount = normalValue;
                _curRemainingPrice.SetFloatingPoint(priceText);

                _unlockingItem?.KeepItLocked();
            }
        }

        private void SaveTheData()
        {
            unlockData.isUnlocked = _isUnlocked;
            unlockData.remainingPrice = _curRemainingPrice;
            unlockData.price = itemPrice;

            ES3.Save(unlockData.name, unlockData, _settings);
            ES3.StoreCachedFile(_fileString);
        }

        public bool IsPurchased()
        {
            if (unlockData.overrideUnlock) return true;
            return _isUnlocked;
        }

        public int GetRemainingPrice() => unlockData.remainingPrice;
    }
}
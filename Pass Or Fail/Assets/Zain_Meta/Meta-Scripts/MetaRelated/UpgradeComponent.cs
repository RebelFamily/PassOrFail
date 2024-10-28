using System.Collections;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.DataRelated;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.MetaRelated.Upgrades;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class UpgradeComponent : MonoBehaviour, IPurchase
    {
        [SerializeField] private ItemsName fileName;

        [SerializeField] private UpgradeData upgradeData;

        [SerializeField] private Utility utility;
        [SerializeField] private int itemPrice;
        [SerializeField] private Image priceFiller;
        [SerializeField] private GameObject fillerCanvas;
        [SerializeField] private Collider unlockCol;
        [SerializeField] private ClassroomUpgradeProfile upgradeItem;
        [SerializeField] private float sizeChange, originalSize;
        [SerializeField] private Text priceText;
        private IUnlocker _unlockingItem;
        private bool _isPlayerTriggering, _isUpgraded;
        private readonly YieldInstruction _delayLong = new WaitForSeconds(.4f);
        private CashManager _cashManager;
        private int _curRemainingPrice;
        private int _upgradeIndex;

        private string _fileString;

        private void Awake()
        {
            fillerCanvas.SetActive(true);
            _unlockingItem = GetComponent<IUnlocker>();
            _fileString = "GameData/Upgrades/" + fileName + ".es3";
            upgradeData = ES3.Load(upgradeData.saveKey, _fileString, upgradeData);

            ReloadData();
        }


        private void Start()
        {
            _cashManager = CashManager.Instance;
        }

        private void UpgradeTheRoom()
        {
            _isPlayerTriggering = false;
            print(_upgradeIndex);
            _upgradeIndex++;
            print(_upgradeIndex);
            fillerCanvas.SetActive(false);
            unlockCol.enabled = false;
            _unlockingItem?.UnlockWithAnimation();
            SaveAfterUpgrade();
            if (_upgradeIndex >= upgradeData.pricing.Length)
            {
                _isUpgraded = true;
                gameObject.SetActive(false);
            }
        }

        private IEnumerator StartTakingCashFromPlayer(PlayerCashSystem player)
        {
            if (!_cashManager) _cashManager = CashManager.Instance;
            //if (!_audioManager) _audioManager = AudioManager.Instance;

            if (!_isPlayerTriggering)
                yield break;
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

                    upgradeData.pricing[_upgradeIndex].remainingPrice = _curRemainingPrice;
                    SaveTheData();
                    if (_curRemainingPrice <= 0)
                    {
                        UpgradeTheRoom();
                        break;
                    }

                    yield return null;
                }
                else
                {
                    yield return _delayLong;
                }
            }
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
            _upgradeIndex = upgradeData.upgradedLevel;
            _isUpgraded = upgradeData.isUpgraded;
            _curRemainingPrice = upgradeData.pricing[_upgradeIndex].remainingPrice;
            itemPrice = upgradeData.pricing[_upgradeIndex].pricesForEachUpgrade;

            upgradeItem.ReloadTheUpgrade(_upgradeIndex, upgradeData.upgradeIndex);

            if (_isUpgraded)
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
            upgradeData.isUpgraded = _isUpgraded;
            upgradeData.pricing[_upgradeIndex].remainingPrice = _curRemainingPrice;
            ES3.Save(upgradeData.saveKey, upgradeData, _fileString);
        }

        private void SaveAfterUpgrade()
        {
            upgradeData.isUpgraded = _isUpgraded;
            upgradeData.upgradedLevel = _upgradeIndex;
            ES3.Save(upgradeData.saveKey, upgradeData, _fileString);
        }

        public bool IsPurchased()
        {
            return _isUpgraded;
        }

        public int GetRemainingPrice() => upgradeData.pricing[_upgradeIndex].remainingPrice;

        [ContextMenu("Reset The Upgrade Area")]
        public void ResetTheUpgrade()
        {
            if (_upgradeIndex >= upgradeData.pricing.Length)
            {
                _isUpgraded = true;
                gameObject.SetActive(false);
            }

            fillerCanvas.SetActive(true);
            unlockCol.enabled = true;
            _curRemainingPrice = upgradeData.pricing[_upgradeIndex].remainingPrice;
            itemPrice = upgradeData.pricing[_upgradeIndex].pricesForEachUpgrade;
            var normalValue = Mathf.InverseLerp(itemPrice, 0, _curRemainingPrice);
            priceFiller.fillAmount = normalValue;
            _curRemainingPrice.SetFloatingPoint(priceText);
        }
    }
}
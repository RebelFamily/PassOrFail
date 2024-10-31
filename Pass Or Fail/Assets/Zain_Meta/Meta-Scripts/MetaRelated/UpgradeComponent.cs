using System.Collections;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;
using Zain_Meta.Meta_Scripts.DataRelated;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class UpgradeComponent : IPurchase
    {
        [SerializeField] private ItemsName fileName;
        [SerializeField] private UpgradeData upgradeData;
        [SerializeField] private float jumpPower, jumpDelay;
        [SerializeField] private Ease jumpEase;
        [SerializeField] private Utility utility;
        [SerializeField] private int itemPrice;
        [SerializeField] private Image priceFiller;
        [SerializeField] private GameObject fillerCanvas;
        [SerializeField] private Collider unlockCol;
        [SerializeField] private float sizeChange, originalSize;
        [SerializeField] private Text priceText;
        private IUnlocker _unlockingItem;
        private bool _isPlayerTriggering, _isUpgraded;
        private bool _isPurchasedForNow;
        private readonly YieldInstruction _delayLong = new WaitForSeconds(.2f);
        private readonly YieldInstruction _delayCash = new WaitForSeconds(0.1f);
        private CashManager _cashManager;
        private int _curRemainingPrice;
        private int _upgradeLevel;

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
            _isPurchasedForNow = true;
            _isPlayerTriggering = false;
            _upgradeLevel++;
            fillerCanvas.SetActive(false);
            unlockCol.enabled = false;
            SaveAfterUpgrade();
            _unlockingItem?.UnlockWithAnimation();
            EventsManager.ItemUnlockedEvent(this);
            
            if (_upgradeLevel >= upgradeData.pricing.Length)
            {
                _isUpgraded = true;
                SaveAfterUpgrade();
                gameObject.SetActive(false);
            }
        }

        private IEnumerator StartTakingCashFromPlayer(PlayerCashSystem player)
        {
            if (!_cashManager) _cashManager = CashManager.Instance;

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
                    var cash = utility.SpawnGivingCashAt(player.spawnPos);
                    cash.transform.parent = transform;
                    cash.transform.ParabolicMovement(transform, jumpDelay, jumpPower, jumpEase,
                        () => { LeanPool.Despawn(cash); });
                    upgradeData.pricing[_upgradeLevel].remainingPrice = _curRemainingPrice;
                    SaveTheData();
                    if (_curRemainingPrice <= 0)
                    {
                        UpgradeTheRoom();
                        break;
                    }

                    yield return _delayCash;
                }
                else
                {
                    yield return _delayLong;
                }
            }
        }

        public override void StopPurchasing()
        {
            _isPlayerTriggering = false;
            DOTween.Kill(fillerCanvas);
            fillerCanvas.transform.DOScale(originalSize, .15f).SetEase(Ease.InBack);
            StopCoroutine(nameof(StartTakingCashFromPlayer));
        }

        public override void StartPurchasing(PlayerCashSystem cashPos)
        {
            _isPlayerTriggering = true;
            DOTween.Kill(fillerCanvas);
            fillerCanvas.transform.DOScale(sizeChange, .15f).SetEase(Ease.InBack);
            StartCoroutine(StartTakingCashFromPlayer(cashPos));
        }

        private void ReloadData()
        {
            _upgradeLevel = upgradeData.upgradedLevel;
            _isUpgraded = upgradeData.isUpgraded;
            if (_isUpgraded)
            {
                unlockCol.enabled = false;
                fillerCanvas.SetActive(false);
                _unlockingItem?.UnlockWithoutAnimation();
                return;
            }
           
            _curRemainingPrice = upgradeData.pricing[_upgradeLevel].remainingPrice;
            itemPrice = upgradeData.pricing[_upgradeLevel].pricesForEachUpgrade;
            fillerCanvas.SetActive(true);
            var normalValue = Mathf.InverseLerp(itemPrice, 0, _curRemainingPrice);
            priceFiller.fillAmount = normalValue;
            _curRemainingPrice.SetFloatingPoint(priceText);
            _unlockingItem?.KeepItLocked();
            
        }

        private void SaveTheData()
        {
            upgradeData.isUpgraded = _isUpgraded;
            upgradeData.pricing[_upgradeLevel].remainingPrice = _curRemainingPrice;
            ES3.Save(upgradeData.saveKey, upgradeData, _fileString);
        }

        private void SaveAfterUpgrade()
        {
            upgradeData.isUpgraded = _isUpgraded;
            upgradeData.upgradedLevel = _upgradeLevel;
            ES3.Save(upgradeData.saveKey, upgradeData, _fileString);
        }

        public override bool IsPurchased()
        {
            return  _isPurchasedForNow;
        }

        public override void EnableMe(bool toEnable)
        {
            gameObject.SetActive(toEnable);
            ResetTheUpgrade();
        }
        
        public override int GetRemainingPrice() => upgradeData.pricing[_upgradeLevel].remainingPrice;

        [ContextMenu("Reset The Upgrade Area")]
        public void ResetTheUpgrade()
        {
            if (_upgradeLevel >= upgradeData.pricing.Length)
            {
                _isUpgraded = true;
                fillerCanvas.SetActive(false);
                unlockCol.enabled = false;
                gameObject.SetActive(false);
                return;
            }

            fillerCanvas.SetActive(true);
            unlockCol.enabled = true;
            _curRemainingPrice = upgradeData.pricing[_upgradeLevel].remainingPrice;
            itemPrice = upgradeData.pricing[_upgradeLevel].pricesForEachUpgrade;
            var normalValue = Mathf.InverseLerp(itemPrice, 0, _curRemainingPrice);
            priceFiller.fillAmount = normalValue;
            _curRemainingPrice.SetFloatingPoint(priceText);
        }
    }
}
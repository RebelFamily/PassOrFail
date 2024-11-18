using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.MetaRelated;
using Zain_Meta.Meta_Scripts.MetaRelated.Upgrades;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class UnlockNavigationManager : MonoBehaviour
    {
        [SerializeField] private IPurchase[] purchases;
        [SerializeField] private int curUnlockIndex;
        [SerializeField] private CameraSwitcher switcher;

        private bool _toNavigateAfterUpgrade;

        public void HideEverything()
        {
            for (var i = 0; i < purchases.Length; i++)
            {
                purchases[i].Hide();
            }
            curUnlockIndex = PlayerPrefs.GetInt("CurUnlockingIndex", 0);
        }

        public void ReloadThePurchasesData()
        {
            curUnlockIndex = PlayerPrefs.GetInt("CurUnlockingIndex", 0);
            for (var i = 0; i < purchases.Length; i++)
            {
                purchases[i].Hide();
            }

            if (OnBoardingManager.TutorialComplete)
            {
                for (var i = 0; i < curUnlockIndex; i++)
                {
                    purchases[i].EnableMe(true, false);
                }

                purchases[curUnlockIndex].EnableMe(true, true);
            }
        }

        private void OnEnable()
        {
            EventsManager.OnItemUnlocked += ShowNextItemToUnlock;
            EventsManager.OnClassReadyToUpgrade += ShowNextItemToUnlock;
        }

        private void OnDisable()
        {
            EventsManager.OnItemUnlocked -= ShowNextItemToUnlock;
            EventsManager.OnClassReadyToUpgrade -= ShowNextItemToUnlock;
        }

        private void ShowNextItemToUnlock(ClassroomUpgradeProfile arg1, bool isUpgrade)
        {
            if (isUpgrade) return;
            if (!_toNavigateAfterUpgrade) return;

            _toNavigateAfterUpgrade = false;
            LookToNextTarget(.5f);
        }

        private void ShowNextItemToUnlock(IPurchase purchase)
        {
            if (purchase != purchases[curUnlockIndex]) return;

            _toNavigateAfterUpgrade = false;
            var isUpgrade = purchase.isUpgrade;
            if (isUpgrade)
                _toNavigateAfterUpgrade = true;
            curUnlockIndex++;
            if (curUnlockIndex >= purchases.Length)
            {
                curUnlockIndex = purchases.Length - 1;
            }

            PlayerPrefs.SetInt("CurUnlockingIndex", curUnlockIndex);
            if (isUpgrade) return;
            LookToNextTarget();
        }

        public void LookAtNextUnlock()
        {
            LookToNextTarget();
        }

        private void GoForNextUnlocker()
        {
            purchases[curUnlockIndex].EnableMe(true, true);
        }

        private void LookToNextTarget()
        {
            DOVirtual.DelayedCall(3f, () =>
            {
                switcher.ZoomToTarget(purchases[curUnlockIndex].transform,true);
                DOVirtual.DelayedCall(.5f, GoForNextUnlocker);
            });
        }

        private void LookToNextTarget(float delay)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                switcher.ZoomToTarget(purchases[curUnlockIndex].transform,true);
                DOVirtual.DelayedCall(.5f, GoForNextUnlocker);
            });
        }
    }
}
using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.MetaRelated;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class UnlockNavigationManager : MonoBehaviour
    {
        [SerializeField] private IPurchase[] purchases;
        [SerializeField] private int curUnlockIndex;
        [SerializeField] private CameraSwitcher switcher;

        public void HideEverything()
        {
            for (var i = 0; i < purchases.Length; i++)
            {
                purchases[i].Hide();
            }
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
                    purchases[i].EnableMe(true,false);
                }

                purchases[curUnlockIndex].EnableMe(true,true);
            }
        }

        private void OnEnable()
        {
            EventsManager.OnItemUnlocked += ShowNextItemToUnlock;
        }

        private void OnDisable()
        {
            EventsManager.OnItemUnlocked -= ShowNextItemToUnlock;
        }

        private void ShowNextItemToUnlock(IPurchase purchase)
        {
            if (purchase != purchases[curUnlockIndex]) return;

            curUnlockIndex++;
            if (curUnlockIndex >= purchases.Length)
            {
                curUnlockIndex = purchases.Length - 1;
            }

            PlayerPrefs.SetInt("CurUnlockingIndex", curUnlockIndex);
            LookToNextTarget();
        }

        public void LookAtNextUnlock()
        {
            LookToNextTarget();
        }

        private void GoForNextUnlocker()
        {
            purchases[curUnlockIndex].EnableMe(true,true);
        }

        private void LookToNextTarget()
        {
            print("Switcher");
            DOVirtual.DelayedCall(3f, () =>
            {
                switcher.ZoomToTarget(purchases[curUnlockIndex].transform);
                DOVirtual.DelayedCall(.5f, GoForNextUnlocker);
            });
        }
    }
}
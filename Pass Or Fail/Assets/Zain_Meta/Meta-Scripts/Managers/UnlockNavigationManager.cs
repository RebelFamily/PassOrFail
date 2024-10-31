using UnityEngine;
using Zain_Meta.Meta_Scripts.MetaRelated;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class UnlockNavigationManager : MonoBehaviour
    {
        [SerializeField] private IPurchase[] purchases;
        [SerializeField] private int curUnlockIndex;
        

        public void HideEverything()
        {
            for (var i = 0; i < purchases.Length; i++)
            {
                purchases[i].EnableMe(false);
            }
        }
        public void ReloadThePurchasesData()
        {
            curUnlockIndex = PlayerPrefs.GetInt("CurUnlockingIndex", 0);
            for (var i = 0; i < purchases.Length; i++)
            {
                purchases[i].EnableMe(false);
            }

            if (OnBoardingManager.TutorialComplete)
            {
                for (var i = 0; i <= curUnlockIndex; i++)
                {
                    purchases[i].EnableMe(true);
                }

                purchases[curUnlockIndex].EnableMe(true);
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
            GoForNextUnlocker();
        }

        private void GoForNextUnlocker()
        {
            purchases[curUnlockIndex].EnableMe(true);
        }
    }
}
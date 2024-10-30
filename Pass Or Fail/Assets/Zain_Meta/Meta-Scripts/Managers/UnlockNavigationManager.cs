using System;
using UnityEngine;
using Zain_Meta.Meta_Scripts.MetaRelated;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class UnlockNavigationManager : MonoBehaviour
    {
        [SerializeField] private IPurchase[] purchases;
        [SerializeField] private int curUnlockIndex;

        public void ReloadThePurchasesData()
        {
            curUnlockIndex = PlayerPrefs.GetInt("CurUnlockingIndex", 0);
            for (var i = 0; i < purchases.Length; i++)
            {
                purchases[i].gameObject.SetActive(false);
            }

            if (OnBoardingManager.TutorialComplete)
            {
                for (var i = 0; i <= curUnlockIndex; i++)
                {
                    purchases[i].gameObject.SetActive(true);
                }

                purchases[curUnlockIndex].gameObject.SetActive(true);
            }
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }
    }
}
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class CashManager : MonoBehaviour
    {
          #region Initials

        public static CashManager Instance;

        private void Awake()
        {
            Instance = this;
            totalCash = PlayerPrefs.GetInt("TotalCash", totalCash);
            totalCash.SetFloatingPoint(cashText);
        }

        #endregion
        
        [SerializeField] private int totalCash;
        [SerializeField] private int totalCoins;
        [SerializeField] private GameObject couponsBar;
        public RectTransform cashIcon;
        [SerializeField] private Text cashText;

        

        public void RemoveCash(int amount)
        {
            totalCash -= amount;
            if (totalCash <= 0)
                totalCash = 0;
            totalCash.SetFloatingPoint(cashText);
            PlayerPrefs.SetInt("TotalCash", totalCash);
           // EventsManager.CashUpdatedEvent();
        }
        


        public void AddCash(int amount)
        {
            totalCash += amount;
            totalCash.SetFloatingPoint(cashText);
            PlayerPrefs.SetInt("TotalCash", totalCash);
           // EventsManager.CashUpdatedEvent();
            DOTween.Kill(cashIcon);
            cashIcon.localScale = Vector3.one;
            cashIcon.DOScale(1.25f, .15f).SetEase(Ease.InBack).SetLoops(2, LoopType.Yoyo);
        }
        
        

        public int GetTotalCash() => totalCash;
        public int GetTotalCoins() => totalCoins;
    }
}
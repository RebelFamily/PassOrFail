using GameAnalyticsSDK;
using UnityEngine;
namespace Zain_Meta.Meta_Scripts.DataRelated
{
    public class DataAdjuster : MonoBehaviour
    {
        [SerializeField] private SaveClass[] allSaveClasses;
        private void Awake()
        {
            Application.targetFrameRate = 90;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, PlayerPrefsHandler.Meta);
            AdsCaller.Instance.ShowBanner();
            AdsCaller.Instance.HideRectBanner();
        }
        private void OnDisable()
        {
            ResetAllData();
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, PlayerPrefsHandler.Meta);
        }
        private void ResetAllData()
        {
            foreach (var t in allSaveClasses)
            {
                t.ClearData();
            }
        }
    }
}
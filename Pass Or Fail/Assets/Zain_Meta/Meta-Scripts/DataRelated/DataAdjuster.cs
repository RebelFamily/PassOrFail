using UnityEngine;

namespace Zain_Meta.Meta_Scripts.DataRelated
{
    public class DataAdjuster : MonoBehaviour
    {
        [SerializeField] private SaveClass[] allSaveClasses;
        private void OnDisable()
        {
            ResetAllData();
        }

        private void ResetAllData()
        {
            for (var i = 0; i < allSaveClasses.Length; i++)
            {
                allSaveClasses[i].ClearData();
            }
        }
    }
}
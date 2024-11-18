using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;
namespace Zain_Meta.Meta_Scripts.Panel
{
    public class SwitchingPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup panelAttached;
        private void OnEnable()
        {
            EventsManager.OnSwitchTheCamera += SetPanelVisibility;
            SharedUI.Instance.CloseSpecialMenu(PlayerPrefsHandler.CurrencyCounter);
        }
        private void OnDisable()
        {
            EventsManager.OnSwitchTheCamera -= SetPanelVisibility;
        }
        private void SetPanelVisibility(bool obj)
        {
            if (obj)
                panelAttached.HideCanvas();
            else
                panelAttached.ShowCanvas();
        }
    }
}
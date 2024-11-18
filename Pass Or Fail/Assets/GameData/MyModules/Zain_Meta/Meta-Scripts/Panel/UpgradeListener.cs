using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.MetaRelated.Upgrades;

namespace Zain_Meta.Meta_Scripts.Panel
{
    public class UpgradeListener : MonoBehaviour
    {
        [SerializeField] private CanvasGroup panelAttached;

        private void OnEnable()
        {
            EventsManager.OnClassReadyToUpgrade += SetPanelVisibility;
        }

        private void OnDisable()
        {
            EventsManager.OnClassReadyToUpgrade -= SetPanelVisibility;
        }

        private void SetPanelVisibility(ClassroomUpgradeProfile classroom, bool toHide)
        {
            if (toHide)
                panelAttached.HideCanvas();
            else
                panelAttached.ShowCanvas();
        }
    }
}
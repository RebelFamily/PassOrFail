using UnityEngine;
using UnityEngine.UI;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class RideIconUpdater : MonoBehaviour
    {
        [SerializeField] private Sprite uniCycle, skateboard;
        [SerializeField] private Image imageToChange;

        private void Start()
        {
            var rideIndex = PlayerPrefs.GetInt("RideType", 0);
            if (rideIndex == 2)
                imageToChange.sprite = uniCycle;
            else if (rideIndex == 1)
                imageToChange.sprite = skateboard;
        }

        private void OnEnable()
        {
            Callbacks.OnRewardARide += UpdateTheIcon;
        }

        private void OnDisable()
        {
            Callbacks.OnRewardARide -= UpdateTheIcon;
        }

        private void UpdateTheIcon(Callbacks.RewardType rideType)
        {
            if (rideType == Callbacks.RewardType.RewardUniCycle)
                imageToChange.sprite = uniCycle;
            else if (rideType == Callbacks.RewardType.RewardSkateboard)
                imageToChange.sprite = skateboard;
        }
    }
}
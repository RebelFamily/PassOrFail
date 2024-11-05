using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.PlayerRelated.Rides;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public class RideEnabler : MonoBehaviour
    {
        [SerializeField] private CycleRide cycleRide;
        [SerializeField] private BoardRide skateboard;
        [SerializeField] private GameObject walkingVfx;
        private IRideable _curRide;
        private ArcadeMovement.PlayerState _curState;

        
        private void OnEnable()
        {
            EventsManager.OnTriggerTeaching += SetRideDuringTrigger;
        }

        private void OnDisable()
        {
            EventsManager.OnTriggerTeaching -= SetRideDuringTrigger;
        }

        private void SetRideDuringTrigger(bool val, Vector3 arg2, Vector3 arg3, ClassroomProfile arg4)
        {
            ResizeBasedOnState(_curState, !val);
        }



        private void ResizeBasedOnState(ArcadeMovement.PlayerState state, bool toScale)
        {
            switch (state)
            {
                case ArcadeMovement.PlayerState.RidingSkateboard:
                    skateboard.transform.DOScale(toScale ? 1 : 0, .25f).SetEase(Ease.Linear);
                    break;
                case ArcadeMovement.PlayerState.RidingOneWheelCycle:
                    cycleRide.transform.DOScale(toScale ? 1 : 0, .25f).SetEase(Ease.Linear);
                    break;
            }
        }

        public void EnableTheRide(ArcadeMovement.PlayerState curState)
        {
            _curState = curState;
            walkingVfx.SetActive(false);
            cycleRide.gameObject.SetActive(false);
            skateboard.gameObject.SetActive(false);
            switch (_curState)
            {
                case ArcadeMovement.PlayerState.ByFoot:
                    _curRide = null;
                    walkingVfx.SetActive(true);
                    break;
                case ArcadeMovement.PlayerState.RidingSkateboard:
                    _curRide = skateboard;
                    skateboard.gameObject.SetActive(true);
                    break;
                case ArcadeMovement.PlayerState.RidingOneWheelCycle:
                    _curRide = cycleRide;
                    cycleRide.gameObject.SetActive(true);
                    break;
            }
        }

        public IRideable GetCurrentRide()
        {
            return _curRide;
        }
    }
}
using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.DataRelated;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    [RequireComponent(typeof(PlayerAnimator))]
    public class ArcadeMovement : MonoBehaviour
    {
        public enum PlayerState
        {
            ByFoot, //0
            RidingSkateboard, //1
            RidingOneWheelCycle, //2
        }

        [SerializeField] private PlayerState curPlayerWalkingState;
        [SerializeField] private PlayerState defaultStateOfPlayer;
        [SerializeField] private PlayerSpeedData playerSpeedData;
        [SerializeField] private CharacterController controller;
        [SerializeField] private FloatingJoystick joystick;
        [SerializeField] private RideEnabler rideEnabler;
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float turnSpeed;
        private IRideable _myCurRide;
        private float _curMoveSpeed, _curRotSpeed;
        private Vector3 _input;
        private bool _stop;


        private void OnEnable()
        {
            EventsManager.OnTriggerTeaching += SnapToThisPos;
            EventsManager.OnSwitchTheCamera += AdjustTheMovement;
            EventsManager.OnSnapPlayer += SnapThePlayer;
            Callbacks.OnRewardARide += RewardMeARide;
            EventsManager.OnBackToFoot += ResetTheState;
            EventsManager.OnInterPopupShown += AdjustTheMovement;
        }

        private void OnDisable()
        {
            EventsManager.OnTriggerTeaching -= SnapToThisPos;
            EventsManager.OnSwitchTheCamera -= AdjustTheMovement;
            EventsManager.OnSnapPlayer -= SnapThePlayer;
            Callbacks.OnRewardARide -= RewardMeARide;
            EventsManager.OnBackToFoot -= ResetTheState;
            EventsManager.OnInterPopupShown -= AdjustTheMovement;
        }

        private void ResetTheState()
        {
            curPlayerWalkingState = defaultStateOfPlayer;
            PlayerPrefs.SetInt("RideType", (int)curPlayerWalkingState);
            SetValuesBasedOnState();
        }

        private void RewardMeARide(Callbacks.RewardType rideType)
        {
            if (rideType != Callbacks.RewardType.RewardUniCycle &&
                rideType != Callbacks.RewardType.RewardSkateboard) return;

            curPlayerWalkingState = rideType switch
            {
                Callbacks.RewardType.RewardUniCycle => PlayerState.RidingOneWheelCycle,
                Callbacks.RewardType.RewardSkateboard => PlayerState.RidingSkateboard,
                _ => curPlayerWalkingState
            };
            PlayerPrefs.SetInt("RideType", (int)curPlayerWalkingState);
            SetValuesBasedOnState();
        }


        private void SnapThePlayer(Transform posToSnapAt)
        {
            StopMovement();
            transform.DORotateQuaternion(posToSnapAt.rotation, .15f).SetEase(Ease.Linear);
            var position = posToSnapAt.position;
            transform.DOMoveX(position.x, .15f).SetEase(Ease.Linear);
            transform.DOMoveZ(position.z, .15f).SetEase(Ease.Linear).OnComplete(() => { ResumeMovement(); });
        }

        private void AdjustTheMovement(bool toSwitch)
        {
            if (toSwitch)
            {
                StopMovement();
            }
            else
            {
                ResumeMovement();
            }
        }

        private void SnapToThisPos(bool startTeaching, Vector3 teachingPos,
            Vector3 rotation, ClassroomProfile classroomProfile)
        {
            if (!startTeaching)
            {
                ResumeMovement();
                return;
            }
            
            StopMovement();
            transform.DOMove(teachingPos, .15f);
            transform.DORotate(rotation, .15f).OnComplete(() =>
            {
                playerAnimator.PlayTeachingAnim();
            });
        }

        private void Start()
        {
            /*_curMoveSpeed = moveSpeed;
            _curRotSpeed = turnSpeed;*/
            AdjustRideData();
        }

        private void AdjustRideData()
        {
            var rideIndex = PlayerPrefs.GetInt("RideType", 0);
            var normalRide = PlayerPrefs.GetInt("DefaultState", (int)defaultStateOfPlayer);
            defaultStateOfPlayer = normalRide switch
            {
                0 => PlayerState.ByFoot,
                3 => PlayerState.RidingOneWheelCycle,
                _ => defaultStateOfPlayer
            };

            curPlayerWalkingState = rideIndex switch
            {
                0 => defaultStateOfPlayer,
                1 => PlayerState.RidingSkateboard,
                2 => PlayerState.RidingOneWheelCycle,
                _ => defaultStateOfPlayer
            };
            SetValuesBasedOnState();
        }

        private void SetValuesBasedOnState()
        {
            playerAnimator.SetRideAnimations(curPlayerWalkingState);
            rideEnabler.EnableTheRide(curPlayerWalkingState);
            switch (curPlayerWalkingState)
            {
                case PlayerState.ByFoot:
                    moveSpeed = playerSpeedData.moveSpeedNormal;
                    turnSpeed = playerSpeedData.turnSpeedNormal;
                    break;
                case PlayerState.RidingSkateboard:
                    moveSpeed = playerSpeedData.moveSpeedBoard;
                    turnSpeed = playerSpeedData.turnSpeedBoard;
                    break;
                case PlayerState.RidingOneWheelCycle:
                    moveSpeed = playerSpeedData.moveSpeedCycle;
                    turnSpeed = playerSpeedData.turnSpeedCycle;
                    break;
            }
            _myCurRide = rideEnabler.GetCurrentRide();
            _curMoveSpeed = moveSpeed;
            _curRotSpeed = turnSpeed;
        }

        private void SetMovement()
        {
            GatherInputs();
            Look();
        }

        private void Update()
        {
            if (_stop || controller.enabled == false) return;
            SetMovement();
            Move();
            _myCurRide?.Move(_input.x, _input, _curMoveSpeed);
        }


        private void Look()
        {
            if (_input == Vector3.zero) return;
            var rot = Quaternion.LookRotation(_input, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _curRotSpeed * Time.deltaTime);
        }

        private void Move()
        {
            if (controller.enabled)
                controller.Move(transform.forward * (_input.magnitude * _curMoveSpeed * Time.deltaTime));
            var transform1 = transform;
            var transformLocalPosition = transform1.localPosition;
            transformLocalPosition.y = 0;
            transform1.localPosition = transformLocalPosition;
        }


        private void GatherInputs()
        {
            if (!joystick) return;
            if (_stop)
            {
                _input = Vector3.zero;
                joystick.ResetInput();
                playerAnimator.SetAnimations(_input.magnitude);
            }

            var x = joystick.Horizontal;
            var z = joystick.Vertical;
            _input = new Vector3(x, 0f, z);
            playerAnimator.SetAnimations(_input.magnitude);
        }

        public bool IsMoving() => _input.magnitude > 0f;

        private void StopMovement()
        {
            _stop = true;
            _curRotSpeed = _curMoveSpeed = 0;
            // controller.enabled = false;
            _input = Vector3.zero;
            playerAnimator.SetAnimations(0);
            joystick.ResetInput();
        }

        private void ResumeMovement()
        {
            _stop = false;
            //controller.enabled = true;
            _curMoveSpeed = moveSpeed;
            _curRotSpeed = turnSpeed;
        }
        
    }
}
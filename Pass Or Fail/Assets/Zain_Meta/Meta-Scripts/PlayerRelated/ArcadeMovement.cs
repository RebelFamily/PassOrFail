using UnityEngine;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
   [RequireComponent(typeof(PlayerAnimator))]
   public class ArcadeMovement : MonoBehaviour
   {
      [SerializeField] private CharacterController controller;
      [SerializeField] private PlayerAnimator playerAnimator;
      [SerializeField] private float moveSpeed;
      [SerializeField] private float turnSpeed;

      private Vector3 _input;
      private bool _stop;
   
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
      }


      private void Look()
      {
         if (_input == Vector3.zero) return;
         var rot = Quaternion.LookRotation(_input, Vector3.up);
         transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
      }

      private void Move()
      {
         if (controller.enabled)
            controller.Move(transform.forward * (_input.magnitude * moveSpeed * Time.deltaTime));
         var transform1 = transform;
         var transformLocalPosition = transform1.localPosition;
         transformLocalPosition.y = 0;
         transform1.localPosition = transformLocalPosition;
      }


      private void GatherInputs()
      {
         //if (!joystick) return;
         var x = Input.GetAxis("Horizontal");
         var z =  Input.GetAxis("Vertical");
         _input = new Vector3(x, 0f, z);
         playerAnimator.SetAnimations(_input.magnitude);
      }
   }
}

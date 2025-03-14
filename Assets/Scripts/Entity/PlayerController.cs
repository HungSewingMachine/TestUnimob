using System;
using Interface;
using UnityEngine;

namespace Entity
{
    public class PlayerController : MonoBehaviour, ICharacter
    {
        public float moveSpeed = 5f; // Movement Speed
        public float acceleration = 10f; // How fast it reaches target speed
        public float deceleration = 15f; // How fast it slows down
        public float rotationSpeed = 10f; // Rotation smoothing speed

        [SerializeField] private int counter = 0;
        
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Joystick joystick;
        [SerializeField] private Transform modelTransform;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
    
        private Vector3 velocity;
    
        private static readonly int IsMoving = Animator.StringToHash("IsMove");
        private static readonly int IsEmpty = Animator.StringToHash("IsEmpty");

        private void Start()
        {
            animator.SetBool(IsEmpty, true);
        }

        // Update is called once per frame
        private void Update()
        {
            var joystickInput = joystick.Direction;
            var inputDirection = new Vector3(joystickInput.x, 0, joystickInput.y).normalized;
            var moveDirection = ConvertJoystickToWorldDirection(joystickInput);
            var isMoving = inputDirection.sqrMagnitude > 0.1f;
            if (isMoving)
            {
                velocity.x = Mathf.Lerp(velocity.x, moveDirection.x * moveSpeed, acceleration * Time.deltaTime);
                velocity.z = Mathf.Lerp(velocity.z, moveDirection.z * moveSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                velocity.x = Mathf.Lerp(velocity.x, 0, deceleration * Time.deltaTime);
                velocity.z = Mathf.Lerp(velocity.z, 0, deceleration * Time.deltaTime);
            }
        
            animator.SetBool(IsMoving, isMoving);
            characterController.Move(velocity * Time.deltaTime);
        
            // Rotate character towards movement direction
            if (isMoving)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                counter = 0;
            }
        }
    
        private Vector3 ConvertJoystickToWorldDirection(Vector2 input)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Flatten Y axis (ignore camera tilt)
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            // Convert joystick input into world space movement
            return forward * input.y + right * input.x;
        }

        public bool CanGive() => counter > 0;
        
        public bool CanCarry() => counter < 5;

        public void TakeFruits()
        {
            counter += 1;
        }

        public int RemoveFruits()
        {
            return counter -= 1;
        }
    }
}

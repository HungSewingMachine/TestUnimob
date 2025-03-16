using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Entity
{
    public class PlayerController : Character
    {
        [SerializeField] private Joystick joystick;
        [SerializeField] private Transform cameraTransform;
        
        protected override Vector3 ProcessInput()
        {
            var joystickInput = joystick.Direction;
            return ConvertJoystickToWorldDirection(joystickInput);
        }

        protected override int MaxCapacity()
        {
            return 5;
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

        public void CollectMoney()
        {
            
        }
        
        public void SpendMoney()
        {
            
        }
    }
}

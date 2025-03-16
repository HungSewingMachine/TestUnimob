using Data;
using Interface;
using UnityEngine;

namespace Entity
{
    public class PlayerController : Character, IBuyer
    {
        [SerializeField] private Joystick joystick;
        [SerializeField] private Transform cameraTransform;

        protected override void Start()
        {
            base.Start();

            playerData.numberOfCash = 0;
        }

        protected override Vector3 ProcessInput()
        {
            var joystickInput = joystick.Direction;
            return ConvertJoystickToWorldDirection(joystickInput);
        }

        public override int MaxCapacity()
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

        public bool CanBuy()
        {
            return playerData.numberOfCash > 0;
        }

        [SerializeField] protected PlayerData playerData;
        
        public void TakeCash(ITransfer transfer)
        {
            transfer.MoveTo(modelTransform, new Vector3(0, 0.5f, 0), true);
            playerData.AddMoney(1);
        }

        public void SpendCash()
        {
            playerData.AddMoney(-1);;
        }
    }
}

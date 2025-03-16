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

        [SerializeField] private Box boxPrefab;
        
        [Button]
        public void Test()
        {
            var box = Instantiate(boxPrefab);
            FillBox(box);
        }
        
        public async UniTaskVoid FillBox(Box box)
        {
            if (fruits.Count == 0) return;
            
            var boxTransform = box.transform;
            var counter = 0;
            while (fruits.Count > 0)
            {
                var f = fruits.Pop();
                f.MoveTo(boxTransform, box.GetFruitPosition(counter));
                counter++;
            }
            
            box.PlayAnimation();
            await UniTask.Delay(1000);
            var localPosition = new Vector3(0, 0.8f, 1f);
            box.MoveTo(modelTransform, localPosition);
            hasBox = true;
            
            GiveCash().Forget();
        }

        [SerializeField] private Cash cashPrefab;
        
        [Button]
        public async UniTaskVoid GiveCash()
        {
            var cashier = FindObjectOfType<Cashier>();
            
            for (int i = 0; i < 12; i++)
            {
                var cash = Instantiate(cashPrefab, modelTransform.position, Quaternion.identity);
                cash.MoveTo(cashier.transform, cashier.GetCashPosition(i));
                cashier.StoreCash(cash);
                await UniTask.Delay(100);
            }
        }

        public void CollectMoney()
        {
            
        }
        
        public void SpendMoney()
        {
            
        }
    }
}

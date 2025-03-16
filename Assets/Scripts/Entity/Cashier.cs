using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Cashier : InteractBase
    {
        [SerializeField] private Transform myTransform;
        
        private Queue<BotController> objectQueue = new Queue<BotController>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ProcessQueue();
            }
        }

        public void Enqueue(BotController botController)
        {
            var positionIndex = objectQueue.Count;
            botController.SetTarget(GetQueuePosition(positionIndex));
            objectQueue.Enqueue(botController);
        }

        private Vector3 GetQueuePosition(int index)
        {
            return myTransform.position + new Vector3(index * 1.5f, 0, 1);
        }
        
        void ProcessQueue()
        {
            if (objectQueue.Count == 0) return;

            var bot = objectQueue.Dequeue(); // Lấy phần tử đầu tiên (O(1))

            bot.SetTarget(Vector3.zero);

            int index = 0;
            foreach (var obj in objectQueue)
            {
                var pos = GetQueuePosition(index);
                obj.SetTarget(pos);
                index++;
            }
        }

        [SerializeField] private Box boxPrefab;
        private Box wrapBox;

        private Stack<Cash> cashes = new Stack<Cash>();
        
        public void StoreCash(Cash cash)
        {
            cashes.Push(cash);
        }
        
        public Vector3 GetCashPosition(int index)
        {
            var heightIndex = index / 8;
            var colIndex = index % 4;
            var rowIndex = (index / 4) % 2;
            return new Vector3( -1 - colIndex * 0.25f, 0.5f + 0.1f * heightIndex, -0.38f + rowIndex * 0.52f);
        }

        protected override float GetCooldownTime()
        {
            return 0.05f;
        }
        
        protected override bool CanInteract()
        {
            return interactionCounter <= 0f && cashes.Count > 0;
        }
        
        protected override void Interact()
        {
            var cash = cashes.Pop();
            character.TakeCash(cash);
        }
    }
}

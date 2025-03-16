using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Entity
{
    public enum CashierState
    {
        ProcessQueue,
        Processing,
    }
    
    public class Cashier : InteractBase
    {
        [SerializeField] private CashierState state = CashierState.ProcessQueue;
        [SerializeField] private Transform myTransform;
        
        private Queue<BotController> objectQueue = new Queue<BotController>();

        /// <summary>
        /// Only process if character in range
        /// </summary>
        private void Update()
        {
            if (!hasPlayer) return;
            // auto check and process the queue
            if (state == CashierState.ProcessQueue)
            {
                state = CashierState.Processing;
                ProcessQueue().Forget();
            }
        }

        public void AddCustomerToQueue(BotController botController)
        {
            // set position
            var positionIndex = objectQueue.Count;
            var targetPosition = GetQueuePosition(positionIndex);
            botController.SetTarget(targetPosition, 
                positionIndex == 0 ? myTransform.position : targetPosition + 0.5f * Vector3.left);
            
            // check if there is need for box
            if (objectQueue.Count == 0)
            {
                SpawnBox().Forget();
            }

            objectQueue.Enqueue(botController);
        }

        /// <summary>
        /// Spawn box and delay time for scale anim
        /// </summary>
        private async UniTask SpawnBox()
        {
            currentBox = Instantiate(boxPrefab);
            currentBox.ScaleVisual(out var scaleTime);
            await UniTask.Delay(TimeSpan.FromSeconds(scaleTime));
        }

        private Vector3 GetQueuePosition(int index)
        {
            return myTransform.position + new Vector3(index * 1.5f, 0, 1);
        }

        private bool hasPlayer = false;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            hasPlayer = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            hasPlayer = false;
        }

        /// <summary>
        /// If there no one -> wait a frame else wait bot to close, fill box, then give cash, dequeue then rearrange customer queue
        /// </summary>
        private async UniTaskVoid ProcessQueue()
        {
            if (objectQueue.Count == 0)
            {
                await UniTask.DelayFrame(1);
                state = CashierState.ProcessQueue;
                return;
            }

            var bot = objectQueue.Peek();
            
            // Wait until bot at top line
            await UniTask.WaitUntil(() => Vector3.Distance(bot.transform.position, GetQueuePosition(0)) <= 0.8f);
            await bot.FillBox(currentBox);
            await bot.GiveCash(this);
            
            // Update line
            objectQueue.Dequeue();
            int index = 0;
            foreach (var obj in objectQueue)
            {
                var pos = GetQueuePosition(index);
                var lookPosition = index == 0 ? myTransform.position : pos + 0.5f * Vector3.left;
                obj.SetTarget(pos,lookPosition);
                index++;
            }

            var isBotRemain = objectQueue.Count != 0;
            if (isBotRemain)
            {
                await SpawnBox();
            }

            state = CashierState.ProcessQueue;
        }

        [SerializeField] private Box boxPrefab;
        private Box currentBox;

        private Stack<Cash> cashes = new Stack<Cash>();
        
        public void StoreCash(Cash cash)
        {
            cashes.Push(cash);
        }
        
        public static Vector3 GetCashPosition(int index)
        {
            var heightIndex = index / 8;
            var colIndex = index % 4;
            var rowIndex = (index / 4) % 2;
            return new Vector3( -1 - colIndex * 0.25f, 0.5f + 0.1f * heightIndex, -0.38f + rowIndex * 0.52f);
        }

        // time that cashier wait each time it check
        protected override float GetCooldownTime()
        {
            return 0.05f;
        }
        
        protected override bool CanInteractWithPlayer()
        {
            // can interact + has cash
            return interactionCounter <= 0f && cashes.Count > 0;
        }
        
        /// <summary>
        /// Give player cash if have
        /// </summary>
        protected override void InteractPlayer()
        {
            var cash = cashes.Pop();
            character.TakeCash(cash);
        }
    }
}

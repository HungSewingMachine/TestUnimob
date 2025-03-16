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

        private void Update()
        {
            if (!hasPlayer) return;
            if (state == CashierState.ProcessQueue)
            {
                state = CashierState.Processing;
                ProcessQueue().Forget();
            }
        }

        public void Enqueue(BotController botController)
        {
            var positionIndex = objectQueue.Count;
            botController.SetTarget(GetQueuePosition(positionIndex));

            if (objectQueue.Count == 0)
            {
                SpawnBox().Forget();
            }
            objectQueue.Enqueue(botController);
        }

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
        /// 
        /// </summary>
        private async UniTaskVoid ProcessQueue()
        {
            if (objectQueue.Count == 0)
            {
                await UniTask.DelayFrame(1);
                state = CashierState.ProcessQueue;
                return;
            }

            var bot = objectQueue.Dequeue();
            
            await bot.FillBox(currentBox);
            await bot.GiveCash(this);
            
            // Update line
            int index = 0;
            foreach (var obj in objectQueue)
            {
                var pos = GetQueuePosition(index);
                obj.SetTarget(pos);
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

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Entity
{
    public class BotController : Character
    {
        [FormerlySerializedAs("renderer")] 
        [SerializeField] private Renderer botRenderer;
        [SerializeField] private Material[] materials;
        
        [field : SerializeField] public Vector3 TargetPosition { get; private set; }
        
        private Table fruitTable;
        private int positionIndex;
        [SerializeField] private int capacity;

        protected override Vector3 ProcessInput()
        {
            var dir = TargetPosition - modelTransform.position;
            if (dir.magnitude > 0.5f)
            {
                return dir.normalized;
            }

            return Vector3.zero;
        }

        protected override int MaxCapacity()
        {
            return capacity;
        }

        protected override void Start()
        {
            base.Start();
            Initialize();
        }

        public void Initialize()
        {
            cts = new CancellationTokenSource();
            capacity = Random.Range(1, 5);
            
            //renderer.material = materials[Random.Range(0, materials.Length)];
        }

        public void Respawn()
        {
            var randomOffset = Random.insideUnitCircle * 5;
            var pos = new Vector3( -24 + randomOffset.x, 0, -6 + randomOffset.y);
            SetPosition(pos);
            DestroyBox();
        }

        private void OnDestroy()
        {
            // if (renderer.material != null)
            // {
            //     Destroy(renderer.material);
            // }
            
            var director = GetComponent<PlayableDirector>();
            if (director != null)
            {
                director.Stop();
            }
            cts?.Cancel();
            cts?.Dispose();
        }

        /// <summary>
        /// cached table and index for release position when done!
        /// </summary>
        /// <param name="table"></param>
        public void RegisterTable(Table table)
        {
            var pos = table.GetPosition(out var idx);
            SetTarget(pos);
            
            fruitTable = table;
            positionIndex = idx;
        }
        
        /// <summary>
        /// Called after bot has filled. Release position for table!
        /// </summary>
        public void UnregisterTable()
        {
            fruitTable.ReleasePosition(positionIndex);
        }

        public void SetTarget(Vector3 position)
        {
            TargetPosition = position;
        }

        [SerializeField] private Cash cashPrefab;

        private CancellationTokenSource cts;
        
        public async UniTask GiveCash(Cashier c)
        {
            for (int i = 0; i < MaxCapacity() * 3; i++)
            {
                var cash = Instantiate(cashPrefab, modelTransform.position, Quaternion.identity);
                cash.MoveTo(c.transform, c.GetCashPosition(i), onComplete: () =>
                {
                    c.StoreCash(cash);
                });
                await UniTask.Delay(100, cancellationToken: cts.Token);
            }

            HasPay = true;
        }
        
        [field : SerializeField] public bool HasPay {get; private set;}
        
        public async UniTask FillBox(Box box)
        {
            if (fruits.Count == 0) return;
            
            var boxTransform = box.transform;
            var counter = 0;
            while (fruits.Count > 0)
            {
                var f = fruits.Pop();
                f.MoveTo(boxTransform, box.GetFruitPosition(counter));
                counter++;
                await UniTask.Delay(250, cancellationToken: cts.Token);
            }
            
            box.PlayAnimation();
            await UniTask.Delay(1000, cancellationToken: cts.Token);
            var localPosition = new Vector3(0, 0.8f, 1f);
            box.MoveTo(modelTransform, localPosition);
            TakeBox(box);
        }

        private Box currentBox = null;
        private void TakeBox(Box b)
        {
            hasBox = true;
            currentBox = b;
        }

        private void DestroyBox()
        {
            if (currentBox != null)
            {
                Destroy(currentBox.gameObject);
            }
            
            hasBox = false;
            HasPay = false;
        }
    }
}
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
        [field : SerializeField] public Vector3 LookPosition { get; private set; }
        
        private Table fruitTable;
        private int positionIndex;
        [SerializeField] private int capacity;

        /// <summary>
        /// Handle input for bot, position, rotation
        /// </summary>
        /// <returns></returns>
        protected override Vector3 ProcessInput()
        {
            var dir = TargetPosition - modelTransform.position;
            if (dir.magnitude > .8f)
            {
                return dir.normalized;
            }
            
            // if target is close, change input to focus on target
            if (dir.magnitude > 0.5f)
            {
                TargetPosition = modelTransform.position + (LookPosition - modelTransform.position).normalized * 0.5f;
                return (TargetPosition - modelTransform.position).normalized;
            }

            return Vector3.zero;
        }

        public override int MaxCapacity()
        {
            return capacity;
        }

        protected override void Start()
        {
            base.Start();
            Initialize();
        }

        private CancellationTokenSource cts;
        private void Initialize()
        {
            cts = new CancellationTokenSource();
        }
        
        /// <summary>
        /// Clean up
        /// </summary>
        private void OnDestroy()
        {
            CheckAndDestroyMat();
            
            var director = GetComponent<PlayableDirector>();
            if (director != null)
            {
                director.Stop();
            }
            cts?.Cancel();
            cts?.Dispose();
        }

        /// <summary>
        /// Called at the first state of bot to reset
        /// </summary>
        public void Respawn()
        {
            capacity = Random.Range(1, 5);

            CheckAndDestroyMat();
            botRenderer.material = materials[Random.Range(0, materials.Length)];
            
            var randomOffset = Random.insideUnitCircle * 5;
            var pos = new Vector3( -24 + randomOffset.x, 0, -6 + randomOffset.y);
            SetPosition(pos);
            DestroyBox();
        }

        private void CheckAndDestroyMat()
        {
            if (botRenderer.material != null)
            {
                Destroy(botRenderer.material);
            }
        }
        

        /// <summary>
        /// cached table and index for release position when done!
        /// </summary>
        /// <param name="table"></param>
        public void RegisterTablePosition(Table table)
        {
            var pos = table.GetPosition(out var idx);
            SetTarget(pos, table.transform.position);
            
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

        /// <summary>
        /// Main method to control where bot go, and what to look at
        /// </summary>
        /// <param name="position"></param>
        /// <param name="lookPosition"></param>
        public void SetTarget(Vector3 position, Vector3 lookPosition)
        {
            TargetPosition = position;
            LookPosition = lookPosition;
        }

        [SerializeField] private Cash cashPrefab;
        
        /// <summary>
        /// Put cash to the table
        /// </summary>
        /// <param name="c"></param>
        public async UniTask GiveCash(Cashier c)
        {
            for (int i = 0; i < MaxCapacity() * 3; i++)
            {
                var cash = Instantiate(cashPrefab, modelTransform.position, Quaternion.identity);
                cash.MoveTo(c.transform, Cashier.GetCashPosition(i), onComplete: () =>
                {
                    c.StoreCash(cash);
                });
                await UniTask.Delay(100, cancellationToken: cts.Token);
            }

            HasPay = true;
        }
        
        [field : SerializeField] public bool HasPay {get; private set;} // for bot statemachine to check
        
        /// <summary>
        /// Fill fruit to box, play close animation, then move the box
        /// </summary>
        /// <param name="box"></param>
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
                await UniTask.Delay(100, cancellationToken: cts.Token);
            }
            
            box.PlayCloseAnimation();
            await UniTask.Delay(1000, cancellationToken: cts.Token);
            var localPosition = new Vector3(0, 0.8f, 1f);
            box.MoveTo(modelTransform, localPosition);
            TakeBox(box);
        }

        // ======= Cached current box to destroy when reset bot =========================
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
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Entity
{
    public class BotController : Character
    {
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

            capacity = Random.Range(1, 5);
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
            fruitTable.ReleasePosition(positionIndex, this);
        }

        public void SetTarget(Vector3 position)
        {
            TargetPosition = position;
        }

        [SerializeField] private Cash cashPrefab;

        private async UniTask GiveCash()
        {
            var cashier = FindObjectOfType<Cashier>();
            
            for (int i = 0; i < 12; i++)
            {
                var cash = Instantiate(cashPrefab, modelTransform.position, Quaternion.identity);
                cash.MoveTo(cashier.transform, cashier.GetCashPosition(i));
                cashier.StoreCash(cash);
                await UniTask.Delay(100);
            }

            HasPay = true;
        }
        
        [field : SerializeField] public bool HasPay {get; private set;}
        
        public async UniTask FillBoxThenGiveCash(Box box)
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
            
            await GiveCash();
        }
    }
}
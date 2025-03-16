using UnityEngine;

namespace Entity
{
    public class BotController : Character
    {
        [field : SerializeField] public Vector3 TargetPosition { get; private set; }
        
        private Table fruitTable;
        private int positionIndex;
        private int capacity;

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
            fruitTable.ReleasePosition(positionIndex);
        }

        public void SetTarget(Vector3 position)
        {
            TargetPosition = position;
        }

        [SerializeField] private Cash cashPrefab;
        
        public void GiveCash(int numberOfFruit, Cashier cashier)
        {
            for (int i = 0; i < numberOfFruit * 3; i++)
            {
                var cash = Instantiate(cashPrefab, modelTransform.position, Quaternion.identity);
                cash.MoveTo(cashier.transform, cashier.GetCashPosition(i));
                cashier.StoreCash(cash);
            }
        }
    }
}
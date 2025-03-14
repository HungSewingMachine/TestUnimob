using UnityEngine;

namespace Entity
{
    public class BotController : Character
    {
        public Vector3 targetPosition;
        
        private Table fruitTable;
        private int positionIndex;
        private int capacity;

        protected override Vector3 ProcessInput()
        {
            var dir = targetPosition - modelTransform.position;
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
            targetPosition = table.GetPosition(out var idx);
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
    }
}
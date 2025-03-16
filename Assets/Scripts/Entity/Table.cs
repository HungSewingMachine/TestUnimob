using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private Transform tableTransform;
        
        public const int MAX_CAPACITY = 15;
        public const float INTERACTION_TIME = 0.15F;
        
        private Stack<ITransfer> fruits = new Stack<ITransfer>(MAX_CAPACITY);
        
        public float interactionCounter;
        private ICharacter character;
        private int numberOfFruits;
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            if (character == null)
            {
                character = other.GetComponent<ICharacter>();
                if (character == null) return;
            }
            
            interactionCounter -= Time.deltaTime;
            if (interactionCounter <= 0f && fruits.Count < MAX_CAPACITY && character.CanGive())
            {
                var fruit = character.RemoveFruits();
                var index = fruits.Count;
                fruits.Push(fruit);
                fruit.MoveTo(transform, GetFruitLocalPosition(index), onComplete: () =>
                {
                    numberOfFruits++;
                });
                interactionCounter = INTERACTION_TIME;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            interactionCounter = INTERACTION_TIME;
            character = null;
        }

        private static Vector3 GetFruitLocalPosition(int index)
        {
            var row = index % 5;
            var column = index / 5;
            return new Vector3(-0.9f + row * .45f, 0.8f + column * 0.3f, -0.6f + column * 0.6f);
        }
        
        
        //================================================================
        
        public Vector3[] positions = new Vector3[4];
        private bool[] occupied = new bool[4]; 

        public Vector3 GetPosition(out int index)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                if (!occupied[i])
                {
                    occupied[i] = true;
                    index = i;
                    return new Vector3(tableTransform.position.x, 0, tableTransform.position.z) + positions[i];
                }
            }

            index = -1;
            return Vector3.zero; 
        }

        public void ReleasePosition(int index)
        {
            occupied[index] = false;
        }

        private float waitCounter = 0f;
        private Queue<ICharacter> waitClients = new Queue<ICharacter>();
        
        /// <summary>
        /// Add bot to fill list to handle
        /// </summary>
        /// <param name="c"></param>
        public void RegisterClient(ICharacter c)
        {
            if (!waitClients.Contains(c))
            {
                waitClients.Enqueue(c);
            }
        }

        private void Update()
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                GiveCustomerFruits();
                waitCounter = INTERACTION_TIME;
            }
        }

        private void GiveCustomerFruits()
        {
            while (numberOfFruits > 0 && waitClients.Count > 0)
            {
                ICharacter c = waitClients.Dequeue();
                if (!c.CanCarry()) continue;
                    
                var fruit = fruits.Pop();
                numberOfFruits--;
                c.TakeFruits(fruit);
                if (c.CanCarry())
                {
                    waitClients.Enqueue(c);
                }
            }
        }
    }
}

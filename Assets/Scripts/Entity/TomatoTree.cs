using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace Entity
{
    public class TomatoTree : MonoBehaviour
    {
        public const int MAX_FRUIT = 3;
        public const float INTERACTION_TIME = 0.15F;
        public const float GENERATE_TIME = 2F;
        
        private Queue<int> fruits = new Queue<int>();
        public float generateTimer;
        public float interactionCounter;
        
        public int numberOfFruits;
        // Start is called before the first frame update
        private void Start()
        {
            ResetTimer();
        }

        private void ResetTimer()
        {
            generateTimer = GENERATE_TIME;
        }

        // gen fruit
        // simulate on trigger stay!
        private void Update()
        {
            GenerateFruits();

            numberOfFruits = fruits.Count;
        }

        private void GenerateFruits()
        {
            if (fruits.Count < MAX_FRUIT)
            {
                generateTimer -= Time.deltaTime;
                if (generateTimer <= 0f)
                {
                    fruits.Enqueue(1);
                    generateTimer = GENERATE_TIME;
                }
            }
            else
            {
                ResetTimer();
            }
        }

        private ICharacter character;
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            if (character == null)
            {
                character = other.GetComponent<ICharacter>();
                if (character == null) return;
            }
            
            interactionCounter -= Time.deltaTime;
            if (interactionCounter <= 0f && fruits.Count > 0 && character.CanCarry())
            {
                fruits.Dequeue();
                character.TakeFruits();
                interactionCounter = INTERACTION_TIME;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            interactionCounter = INTERACTION_TIME;
            character = null;
        }
    }
}

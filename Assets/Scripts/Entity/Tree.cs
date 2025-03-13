using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Tree : MonoBehaviour, IInteractable
    {
        public const int MAX_FRUIT = 3;

        public const float GENERATE_TIME = 2F;
        
        private Queue<int> fruits = new Queue<int>();
        public float generateTimer;
        
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
            
            numberOfFruits = fruits.Count;
        }

        public bool Interact()
        {
            if (fruits.Count > 0)
            {
                fruits.Dequeue();
                print("lose 1 fruit!");
                return true;
            }
            
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Table : MonoBehaviour
    {
        public const int MAX_CAPACITY = 10;
        public const float INTERACTION_TIME = 0.15F;
        
        public int numberOfFruits;
        
        private Stack<int> fruits = new Stack<int>(MAX_CAPACITY);

        public bool TransferTo(int fruit)
        {
            if (fruits.Count < MAX_CAPACITY)
            {
                fruits.Push(fruit);
                return true;
            }
            
            return false;
        }

        public int GetFrom()
        {
            if (fruits.Count > 0)
            {
                return fruits.Pop();
            }

            return -1;
        }

        private void Update()
        {
            numberOfFruits = fruits.Count;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                print(GetFrom());
            }
        }
        
        public float interactionCounter;
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
            if (interactionCounter <= 0f && fruits.Count < MAX_CAPACITY && character.CanGive())
            {
                fruits.Push(character.RemoveFruits());
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

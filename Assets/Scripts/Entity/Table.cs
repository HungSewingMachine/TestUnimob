using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Table : MonoBehaviour
    {
        public const int MAX_CAPACITY = 15;
        public const float INTERACTION_TIME = 0.15F;
        
        public int numberOfFruits;
        
        private Stack<IFruit> fruits = new Stack<IFruit>(MAX_CAPACITY);

        public IFruit GetFrom()
        {
            if (fruits.Count > 0)
            {
                return fruits.Pop();
            }

            return null;
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
                var fruit = character.RemoveFruits();
                var index = fruits.Count;
                fruit.MoveTo(transform, GetFruitLocalPosition(index));
                fruits.Push(fruit);
                interactionCounter = INTERACTION_TIME;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            interactionCounter = INTERACTION_TIME;
            character = null;
        }

        public static Vector3 GetFruitLocalPosition(int index)
        {
            var row = index % 5;
            var column = index / 5;
            return new Vector3(-0.9f + row * .45f, 0.3f + column * 0.3f, -0.6f + column * 0.6f);
        }
    }
}

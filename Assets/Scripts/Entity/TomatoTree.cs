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
        
        private static readonly int IsGrowing = Animator.StringToHash("IsGrowing");
        private readonly Vector3[] spawnPoints = new Vector3[MAX_FRUIT]
        {
            new Vector3(0.35f, 1.12f, 0.12f),
            new Vector3(-0.46f, 1.4f, -0.26f),
            new Vector3(-0.6f, 0.9f, 0.3f),
        };

        [SerializeField] private Transform myTransform;
        [SerializeField] private Tomato tomatoPrefab;
        [SerializeField] private Animator animator;
        
        private Queue<Tomato> fruits = new Queue<Tomato>();
        public float generateTimer;
        public float interactionCounter;
        
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
        private void Update()
        {
            CheckGenerateFruits();
        }

        private void CheckGenerateFruits()
        {
            var canGenerateFruit = fruits.Count < MAX_FRUIT;
            if (canGenerateFruit)
            {
                generateTimer -= Time.deltaTime;
                if (generateTimer <= 0f)
                {
                    GenerateFruit();
                    generateTimer = GENERATE_TIME;
                }
            }
            else
            {
                ResetTimer();
            }

            animator.SetBool(IsGrowing, canGenerateFruit);
        }

        private void GenerateFruit()
        {
            var positionIndex = fruits.Count % MAX_FRUIT;
            var groundPosition = myTransform.position + spawnPoints[positionIndex] + Vector3.down;
            var tomato = Instantiate(tomatoPrefab, groundPosition, Quaternion.identity, myTransform);
            tomato.MoveTo(myTransform, spawnPoints[positionIndex], onComplete: () =>
            {
                fruits.Enqueue(tomato);
            });
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
                var fruit = fruits.Dequeue();
                character.TakeFruits(fruit);
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

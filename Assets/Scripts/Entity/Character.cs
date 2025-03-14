using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// Bot and Player use the same character, the only difference is the input!
    /// </summary>
    public abstract  class Character : MonoBehaviour,ICharacter
    {
        public float moveSpeed = 5f; // Movement Speed
        public float acceleration = 10f; // How fast it reaches target speed
        public float deceleration = 15f; // How fast it slows down
        public float rotationSpeed = 10f; // Rotation smoothing speed
        
        [SerializeField] protected Transform modelTransform;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
    
        private Vector3 velocity;
    
        private static readonly int IsMoving = Animator.StringToHash("IsMove");
        private static readonly int IsEmpty = Animator.StringToHash("IsEmpty");
        private static readonly int IsCarryMove = Animator.StringToHash("IsCarryMove");

         protected virtual void Start()
        {
            animator.SetBool(IsEmpty, true);
        }

        // Update is called once per frame
        private void Update()
        {
            var moveDirection = ProcessInput();
            var isMoving = moveDirection.sqrMagnitude > 0.1f;
            if (isMoving)
            {
                velocity.x = Mathf.Lerp(velocity.x, moveDirection.x * moveSpeed, acceleration * Time.deltaTime);
                velocity.z = Mathf.Lerp(velocity.z, moveDirection.z * moveSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                velocity.x = Mathf.Lerp(velocity.x, 0, deceleration * Time.deltaTime);
                velocity.z = Mathf.Lerp(velocity.z, 0, deceleration * Time.deltaTime);
            }
        
            velocity.y -= 9.8f * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime); 
            
            animator.SetBool(IsMoving, isMoving);
            var isEmpty = fruits.Count <= 0;
            animator.SetBool(IsEmpty, isEmpty);
            animator.SetBool(IsCarryMove, isMoving && !isEmpty);
        
            // Rotate character towards movement direction
            if (isMoving)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        protected abstract Vector3 ProcessInput();

        protected abstract int MaxCapacity();

        private Stack<IFruit> fruits = new Stack<IFruit>();
        
        public bool IsFull => fruits.Count == MaxCapacity();
        
        public bool CanGive() => fruits.Count > 0;
        
        public bool CanCarry() => fruits.Count < MaxCapacity();

        public void TakeFruits(IFruit fruit)
        {
            var index = fruits.Count;
            var localPosition = new Vector3(0, 0.8f + index * 0.3f, 0.5f);
            fruits.Push(fruit);
            fruit.MoveTo(transform, localPosition);
        }

        public IFruit RemoveFruits()
        {
            return fruits.Pop();
        }
    }
}
using Interface;
using UnityEngine;

namespace Entity
{
    public abstract class InteractBase : MonoBehaviour
    {
        public float interactionCounter;
        protected ICharacter character;

        protected abstract float GetCooldownTime();
        
        protected abstract bool CanInteract();
        
        protected abstract void Interact();
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            if (character == null)
            {
                character = other.GetComponent<ICharacter>();
                if (character == null) return;
            }

            interactionCounter -= Time.deltaTime;
            if (CanInteract())
            {
                Interact();
                interactionCounter = GetCooldownTime();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            interactionCounter = GetCooldownTime();
            character = null;
        }
    }
}
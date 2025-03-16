using Interface;
using UnityEngine;

namespace Entity
{
    public abstract class InteractBase : MonoBehaviour
    {
        public float interactionCounter;
        protected IBuyer character;

        protected abstract float GetCooldownTime();
        
        protected abstract bool CanInteractWithPlayer();
        
        protected abstract void InteractPlayer();
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            if (character == null)
            {
                character = other.GetComponent<IBuyer>();
                if (character == null) return;
            }

            interactionCounter -= Time.deltaTime;
            if (CanInteractWithPlayer())
            {
                InteractPlayer();
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
using DG.Tweening;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Cash : MonoBehaviour, ITransfer
    {
        [SerializeField] private Transform cashTransform;
        
        public void MoveTo(Transform parent, Vector3 position, bool destroyedAtEnd = false)
        {
            cashTransform.SetParent(parent);
            var tween = cashTransform.DOLocalMove(position, 0.2f);
            if (destroyedAtEnd)
            {
                tween.OnComplete(DestroyGameObject);
            }
        }

        private void DestroyGameObject()
        {
            Destroy(gameObject);
        }
    }
}
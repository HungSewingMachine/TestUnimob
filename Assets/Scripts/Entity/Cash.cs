using DG.Tweening;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Cash : MonoBehaviour, ITransfer
    {
        [SerializeField] private Transform cashTransform;
        
        public void MoveTo(Transform parent, Vector3 position, bool destroyedAtEnd = false, System.Action onComplete = null)
        {
            //cashTransform.SetParent(parent);
            var tween = cashTransform.DOMove(parent.position + position, 0.2f);
            tween.OnComplete(() =>
            {
                onComplete?.Invoke();
                if (destroyedAtEnd)
                {
                    DestroyGameObject();
                }
            });
        }

        private void DestroyGameObject()
        {
            Destroy(gameObject);
        }
    }
}
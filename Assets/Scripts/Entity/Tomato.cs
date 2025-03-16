using Data;
using DG.Tweening;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Tomato : MonoBehaviour, ITransfer
    {
        [SerializeField] private GameConfig config;
        [SerializeField] private Transform myTransform;
        
        public void MoveTo(Transform parent, Vector3 position, bool destroyedAtEnd = false, System.Action onComplete = null)
        {
            myTransform.SetParent(parent);
            myTransform.DOLocalMove(position, config.fruitMoveTime).OnComplete(() => onComplete?.Invoke());
        }
    }
}

using DG.Tweening;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Tomato : MonoBehaviour, ITransfer
    {
        [SerializeField] private Transform myTransform;
        
        public void MoveTo(Transform parent, Vector3 position, bool destroyedAtEnd = false)
        {
            myTransform.SetParent(parent);
            myTransform.DOLocalMove(position, 0.2f);
            //myTransform.localPosition = position;
        }
    }
}

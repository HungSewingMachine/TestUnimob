using DG.Tweening;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Tomato : MonoBehaviour, IFruit
    {
        [SerializeField] private Transform myTransform;
        
        public void MoveTo(Transform parent, Vector3 position)
        {
            myTransform.SetParent(parent);
            myTransform.DOLocalMove(position, 0.2f);
            //myTransform.localPosition = position;
        }
    }
}

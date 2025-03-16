using System;
using Data;
using DG.Tweening;
using Interface;
using UnityEngine;

namespace Entity
{
    public class Box : MonoBehaviour, ITransfer
    {
        [SerializeField] private GameConfig config;
        [SerializeField] private Transform myTransform;
        [SerializeField] private Animator animator;
        
        private static readonly int IsClose = Animator.StringToHash("IsClose");
        private readonly Vector3[] fruitPositions = new Vector3[4]
        {
            new Vector3(0.2f, 0, -0.2f),
            new Vector3(-0.2f, 0, -0.2f),
            new Vector3(0.2f, 0, 0.2f),
            new Vector3(-0.2f, 0, 0.2f),
        };

        public Vector3 GetFruitPosition(int index)
        {
            return fruitPositions[index];
        }

        /// <summary>
        /// Called at init for anim
        /// </summary>
        /// <param name="scaleTime"></param>
        public void ScaleVisual(out float scaleTime)
        {
            myTransform.DOScale(Vector3.one, config.boxScaleTime).SetEase(Ease.OutBounce);
            scaleTime = config.boxScaleTime;
        }
        
        public void PlayCloseAnimation()
        {
            animator.SetTrigger(IsClose);
        }
        
        public void MoveTo(Transform parent, Vector3 position, bool destroyedAtEnd = false, Action onComplete = null)
        {
            myTransform.SetParent(parent);
            
            var s = DOTween.Sequence();
            s.Append(myTransform.DOLocalMove(position, .5f));
            s.Join(myTransform.DORotate(new Vector3(0, 359, 0), .5f, RotateMode.FastBeyond360));
            if (onComplete != null)
            {
                s.OnComplete(() => onComplete());
            }
        }
    }
}
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        public float tomatoTreeGrowTime = 1f;
        public float tomatoMoveTime = 0.5f;
        public float boxScaleTime = 0.3f;
    }
}
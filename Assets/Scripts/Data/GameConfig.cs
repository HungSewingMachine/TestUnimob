using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        public float tomatoTreeGrowTime = 1f;
        public float fruitMoveTime = 0.2f;
        public float boxScaleTime = 0.3f;
    }
}
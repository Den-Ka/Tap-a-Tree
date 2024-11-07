using UnityEngine;

namespace Tap_a_Tree
{
    [CreateAssetMenu(menuName = "Game/Game Config", fileName = "Game Config")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField, Min(0f)] public float MinDistanceBetweenTrees { get; set; } = 0.5f;
        [field: SerializeField, Min(0f)] public float MaxDistanceBetweenTrees { get; set; } = 4f;
        
        public float GetRandomDistanceBetweenTrees() => Random.Range(MinDistanceBetweenTrees, MaxDistanceBetweenTrees);

        
#if UNITY_EDITOR
        private void OnValidate()
        {
            MaxDistanceBetweenTrees = Mathf.Max(MinDistanceBetweenTrees, MaxDistanceBetweenTrees);
        }
#endif
        
    }
}
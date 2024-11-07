using UnityEngine;

namespace Tap_a_Tree
{
    [CreateAssetMenu(menuName = "Game/Level Config", fileName = "Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private Sprite[] _treesSprites;
        
        public Sprite GetRandomTreeSprite() => _treesSprites[Random.Range(0, _treesSprites.Length)];
    }
}
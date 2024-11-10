using Tap_a_Tree.Player.Resources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tap_a_Tree
{
    [CreateAssetMenu(menuName = "Game/Game Config", fileName = "Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Sprites")]
        [SerializeField] private Sprite[] _treesSprites;
        [FormerlySerializedAs("_foliageSprites")] [SerializeField] private Sprite[] _decorativeFoliageSprites;

        [Header("Spawn Rate")]
        [field: SerializeField] public float MinDistanceBetweenTrees { get; private set; } = 0.75f;
        [field: SerializeField] public float MaxDistanceBetweenTrees { get; private set; } = 4f;
        [field: Space]
        [field: SerializeField] public float MinDistanceBetweenDecorativeFoliage { get; private set; } = 0.2f;
        [field: SerializeField] public float MaxDistanceBetweenDecorativeFoliage { get; private set; } = 1f;
        [Header("Trees Settings")]
        [SerializeField] private int _treeHealth = 20;
        [FormerlySerializedAs("_primaryBonusChance")] [SerializeField, Range(0f, 1f)] private float _fruitSpawnChance = 0.2f;
        
        public float RandomDistanceBetweenTrees 
            => Random.Range(MinDistanceBetweenTrees, MaxDistanceBetweenTrees);
        public float RandomDistanceBetweenDecorativeFoliage 
            => Random.Range(MinDistanceBetweenDecorativeFoliage, MaxDistanceBetweenDecorativeFoliage);

        public Sprite RandomDecorativeFoliageSprite
            => _decorativeFoliageSprites[Random.Range(0, _decorativeFoliageSprites.Length)];

        public TreeSpawnData GetRandomTree()
        {
            var randomWood = new Resource(ResourceType.Wood, Random.Range(0, _treeHealth) + 1);

            Resource[] resources = TryGetBonusResource(out var bonusResource)
                ? new[] { randomWood, new Resource(bonusResource.Value, 1) }
                : new[] { randomWood };
            
            return new TreeSpawnData
            {
                Sprite = _treesSprites[Random.Range(0, _treesSprites.Length)],
                Health = _treeHealth,
                Drop = resources,
            };
        }

        private bool TryGetBonusResource(out ResourceType? bonusType)
        {
            var random = Random.Range(0, 1f);
            if (random < _fruitSpawnChance)
            {
                ResourceType[] possibleBonuses = { ResourceType.Apple, ResourceType.Orange, ResourceType.Pear };
                
                bonusType = possibleBonuses[Random.Range(0, possibleBonuses.Length)];
                return true;
            }
            else
            {
                bonusType = null;
                return false;
            }
        }
    }
}
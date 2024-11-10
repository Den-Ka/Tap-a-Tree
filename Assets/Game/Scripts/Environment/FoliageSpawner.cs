using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tap_a_Tree.Environment
{
    public class FoliageSpawner
    {
        private readonly GameConfig _config;

        private PoolableFoliageFactory<Tree> _treeFactory;
        private PoolableFoliageFactory<SpriteRenderer> _decorationsFactory;

        public FoliageSpawner(
            GameConfig config,
            Transform spawnContainer,
            Tree treePrefab)
        {
            _config = config;

            _treeFactory = new PoolableFoliageFactory<Tree>(
                () => Object.Instantiate(treePrefab, spawnContainer),
                spawnContainer,
                10
            );
            _decorationsFactory = new PoolableFoliageFactory<SpriteRenderer>(
                () => new GameObject().AddComponent<SpriteRenderer>(),
                spawnContainer,
                10
            );
        }

        public IPooledTransform SpawnPooledTree(Vector3 position)
        {
            TreeSpawnData treeSpawnData = _config.GetRandomTree();
            PooledTransform<Tree> pooledTree = _treeFactory.Create(position);
            
            Tree tree = pooledTree.Value;
            tree.Initialize(treeSpawnData);
            
            return pooledTree;
        }

        public IPooledTransform SpawnPooledDecoration(Vector3 position)
        {
            PooledTransform<SpriteRenderer> pooledDecoration = _decorationsFactory.Create(position);
            pooledDecoration.Value.sprite = _config.RandomDecorativeFoliageSprite;

            return pooledDecoration;
        }
    }
}
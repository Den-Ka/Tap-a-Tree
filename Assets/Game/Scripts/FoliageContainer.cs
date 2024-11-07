using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tap_a_Tree
{
    public class FoliageContainer : MonoBehaviour
    {
        [SerializeField] private Tree _treePrefab;
        [Space] [SerializeField, Min(0)] private float _worldBordersOffset = 1.5f;

        [SerializeField, HideInInspector] private float _worldLeftBorder;
        [SerializeField, HideInInspector] private float _worldRightBorder;

        public float SpawnPosition => _worldRightBorder + _worldBordersOffset;
        public float DespawnPosition => _worldLeftBorder - _worldBordersOffset;

        private readonly Queue<Transform> _foliageQueue = new();

        public void AddTree(Sprite treeSprite, float spawnOffset)
        {
            var tree = CreateNewTree();
            tree.Sprite = treeSprite;
            tree.transform.position = new Vector3(SpawnPosition + spawnOffset, 0, 0);
            
            _foliageQueue.Enqueue(tree.transform);
        }

        private Tree CreateNewTree()
        {
            return Instantiate(_treePrefab, Vector3.right * SpawnPosition, Quaternion.identity, transform);
        }

        public void Scroll(float distance)
        {
            if (_foliageQueue.Count == 0 || distance == 0) return;

            foreach (var foliage in _foliageQueue)
            {
                foliage.Translate(Vector3.left * distance);
            }

            var peek = _foliageQueue.Peek();

            //Handle finished foliages
            while (peek != null)
            {
                if (peek.position.x <= DespawnPosition)
                {
                    _foliageQueue.Dequeue();
                    Destroy(peek.gameObject);

                    peek = _foliageQueue.Peek();
                }
                else
                {
                    peek = null;
                }
            }
        }

#if UNITY_EDITOR
        public void ValidateFromWorldWidth(float worldWidth)
        {
            _worldRightBorder = worldWidth / 2;
            _worldLeftBorder = worldWidth / -2;
        }

        private void OnDrawGizmos()
        {
            float gizmosWidth = _worldBordersOffset * 2;
            float gizmosHeight = 5f;

            var gizmosSize = new Vector3(gizmosWidth, gizmosHeight);

            float gizmosYOffset = gizmosHeight / 2;

            var spawnCenter = transform.position
                              + new Vector3(SpawnPosition, gizmosYOffset);
            var destroyCenter = transform.position
                                + new Vector3(DespawnPosition, gizmosYOffset);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(destroyCenter, gizmosSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnCenter, gizmosSize);
        }
#endif
    }
}
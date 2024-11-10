using UnityEngine;

namespace Tap_a_Tree.Environment
{
    public class EnvironmentSetup : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _worldWidth = 10f;
        [SerializeField, Min(0)] private float _worldBordersOffset = 1.5f;
        [Space]
        [SerializeField] private SpriteRenderer _groundTopLayer;
        [SerializeField] private SpriteRenderer _groundLowerLayer;
        [field: Space]
        [field: SerializeField] public Tree TreePrefab { get; private set; }
        [field: SerializeField] public Transform SpawnContainer { get; private set; }

        private float WorldLeftBorder => transform.position.x - _worldWidth / 2;
        private float WorldRightBorder => transform.position.x + _worldWidth / 2;

        public Vector3 WorldSpawnPoint => new Vector3(WorldRightBorder + _worldBordersOffset, transform.position.y,
            transform.position.z);

        public Vector3 WorldDespawnPoint => new Vector3(WorldLeftBorder - _worldBordersOffset, transform.position.y,
            transform.position.z);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;

            ValidateGroundLayer(_groundTopLayer);
            ValidateGroundLayer(_groundLowerLayer);
        }

        private void ValidateGroundLayer(SpriteRenderer ground)
        {
            if (ground == null) return;

            ground.drawMode = SpriteDrawMode.Tiled;

            var size = ground.size;
            size.x = _worldWidth;
            ground.size = size;

            var localPosition = ground.transform.localPosition;
            localPosition.x = 0;
            ground.transform.localPosition = localPosition;
        }

        private void OnDrawGizmos()
        {
            float gizmosWidth = _worldBordersOffset * 2;
            float gizmosHeight = 5f;

            var gizmosSize = new Vector3(gizmosWidth, gizmosHeight);

            float gizmosYOffset = gizmosHeight / 2;

            var spawnCenter = transform.position
                              + new Vector3(WorldSpawnPoint.x, gizmosYOffset);
            var destroyCenter = transform.position
                                + new Vector3(WorldDespawnPoint.x, gizmosYOffset);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(destroyCenter, gizmosSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnCenter, gizmosSize);
        }
#endif
    }
}
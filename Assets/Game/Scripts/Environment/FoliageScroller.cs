using System.Collections.Generic;
using Tap_a_Tree.Core;
using UnityEngine;

namespace Tap_a_Tree.Environment
{
    public class FoliageScroller : MonoBehaviour
    {
        private GameConfig _config;
        private FoliageSpawner _spawner;
        private WorldScrollService _worldScrollService;
        private Vector3 _worldSpawnPoint;
        private Vector3 _worldDespawnPoint;

        private readonly List<IPooledTransform> _activeFoliage = new();
        private float _distanceToNextTree = 0;
        private float _distanceToNextDecoration = 0;

        public void Construct(
            GameConfig config,
            FoliageSpawner spawner,
            WorldScrollService worldScrollService,
            Vector3 worldSpawnPoint,
            Vector3 worldDespawnPoint)
        {
            _config = config;

            _spawner = spawner;
            _worldScrollService = worldScrollService;

            _worldSpawnPoint = worldSpawnPoint;
            _worldDespawnPoint = worldDespawnPoint;

            enabled = true;
        }

        private void Update()
        {
            if (_worldScrollService == null)
            {
                enabled = false;
                return;
            }

            if (_worldScrollService.WorldScrollState == WorldScrollState.Staying) return;

            float distanceDelta = _worldScrollService.ScrollSpeed * Time.deltaTime;
            MoveActiveFoliage(distanceDelta);
            CountPassedDistances(distanceDelta);
        }

        public void ForceScroll(float distanceDelta)
        {
            MoveActiveFoliage(distanceDelta);
            CountPassedDistances(distanceDelta);
        }

        private void CountPassedDistances(float distanceDelta)
        {
            _distanceToNextTree -= distanceDelta;
            _distanceToNextDecoration -= distanceDelta;

            if (_distanceToNextTree < 0) SpawnTrees();
            if (_distanceToNextDecoration < 0) SpawnDecoration();
        }

        private void MoveActiveFoliage(float distanceDelta)
        {
            if (_activeFoliage.Count == 0) return;

            for (int i = 0; i < _activeFoliage.Count; i++)
            {
                var foliageTransform = _activeFoliage[i].Transform;
                foliageTransform.Translate(Vector3.left * distanceDelta);
                if (foliageTransform.position.x <= _worldDespawnPoint.x)
                {
                    _activeFoliage[i].Release();
                    _activeFoliage.RemoveAt(i);
                    i--;
                }
            }
        }

        private void SpawnTrees()
        {
            while (_distanceToNextTree <= 0)
            {
                IPooledTransform pooledTreeObject =
                    _spawner.SpawnPooledTree(GetSpawnPointWithOffset(_distanceToNextTree));

                _activeFoliage.Add(pooledTreeObject);

                _distanceToNextTree += _config.RandomDistanceBetweenTrees;
            }
        }

        private void SpawnDecoration()
        {
            while (_distanceToNextDecoration <= 0)
            {
                IPooledTransform pooledDecoration =
                    _spawner.SpawnPooledDecoration(GetSpawnPointWithOffset(_distanceToNextDecoration));

                _activeFoliage.Add(pooledDecoration);

                _distanceToNextDecoration += _config.RandomDistanceBetweenDecorativeFoliage;
            }
        }

        private Vector3 GetSpawnPointWithOffset(float distanceOffset)
            => _worldSpawnPoint + Vector3.right * distanceOffset;
    }
}
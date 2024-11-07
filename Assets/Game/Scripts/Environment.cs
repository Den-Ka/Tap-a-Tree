using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tap_a_Tree
{
    public class Environment : MonoBehaviour
    {
        private static readonly int UVOffsetProperty = Shader.PropertyToID("_UV_Offset");

        [SerializeField, Min(0)] private float _worldWidth = 10f;
        [Space]
        [SerializeField] private FoliageContainer _foliageContainer;
        [Space]
        [SerializeField] private SpriteRenderer _groundTopLayer;
        [SerializeField] private SpriteRenderer _groundLowerLayer;
        [Space]
        [SerializeField] private Material _material;
        [Space]
        [SerializeField, Range(0f, 1f)]
        private float _groundTextureOffset;

        private bool _isMoving;
        [SerializeField] private float _speed;
        [SerializeField] private float _minTreeDistance = 0.5f;
        [SerializeField] private float _maxTreeDistance = 4f;

        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private LevelConfig _levelConfig;
        
        private float _distanceToNewTree;
        
        private void Start()
        {
            _speed = 1f;
            _isMoving = true;
        }

        private void Update()
        {
            if (!_isMoving || _speed == 0) return;

            var scrollOffset = _speed * Time.deltaTime;
            
            _foliageContainer.Scroll(scrollOffset);
            ScrollWorld(scrollOffset);
            SpawnTrees(scrollOffset);
        }

        private void SpawnTrees(float scrollOffset)
        {
            _distanceToNewTree -= scrollOffset;
            
            if(_distanceToNewTree > 0) return;

            while (_distanceToNewTree < 0)
            {
                _foliageContainer.AddTree(_levelConfig.GetRandomTreeSprite(), _distanceToNewTree);
                
                _distanceToNewTree += _gameConfig.GetRandomDistanceBetweenTrees();
            }
        }

        

        private void ScrollWorld(float offset)
        {
            _groundTextureOffset += offset;
            _groundTextureOffset %= 1f;
            ValidateMaterial();
        }

        private void ValidateMaterial()
        {
            _material?.SetFloat(UVOffsetProperty, _groundTextureOffset);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(Application.isPlaying) return;
            
            _foliageContainer?.ValidateFromWorldWidth(_worldWidth);

            ValidateGroundLayer(_groundTopLayer);
            ValidateGroundLayer(_groundLowerLayer);

            ValidateMaterial();
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
#endif
    }
}
using System;
using Tap_a_Tree.Core;
using UnityEngine;

namespace Tap_a_Tree.Environment
{
    public class WorldScroller : MonoBehaviour
    {
        [Serializable]
        private class ScrollData
        {
            public SpriteRenderer SpriteRenderer;
            [Range(0f, 10f)] public float ScrollSpeedMultiplier = 1f;
            
            [HideInInspector] public Material Material;
            [HideInInspector] public float CurrentOffset;
        }

        private static readonly int UVOffsetProperty = Shader.PropertyToID("_UV_Offset");
        
        [SerializeField] private ScrollData[] _scrollData;

        private WorldScrollService _worldScrollService;

        public void Construct(WorldScrollService worldScrollService)
        {
            _worldScrollService = worldScrollService;

            for (int i = 0; i < _scrollData.Length; i++)
            {
                _scrollData[i].Material = _scrollData[i].SpriteRenderer.material;
            }
        }

        public void Update()
        {
            if(_worldScrollService == null || _worldScrollService.WorldScrollState == WorldScrollState.Staying) return;

            float baseScrollDelta = Time.deltaTime * _worldScrollService.ScrollSpeed;

            for (int i = 0; i < _scrollData.Length; i++)
            {
                ScrollData data = _scrollData[i];
                
                float currentOffset = data.CurrentOffset;
                currentOffset += baseScrollDelta * data.ScrollSpeedMultiplier;
                currentOffset %= 1f;
                data.CurrentOffset = currentOffset;
                
                data.Material?.SetFloat(UVOffsetProperty, currentOffset);
            }
        }
    }
}
using System;
using UnityEngine;

namespace Tap_a_Tree
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tree : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Sprite Sprite
        {
            get => _spriteRenderer.sprite;
            set => _spriteRenderer.sprite = value;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
#endif
    }
}
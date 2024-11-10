using System;
using Tap_a_Tree.Player.Resources;
using UnityEngine;

namespace Tap_a_Tree
{
    public class Tree : MonoBehaviour, IHealth
    {
        public event Action HealthChanged;
        public event Action Died;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Vector3 _healthBarPosition;

        [SerializeField] private TreeAnimation _animation;
        
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public Resource[] Drop { get; private set; }
        public Vector3 HealthbarWorldPosition => transform.position + _healthBarPosition;

        private bool _isDead;
        
        public void Initialize(TreeSpawnData treeSpawnData)
        {
            _spriteRenderer.sprite = treeSpawnData.Sprite;
            MaxHealth = CurrentHealth = treeSpawnData.Health;
            Drop = treeSpawnData.Drop;
            
            _animation.ResetAnimations();
            _isDead = false;
        }

        public void TakeHit(float damage, Action<Tree> deathCallback = null)
        {
            if(_isDead) return;
            
            CurrentHealth = Math.Max(0, CurrentHealth - damage);
            HealthChanged?.Invoke();

            _animation?.PlayChopAnimation();
            
            if (CurrentHealth <= 0)
            {
                _isDead = true;
                _animation.PlayDeathAnimation();
                deathCallback?.Invoke(this);
                Died?.Invoke();
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(HealthbarWorldPosition, 0.1f);
        }
#endif
    }
}
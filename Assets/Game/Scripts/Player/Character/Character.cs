using System;
using System.Collections.Generic;
using Tap_a_Tree.Core;
using Tap_a_Tree.Player.Resources;
using Tap_a_Tree.Player.Upgrades;
using UnityEngine;

namespace Tap_a_Tree
{
    [RequireComponent(typeof(Animator), typeof(AutoChopping))]
    public class Character : MonoBehaviour
    {
        private static readonly int MovingStringHash = Animator.StringToHash("IsMoving");
        private static readonly int ChopStringHash = Animator.StringToHash("Chop");
        private static readonly int SpeedStringHash = Animator.StringToHash("Speed");
        
        public event Action<Tree> HittedTree;

        [SerializeField] private Animator _animator;
        [SerializeField] private AutoChopping _autoChopping;

        private WorldScrollService _worldScrollService;
        private PlayerResourcesService _resourcesService;
        private SoundsService _soundsService;

        private Tree _targetTree;
        private readonly Queue<Tree> _treesToChopQueue = new();
        private float _axePower;
        private float _moveSpeed;

        public void Construct(
            WorldScrollService worldScrollService,
            PlayerUpgradesService upgradesService,
            PlayerResourcesService resourcesService,
            SoundsService soundsService)
        {
            _worldScrollService = worldScrollService;
            _worldScrollService.GameStateChanged += OnWorldScrollStateChanged;
            _worldScrollService.ScrollSpeedChanged += OnScrollSpeedChanged;
            _moveSpeed = _worldScrollService.ScrollSpeed;

            _resourcesService = resourcesService;
            
            _soundsService = soundsService;
            
            upgradesService.UpgradeLeveledUp += OnUpgradeLeveledUp;
            _axePower = upgradesService[UpgradeType.AxePower].Value;

            _autoChopping.Construct(upgradesService);
        }

        public bool TryChop()
        {
            if (_targetTree == null) return false;

            _animator.SetTrigger(ChopStringHash);
            _soundsService.PlayChopSound();

            HittedTree?.Invoke(_targetTree);
            _targetTree.TakeHit(_axePower);

            return true;
        }

        private void SetTargetTree(Tree tree)
        {
            if (_targetTree != null) _targetTree.Died -= OnTreeChopped;
            
            _targetTree = tree;

            if (_targetTree != null)
            {
                _targetTree.Died += OnTreeChopped;
                _worldScrollService.WorldScrollState = WorldScrollState.Staying;
            }
            else
            {
                _worldScrollService.WorldScrollState = WorldScrollState.Scrolling;
            }
        }

        private void OnTreeChopped()
        {
            _soundsService.PlayTreeChoppedSound();
            AddDropFromTree(_targetTree.Drop);
            
            _treesToChopQueue.TryDequeue(out Tree nextTree);
            SetTargetTree(nextTree);
        }

        private void AddDropFromTree(Resource[] droppedResources)
        {
            for (int i = 0; i < droppedResources.Length; i++)
            {
                _resourcesService.Add(droppedResources[i].ResourceType, droppedResources[i].Amount);
            }
        }

        private void OnScrollSpeedChanged(float speed)
        {
            _moveSpeed = speed;
            if (_worldScrollService.WorldScrollState == WorldScrollState.Scrolling)
            {
                _animator.SetFloat(SpeedStringHash, _moveSpeed);
            }
        }

        private void OnWorldScrollStateChanged(WorldScrollState worldScrollState)
        {
            switch (worldScrollState)
            {
                case WorldScrollState.Scrolling:
                    StartMovingWithSpeed(_moveSpeed);
                    break;
                case WorldScrollState.Staying:
                    StopMoving();
                    break;
            }
        }

        private void StartMovingWithSpeed(float speed)
        {
            _animator.SetBool(MovingStringHash, true);
            _animator.SetFloat(SpeedStringHash, speed);
        }

        private void StopMoving()
        {
            _animator.SetBool(MovingStringHash, false);
            _animator.SetFloat(SpeedStringHash, 0f);
        }

        private void OnUpgradeLeveledUp(Upgrade upgrade)
        {
            if (upgrade.UpgradeType == UpgradeType.AxePower) _axePower = upgrade.Value;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Tree>(out var triggeredTree) == false) return;

            if (_targetTree == triggeredTree || _treesToChopQueue.Contains(triggeredTree))
            {
                Debug.LogWarning($"Somehow the same tree entered in character trigger multiple times.");
                return;
            }
            
            if (_targetTree == null)
            {
                SetTargetTree(triggeredTree);
            }
            else
            {
                _treesToChopQueue.Enqueue(triggeredTree);
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _animator = GetComponent<Animator>();
        }
#endif
    }
}
using System;
using Tap_a_Tree.Player.Upgrades;

namespace Tap_a_Tree.Core
{
    public class WorldScrollService
    {
        public event Action<WorldScrollState> GameStateChanged;
        public event Action<float> ScrollSpeedChanged;

        private WorldScrollState _worldScrollState = WorldScrollState.Staying;

        public WorldScrollState WorldScrollState
        {
            get => _worldScrollState;
            set
            {
                if (_worldScrollState == value) return;
                
                _worldScrollState = value;
                GameStateChanged?.Invoke(_worldScrollState);
            }
        }

        private float _scrollSpeed;

        public float ScrollSpeed
        {
            get => _scrollSpeed;
            set
            {
                if (_scrollSpeed == value) return;
                
                _scrollSpeed = value;
                ScrollSpeedChanged?.Invoke(_scrollSpeed);
            }
        }

        public WorldScrollService(PlayerUpgradesService upgradesService)
        {
            upgradesService.UpgradeLeveledUp += OnUpgradeLeveledUp;
            _scrollSpeed = upgradesService[UpgradeType.MovementSpeed].Value;
        }

        private void OnUpgradeLeveledUp(Upgrade upgrade)
        {
            if(upgrade.UpgradeType == UpgradeType.MovementSpeed) ScrollSpeed = upgrade.Value;
        }
    }
}
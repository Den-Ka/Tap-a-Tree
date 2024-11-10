using System;
using Tap_a_Tree.Player.Resources;

namespace Tap_a_Tree.Player.Upgrades
{
    public class Upgrade : IUpgradeable, IPurchasable
    {
        public event Action<int> Upgraded;

        public UpgradeType UpgradeType => _config.UpgradeType;
        public int Level { get; private set; }
        public bool ReachedMax { get; private set; }
        public float Value { get; private set; }
        public int Price { get; private set; }
        public ResourceType ResourceType => _config.ResourceTypeForUpgrade;


        private readonly UpgradeConfig _config;

        public Upgrade(UpgradeConfig config, int level = 0)
        {
            _config = config;
            Level = level;

            ValidateCurrentLevel();
        }

        public bool TryIncreaseLevel()
        {
            if (ReachedMax) return false;

            IncreaseLevel();
            return true;
        }

        private void IncreaseLevel()
        {
            Level++;
            ValidateCurrentLevel();
            
            Upgraded?.Invoke(Level);
        }

        private void ValidateCurrentLevel()
        {
            Value = _config.CalculateValue(Level);
            Price = _config.CalculateUpgradeCost(Level);
            ReachedMax = Level >= _config.MaxLevel;
        }
    }
}
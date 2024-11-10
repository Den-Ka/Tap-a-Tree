using System;
using System.Collections;
using System.Collections.Generic;
using Tap_a_Tree.Player.Resources;

namespace Tap_a_Tree.Player.Upgrades
{
    public class PlayerUpgradesService
    {
        public event Action<Upgrade> UpgradeLeveledUp;

        private readonly Dictionary<UpgradeType, Upgrade> _upgradeFromTypeDictionary = new();
        private readonly Dictionary<Upgrade, UpgradeConfig> _upgradeConfigFromUpgradeDictionary = new();

        public PlayerUpgradesService(List<UpgradeConfig> upgradesConfigsList)
        {
            foreach (var upgradeConfig in upgradesConfigsList)
            {
                var upgrade = new Upgrade(upgradeConfig);
                _upgradeFromTypeDictionary.Add(upgradeConfig.UpgradeType, upgrade);
                _upgradeConfigFromUpgradeDictionary.Add(upgrade, upgradeConfig);
            }
        }

        public IEnumerable<Upgrade> ExistingUpgrades => _upgradeFromTypeDictionary.Values;

        public bool TryLevelUpUpgrade(UpgradeType upgradeType) => TryLevelUpUpgrade(GetSkill(upgradeType));
        public bool TryLevelUpUpgrade(Upgrade upgrade)
        {
            if (upgrade.TryIncreaseLevel())
            {
                UpgradeLeveledUp?.Invoke(upgrade);

                return true;
            }

            return false;
        }

        public IEnumerable<Upgrade> GetUpgradeByResourceType(ResourceType resourceType)
        {
            foreach (var skill in _upgradeFromTypeDictionary.Values)
            {
                if (skill.ResourceType == resourceType)
                {
                    yield return skill;
                }
            }
        }

        public Upgrade GetSkill(UpgradeType upgradeType) => _upgradeFromTypeDictionary[upgradeType];
        public Upgrade this[UpgradeType upgradeType] => GetSkill(upgradeType);

        public UpgradeConfig GetConfigOfUpgrade(Upgrade upgrade) => _upgradeConfigFromUpgradeDictionary[upgrade];
    }
}
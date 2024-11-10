using System;
using Tap_a_Tree.Player.Resources;
using UnityEngine;

namespace Tap_a_Tree.Player.Upgrades
{
    [CreateAssetMenu(menuName = "Game/Updrade Config", fileName = "Upgrade Config")]
    public class UpgradeConfig : ScriptableObject
    {
        [Header("Type")]
        [field: SerializeField] public UpgradeType UpgradeType { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [Header("Purchase")]
        [field: SerializeField] public ResourceType ResourceTypeForUpgrade { get; private set; }
        [field: SerializeField] public int BaseUpgradeCost { get; private set; }
        [field: SerializeField] public int UpgradeCostIncrease { get; private set; }
        [Header("Level")]
        [field: SerializeField] public int MaxLevel { get; private set; } = 20;
        [Header("Values")]
        [field: SerializeField] public float BaseValue { get; private set; }
        [field: SerializeField] public float ValueIncrease { get; private set; }

        public float CalculateValue(int currentLevel)
            => BaseValue + Math.Min(currentLevel, MaxLevel) * ValueIncrease;

        public int CalculateUpgradeCost(int currentLevel)
            => BaseUpgradeCost + Math.Min(currentLevel, MaxLevel) * UpgradeCostIncrease;
    }
}
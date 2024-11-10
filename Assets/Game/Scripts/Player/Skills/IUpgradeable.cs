using System;

namespace Tap_a_Tree.Player.Upgrades
{
    public interface IUpgradeable
    {
        event Action<int> Upgraded;

        UpgradeType UpgradeType { get; }
        int Level { get; }
        bool ReachedMax { get; }
        float Value { get; }
    }
}
using Tap_a_Tree.Player.Resources;

namespace Tap_a_Tree.Player.Upgrades
{
    public interface IPurchasable
    {
        int Price { get; }
        ResourceType ResourceType { get; }
    }
}
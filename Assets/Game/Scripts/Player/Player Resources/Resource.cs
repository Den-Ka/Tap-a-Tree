namespace Tap_a_Tree.Player.Resources
{
    public struct Resource
    {
        public ResourceType ResourceType;
        public readonly int Amount;

        public Resource(ResourceType resourceType, int amount)
        {
            ResourceType = resourceType;
            Amount = amount;
        }
    }
}
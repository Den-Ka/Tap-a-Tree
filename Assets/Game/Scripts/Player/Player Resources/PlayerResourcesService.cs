using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tap_a_Tree.Player.Resources
{
    public class PlayerResourcesService
    {
        public event Action<ResourceType, int> ResourceChanged;
        public event Action<ResourceType, int> ResourceAdded;
        public event Action<ResourceType, int> ResourceSpent;

        private readonly ResourcesIconsConfig _resourcesIconsConfig;
        
        private readonly Dictionary<ResourceType, int> _resources = new();
        private readonly Dictionary<ResourceType, Sprite> _resourcesIcons = new();

        public PlayerResourcesService(ResourcesIconsConfig resourcesIconsConfig)
        {
            _resourcesIconsConfig = resourcesIconsConfig;
            foreach (var resourceIcon in _resourcesIconsConfig.ResourcesIcons)
            {
                _resourcesIcons.Add(resourceIcon.ResourceType, resourceIcon.Icon);
            }
        }
        
        public void Add(Resource resource) => Add(resource.ResourceType, resource.Amount);

        public void Add(ResourceType resourceType, int amount)
        {
            _resources.TryGetValue(resourceType, out var currentAmount);
            currentAmount += amount;
            _resources[resourceType] = currentAmount;

            ResourceChanged?.Invoke(resourceType, currentAmount);
            ResourceAdded?.Invoke(resourceType, amount);
        }

        public void Spend(ResourceType resource, int amount)
        {
            _resources.TryGetValue(resource, out var currentAmount);

            if (currentAmount < amount) throw new InvalidOperationException("Not enough resources.");

            currentAmount -= amount;
            _resources[resource] = currentAmount;

            ResourceChanged?.Invoke(resource, currentAmount);
            ResourceSpent?.Invoke(resource, amount);
        }

        public int Get(ResourceType resource)
        {
            _resources.TryGetValue(resource, out var amount);
            return amount;
        }

        public bool HasEnough(ResourceType resourceType, int price)
        {
            return Get(resourceType) >= price;
        }

        public Sprite GetIconForResource(ResourceType resourceType) => _resourcesIcons[resourceType];
    }
}
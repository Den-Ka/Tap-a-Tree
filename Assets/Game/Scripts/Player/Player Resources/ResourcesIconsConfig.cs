using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tap_a_Tree.Player.Resources
{
    [CreateAssetMenu(menuName = "Game/Resources Icons Config", fileName = "Resources Icons Config")]
    public class ResourcesIconsConfig : ScriptableObject
    {
        [Serializable]
        public struct ResourceIcon
        {
            public ResourceType ResourceType;
            public Sprite Icon;
        }
        
        [SerializeField] private List<ResourceIcon> _resourcesIcons = new();
        public IReadOnlyCollection<ResourceIcon> ResourcesIcons => _resourcesIcons;
    }
}
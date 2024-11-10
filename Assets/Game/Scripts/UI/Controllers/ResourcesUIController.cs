using System.Collections.Generic;
using DG.Tweening;
using Tap_a_Tree.Core;
using Tap_a_Tree.Player.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Tap_a_Tree.UI.Controllers
{
    public class ResourcesUIController
    {
        private readonly PlayerResourcesService _resourceService;
        private readonly SoundsService _soundService;
        private readonly ResourceContainerUI _resourceContainerPrefab;
        private readonly RectTransform _resourcesContainer;
        private readonly ResourceContainerUI _scoresContainerUI;
        private readonly Image _resourceDropPrefab;
        private readonly RectTransform _dropContainer;

        private readonly Dictionary<ResourceType, ResourceContainerUI> _resourcesDictionary = new();

        public ResourcesUIController(
            ResourcesUI resourcesUI,
            PlayerResourcesService resourcesService,
            SoundsService soundService)
        {
            _resourceContainerPrefab = resourcesUI.ResourceContainerPrefab;
            _resourcesContainer = resourcesUI.ResourcesContainer;
            
            _soundService = soundService;
            
            _scoresContainerUI = resourcesUI.ScoresContainerUI;

            _resourceDropPrefab = resourcesUI.DropPrefab;
            _dropContainer = resourcesUI.DropContainer;

            _resourceService = resourcesService;
            _resourceService.ResourceAdded += OnResourceCollected;
            _resourceService.ResourceSpent += OnResourceSpent;

            CreateResources();
        }

        private void CreateResources()
        {
            ClearContainer();

            InstantiateResourceUI(ResourceType.Apple);
            InstantiateResourceUI(ResourceType.Orange);
            InstantiateResourceUI(ResourceType.Pear);
            _resourcesDictionary.Add(ResourceType.Wood, _scoresContainerUI);
            _scoresContainerUI.Value = _resourceService.Get(ResourceType.Wood);
        }

        private void ClearContainer()
        {
            for (int i = 0; i < _resourcesContainer.childCount; i++)
            {
                Object.Destroy(_resourcesContainer.GetChild(i).gameObject);
            }
        }

        private ResourceContainerUI InstantiateResourceUI(ResourceType resourceType)
        {
            ResourceContainerUI resourceContainerUI = Object.Instantiate(_resourceContainerPrefab, _resourcesContainer);
            resourceContainerUI.Value = _resourceService.Get(resourceType);
            resourceContainerUI.Icon = _resourceService.GetIconForResource(resourceType);

            _resourcesDictionary.Add(resourceType, resourceContainerUI);

            return resourceContainerUI;
        }

        private void OnResourceSpent(ResourceType resourceType, int amount)
        {
            _resourcesDictionary[resourceType].Value = _resourceService.Get(resourceType);
        }

        private void OnResourceCollected(ResourceType resourceType, int amount)
        {
            Sprite resourceIcon = _resourceService.GetIconForResource(resourceType);
            ResourceContainerUI resourceContainer = _resourcesDictionary[resourceType];

            var baseDelay = 0.05f;

            for (int i = 0; i < amount; i++)
            {
                AnimateResourceDrop(resourceIcon, resourceContainer, resourceType, baseDelay * i);
            }
        }

        private void AnimateResourceDrop(
            Sprite resourceIcon,
            ResourceContainerUI resourceContainer,
            ResourceType resourceType,
            float delay = 0)
        {
            Image resourceDrop = InstantiateResourceDrop(resourceIcon);

            resourceDrop.transform
                .DOJump(resourceContainer.IconPosition, Mathf.Min(0, -3f + delay), 1, 1f)
                .SetDelay(delay)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Object.Destroy(resourceDrop.gameObject);
                    int currentAmount = _resourceService.Get(resourceType);
                    resourceContainer.Value = currentAmount;
                    resourceContainer.Animate();
                    _soundService.PlayDropPickedUpSound();
                });
        }

        private Image InstantiateResourceDrop(Sprite resourceIcon)
        {
            Image image = Object.Instantiate(_resourceDropPrefab, _dropContainer);
            image.sprite = resourceIcon;
            image.gameObject.SetActive(true);
            return image;
        }
    }
}
using System;
using System.Collections.Generic;
using Tap_a_Tree.Core;
using Tap_a_Tree.Player.Resources;
using Tap_a_Tree.Player.Upgrades;
using Tap_a_Tree.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tap_a_Tree.UI.Controllers
{
    public class UpgradesUIController
    {
        private struct UpgradeButtonData
        {
            public readonly UpgradeButtonUI Button;
            public readonly Upgrade Upgrade;
            public readonly ResourceType ResourceType;

            public UpgradeButtonData(UpgradeButtonUI button, Upgrade upgrade, ResourceType resourceType)
            {
                Button = button;
                Upgrade = upgrade;
                ResourceType = resourceType;
            }
        }

        private readonly PlayerUpgradesService _upgradesService;
        private readonly PlayerResourcesService _resourcesService;
        private readonly SoundsService _soundsService;

        private readonly UpgradeButtonUI _upgradeButtonPrefab;
        private readonly RectTransform _upgradeButtonsContainer;

        private readonly List<UpgradeButtonData> _buttons = new();
        private readonly Dictionary<Upgrade, UpgradeButtonData> _upgradeToButtonDictionary = new();

        public UpgradesUIController(
            UpgradesUI upgradesUI,
            PlayerUpgradesService upgradesService,
            PlayerResourcesService resourcesService,
            SoundsService soundsService)
        {
            _upgradesService = upgradesService;
            _upgradesService.UpgradeLeveledUp += ValidateButtonWithUpgrade;
            
            _resourcesService = resourcesService;
            _resourcesService.ResourceChanged += ValidateButtonsWithResourceType;
            
            _soundsService = soundsService;
            
            _upgradeButtonPrefab = upgradesUI.UpgradeButtonPrefab;
            _upgradeButtonsContainer = upgradesUI.UpgradesContainer;

            CreateButtonsForAllUpgrades();
        }

        private void CreateButtonsForAllUpgrades()
        {
            ClearContainer();
            
            foreach (var upgrade in _upgradesService.ExistingUpgrades)
            {
                UpgradeButtonUI button = InstantiateButton(upgrade);
                var buttonData = new UpgradeButtonData(button, upgrade, upgrade.ResourceType);
                _buttons.Add(buttonData);
                _upgradeToButtonDictionary.Add(upgrade, buttonData);
            }
        }

        private void ClearContainer()
        {
            for (int i = 0; i < _upgradeButtonsContainer.childCount; i++)
            {
                Object.Destroy(_upgradeButtonsContainer.GetChild(i).gameObject);
            }
        }

        private UpgradeButtonUI InstantiateButton(Upgrade upgrade)
        {
            UpgradeButtonUI button = Object.Instantiate(_upgradeButtonPrefab, _upgradeButtonsContainer);
            button.Name = upgrade.UpgradeType.ToString();
            button.Icon = _upgradesService.GetConfigOfUpgrade(upgrade).Icon;
            button.Price = upgrade.Price;
            button.ResourceIcon = _resourcesService.GetIconForResource(upgrade.ResourceType);
            button.Level = upgrade.Level;
            button.Value = upgrade.Value;
            ValidateIfButtonIsPurchasable(button, upgrade);
            button.Clicked += () =>
            {
                var price = upgrade.Price;
                if (_resourcesService.HasEnough(upgrade.ResourceType, price)
                    && _upgradesService.TryLevelUpUpgrade(upgrade))
                {
                    _resourcesService.Spend(upgrade.ResourceType, price);
                    _soundsService.PlayPositiveClick();
                }
                else
                {
                    _soundsService.PlayNegativeClick();
                }
            };
            return button;
        }
        private void ValidateButtonWithUpgrade(Upgrade upgrade)
        {
            UpgradeButtonUI button = _upgradeToButtonDictionary[upgrade].Button;
            button.Level = upgrade.Level;
            button.Value = upgrade.Value;
            button.Price = upgrade.Price;
            ValidateIfButtonIsPurchasable(button, upgrade);
        }

        private void ValidateButtonsWithResourceType(ResourceType resourceType, int amount)
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (_buttons[i].ResourceType == resourceType)
                {
                    UpgradeButtonUI button = _buttons[i].Button;
                    Upgrade upgrade = _buttons[i].Upgrade;
                    ValidateIfButtonIsPurchasable(button, upgrade);
                }
            }
        }

        private void ValidateIfButtonIsPurchasable(UpgradeButtonUI button, Upgrade upgrade)
        {
            button.CanUpgrade = !upgrade.ReachedMax && 
                                _resourcesService.HasEnough(upgrade.ResourceType, upgrade.Price);
        }

    }
}
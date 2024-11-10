using System;
using System.Collections.Generic;
using Tap_a_Tree.Environment;
using Tap_a_Tree.Player.Resources;
using Tap_a_Tree.Player.Upgrades;
using Tap_a_Tree.UI;
using Tap_a_Tree.UI.Controllers;
using UnityEngine;

namespace Tap_a_Tree.Core
{
    public class GameStartup : MonoBehaviour
    {
        [Header("Game Setup")]
        [SerializeField] private float _prespawnDistance = 5f;
        [Header("Scene Objects")]
        [SerializeField] private SoundsService _soundsService;
        [SerializeField] private EnvironmentSetup _environment;
        [SerializeField] private FoliageScroller _foliageScroller;
        [SerializeField] private WorldScroller _worldScroller;
        [SerializeField] private Character _playerCharacter;
        [Header("UI")]
        [SerializeField] private GameUI _ui;
        [Header("Configs")]
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private ResourcesIconsConfig _resourcesIconsConfig;
        [SerializeField] private List<UpgradeConfig> _playerUpgradesConfigs;

        private WorldScrollService _worldScrollService;
        private FoliageSpawner _foliageSpawner;
        private PlayerUpgradesService _playerUpgradesService;
        private PlayerResourcesService _playerResourcesService;
        private GameUIController _uiController;


        [RuntimeInitializeOnLoadMethod]
        private static void GameInitializeSetups()
        {
            Application.targetFrameRate = 60;
        }

        public void Awake()
        {
            _playerUpgradesService = new PlayerUpgradesService(_playerUpgradesConfigs);
            _playerResourcesService = new PlayerResourcesService(_resourcesIconsConfig);
            
            _foliageSpawner = new FoliageSpawner(
                _gameConfig,
                _environment.SpawnContainer,
                _environment.TreePrefab
            );
            
            _worldScrollService = new WorldScrollService(_playerUpgradesService);

            _worldScroller.Construct(_worldScrollService);
            
            _foliageScroller.Construct(
                _gameConfig,
                _foliageSpawner, _worldScrollService,
                _environment.WorldSpawnPoint,
                _environment.WorldDespawnPoint
            );
            
            _playerCharacter.Construct(
                _worldScrollService,
                _playerUpgradesService,
                _playerResourcesService,
                _soundsService
            );
            
            _uiController = new GameUIController(
                _ui, 
                _playerCharacter,
                _playerUpgradesService,
                _playerResourcesService,
                _soundsService
            );
        }

        private void Start()
        {
            _soundsService.LaunchEnvironmentSound();
            _foliageScroller.ForceScroll(_prespawnDistance);
            _worldScrollService.WorldScrollState = WorldScrollState.Scrolling;
        }
    }
}
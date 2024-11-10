using Tap_a_Tree.Core;
using Tap_a_Tree.Player.Resources;
using Tap_a_Tree.Player.Upgrades;

namespace Tap_a_Tree.UI.Controllers
{
    public class GameUIController
    {
        private readonly Character _character;
        private readonly GameUI _ui;

        private UpgradesUIController _upgradesUIController;
        private ResourcesUIController _resourcesUIController;

        public GameUIController(GameUI ui, Character character, PlayerUpgradesService upgradesService,
            PlayerResourcesService resourcesService, SoundsService soundsService)
        {
            _upgradesUIController = new UpgradesUIController(ui.UpgradesUI, upgradesService, resourcesService, soundsService);
            _resourcesUIController = new ResourcesUIController(ui.ResourcesUI, resourcesService, soundsService);

            _character = character;
            _ui = ui;

            _ui.ScreenTapped += OnTap;

            _character.HittedTree += OnTreeHit;
        }

        private void OnTap()
        {
            _character.TryChop();
        }

        private void OnTreeHit(Tree tree)
        {
            HealthBarUI healthBar = _ui.HealthBarUI;
            
            _ui.SetScreenPositionFromWorld(healthBar.RectTransform, tree.HealthbarWorldPosition);
            healthBar.TargetHealth = tree;
        }
    }
}
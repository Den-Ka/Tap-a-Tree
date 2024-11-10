using Tap_a_Tree.Player.Upgrades;
using UnityEngine;

namespace Tap_a_Tree
{
    [RequireComponent(typeof(Character))]
    public class AutoChopping : MonoBehaviour
    {
        [SerializeField] private Character _character;

        private float _choppingSpeed;
        private float _delayBetweenChops;
        private float _chopTimer;

        private bool _isReadyToChop;

        public void Construct(PlayerUpgradesService _upgradesService)
        {
            _upgradesService.UpgradeLeveledUp += OnUpgradeLeveledUp;
            ValidateChoppingSpeed(_upgradesService[UpgradeType.AutoClick].Value);
        }

        private void Update()
        {
            if (_choppingSpeed == 0) return;

            if (_chopTimer > 0)
            {
                _chopTimer -= Time.deltaTime;
                return;
            }
            else if (_character.TryChop())
            {
                _chopTimer = _delayBetweenChops;
            }
        }

        private void OnUpgradeLeveledUp(Upgrade upgrade)
        {
            if (upgrade.UpgradeType == UpgradeType.AutoClick) ValidateChoppingSpeed(upgrade.Value);
        }

        private void ValidateChoppingSpeed(float choppingSpeed)
        {
            _choppingSpeed = choppingSpeed;
            _delayBetweenChops = _choppingSpeed == 0f ? 0f : 1f / _choppingSpeed;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _character = GetComponent<Character>();
        }
#endif
    }
}
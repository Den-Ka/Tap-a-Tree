using UnityEngine;

namespace Tap_a_Tree.UI
{
    public class UpgradesUI : MonoBehaviour
    {
        [field: SerializeField] public UpgradeButtonUI UpgradeButtonPrefab { get; private set; }
        [field: SerializeField] public RectTransform UpgradesContainer { get; private set; }
    }
}
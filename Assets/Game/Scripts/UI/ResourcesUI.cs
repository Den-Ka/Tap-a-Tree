using UnityEngine;
using UnityEngine.UI;

namespace Tap_a_Tree.UI
{
    public class ResourcesUI : MonoBehaviour
    {
        [field: SerializeField] public ResourceContainerUI ResourceContainerPrefab { get; private set; }
        [field: SerializeField] public RectTransform ResourcesContainer { get; private set; }
        [field: Space]
        [field: SerializeField] public ResourceContainerUI ScoresContainerUI { get; private set; }
        [field: Space]
        [field: SerializeField] public Image DropPrefab { get; private set; }
        [field: SerializeField] public RectTransform DropContainer { get; private set; }
        
    }
}
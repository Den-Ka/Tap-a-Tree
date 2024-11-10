using DG.Tweening;
using Tap_a_Tree.Player;
using Tap_a_Tree.Player.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tap_a_Tree.UI
{
    public class ResourceContainerUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _amountText;

        [SerializeField] private ResourceType _resourceType;

        public Sprite Icon
        {
            get => _icon.sprite;
            set => _icon.sprite = value;
        }

        public int Value
        {
            set => _amountText.text = value.ToString("N0");
        }
        public Vector3 IconPosition => _icon.transform.position;
        

        public void Animate()
        {
            _amountText.transform.DOComplete();
            _amountText.transform.DOPunchScale(Vector3.one, 0.2f);
        }
    }
}
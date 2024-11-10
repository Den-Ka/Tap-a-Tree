using System;
using System.ComponentModel;
using Tap_a_Tree.Player.Resources;
using Tap_a_Tree.Player.Upgrades;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tap_a_Tree.UI
{
    public class GameUI : MonoBehaviour
    {
        public event Action ScreenTapped;

        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private Button _tapButton;
        [field: Space]
        [field: SerializeField] public HealthBarUI HealthBarUI { get; private set; }
        [field: SerializeField] public UpgradesUI UpgradesUI { get; private set; }
        [field: SerializeField] public ResourcesUI ResourcesUI { get; private set; }

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _tapButton.onClick.AddListener(OnTap);
        }

        private void OnTap()
        {
            ScreenTapped?.Invoke();
        }

        public Vector2 WorldToCanvasPoint(Vector3 worldPosition)
        {
            var screenPoint = _camera.WorldToScreenPoint(worldPosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform,
                screenPoint,
                _camera,
                out var localPoint
            );
            return localPoint;
        }

        public void SetScreenPositionFromWorld(RectTransform transform, Vector3 worldPosition)
        {
            Vector2 screenPoint = WorldToCanvasPoint(worldPosition);
            transform.localPosition = screenPoint;
        }
    }
}
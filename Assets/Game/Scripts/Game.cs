using System;
using UnityEngine;

namespace Tap_a_Tree
{
    public class Game : MonoBehaviour
    {
        public event Action<float> PlayerSpeedChanged;
        public event Action PlayerStartedMoving;
        public event Action PlayerStoppedMoving;

        [field: SerializeField] public float _playerSpeed { get; private set; }
        public static Game Instance { get; private set; }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError($"There is more than one instance of {nameof(Game)}!", this);
                Destroy(this);
            }

            _player.IsMoving = true;
        }

        [SerializeField] private Character _player;


#if UNITY_EDITOR
        private void OnValidate()
        {
        }
#endif
    }
}
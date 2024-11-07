using UnityEditor.Animations;
using UnityEngine;

namespace Tap_a_Tree
{
    [RequireComponent(typeof(AnimatorController))]
    public class Character : MonoBehaviour
    {
        private static readonly int MovingStringHash = Animator.StringToHash("IsMoving");
        private static readonly int ChopStringHash = Animator.StringToHash("Chop");

        [SerializeField] private Animator _animator;

        public bool IsMoving
        {
            set => _animator.SetBool(MovingStringHash, value);
        }

        public float MoveSpeed { get; set; }

        public void Start()
        {
            Game.Instance.PlayerStartedMoving += OnPlayerStartedMoving;
            Game.Instance.PlayerStoppedMoving += OnPlayerStoppedMoving;
        }


        private void OnPlayerStartedMoving() => IsMoving = true;
        private void OnPlayerStoppedMoving() => IsMoving = false;

        public void Chop()
        {
            _animator.SetTrigger(ChopStringHash);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _animator = GetComponent<Animator>();
        }
#endif
    }
}
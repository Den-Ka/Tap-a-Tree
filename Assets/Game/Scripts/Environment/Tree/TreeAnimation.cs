using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tap_a_Tree
{
    public class TreeAnimation : MonoBehaviour
    {
        [SerializeField, Range(0f, 10f)] private float _strenght = 3f;
        [SerializeField, Range(0f, 1f)] private float _duration = 0.1f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField, Range(0f, 90f)] private float _randomness = 30f;
        [Space]
        [SerializeField, Range(-90f, 90f)] private float _deathPositionAngle = 90f;
        [SerializeField, Min(0f)] private float _deathDuration = 0.5f;
        
        public void PlayChopAnimation()
        {
            transform.DOComplete();
            transform.DOShakeRotation(_duration, Vector3.forward * _strenght, _vibrato, _randomness, true, ShakeRandomnessMode.Harmonic);
        }

        public void PlayDeathAnimation()
        {
            var deathPosition = new Vector3(0, 0, _deathPositionAngle);
            transform.DOComplete();
            transform.DORotate(deathPosition, _deathDuration).SetEase(Ease.OutBounce);
        }

        public void ResetAnimations()
        {
            transform.DOKill();
            transform.rotation = Quaternion.identity;
        }
    }
}
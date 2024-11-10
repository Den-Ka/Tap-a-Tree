using UnityEngine;
using UnityEngine.Serialization;

namespace Tap_a_Tree.Core
{
    public class SoundsService : MonoBehaviour
    {
        [FormerlySerializedAs("_audioSource")] [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _environmentSource;
        [Space]
        [SerializeField] private AudioClip _chopSound;
        [SerializeField] private AudioClip _treeChoppedSound;
        [SerializeField] private AudioClip _dropPickedUpSound;
        [SerializeField] private AudioClip _positiveClickSound;
        [SerializeField] private AudioClip _negativeClickSound;
        [Space]
        [SerializeField] private AudioClip _environmentSound;
        
        public void PlayChopSound()
        {
            _sfxSource.PlayOneShot(_chopSound);
        }

        public void PlayTreeChoppedSound()
        {
            _sfxSource.PlayOneShot(_treeChoppedSound);
        }

        public void PlayDropPickedUpSound()
        {
            _sfxSource.PlayOneShot(_dropPickedUpSound);
        }
        
        public void PlayPositiveClick()
        {
            
        }

        public void PlayNegativeClick()
        {
            
        }

        public void LaunchEnvironmentSound()
        {
            if(_environmentSource.isPlaying) return;
            
            _environmentSource.loop = true;
            _environmentSource.clip = _environmentSound;
            _environmentSource.Play();
        }
    }
}
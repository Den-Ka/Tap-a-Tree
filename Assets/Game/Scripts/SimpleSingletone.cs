using UnityEngine;

namespace Tap_a_Tree.Player
{
    public abstract class SimpleSingleTone<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);
                    (_instance as SimpleSingleTone<T>)?.Initialize();
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                Initialize();
            }
            else if(_instance != this)
            {
                Debug.LogWarning($"There is more than one instance of {typeof(T).Name}.");
                Destroy(this);
            }
        }

        protected void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        protected abstract void Initialize();
    }
}
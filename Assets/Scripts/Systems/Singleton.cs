using UnityEngine;

namespace Systems
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                // Find existing instance if one exists in the scene
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                // Log an error if an instance is still not found
                if (_instance == null)
                {
                    Debug.LogError("No instance of " + typeof(T).Name + " found in the scene.");
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            // Destroy this instance if an instance already exists
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject); // Optional: makes the object not be destroyed when loading a new scene
            }
        }
    }
}
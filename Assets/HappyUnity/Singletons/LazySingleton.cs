using UnityEngine;

namespace HappyUnity.Singletons
{
    public class LazySingleton<T> : MonoBehaviour where T : LazySingleton<T>
    {
        private static T instance;

        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();
                        if (instance == null)
                        {
                            var singleton = new GameObject
                            {
                                name = typeof(T).ToString()
                            };
                            instance = singleton.AddComponent<T>();

                            Debug.Log("[Singleton] An instance of " + typeof(T) +
                                      " is needed in the scene, so '" + singleton +
                                      "' was created.");
                        }
                    }

                    return instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (instance == null)
            {
                instance = this as T;
            }
            else //singleton already exists
            {
                Destroy(gameObject);
            }
        }
    }

}


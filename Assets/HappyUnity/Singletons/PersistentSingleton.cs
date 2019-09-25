using UnityEngine;

namespace HappyUnity.Singletons
{
    public class PersistentSingleton<T> : MonoBehaviour where T : PersistentSingleton<T>
    {
        private static T instance;
        
        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(transform.gameObject);
                SingletonAwake();
            }
            else
            {
                //Singleton already exists
                Destroy(gameObject);
            }
        }

        protected virtual void SingletonAwake()
        {
        }
    }
}
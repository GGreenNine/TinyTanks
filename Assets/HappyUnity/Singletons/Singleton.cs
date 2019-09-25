using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace HappyUnity.Singletons
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }
        protected virtual void OnEnable()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            if (Instance == null)
            {
                Instance = this as T;
                Debug.Log(" Is loaded \t" + typeof(T));
            }
        }

        protected virtual void OnDisable()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (Instance == null)
            {
                Instance = this as T;
            }
            else 
            {
                Destroy(gameObject);
                Debug.Log($"This singleton has already exists {typeof(T)}");
            }
        }
    }
}

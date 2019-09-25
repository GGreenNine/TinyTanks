using UnityEngine;
using UnityEngine.EventSystems;
using System;
using HappyUnity.Spawners;

namespace HappyUnity.Spawners.ObjectPools
{
    [Serializable]
    [ExecuteInEditMode]
    public sealed class Poolable : Parkable, IRecyclable
    {
        [SerializeField] [HideInInspector] ObjectPool pool;

        static bool scriptBuiltInstance;
        public string poolableId;
        
        void Awake()
        {
            InstantiationGuard();
            ExecuteEvents.Execute<IPoolableAware>(gameObject, null, (script, ignored) => script.PoolableAwoke(this));
        }

        void InstantiationGuard()
        {
            if (!scriptBuiltInstance)
            {
                DestroyImmediate(this, true);
                throw new InvalidOperationException("Can only be created with AddPoolableComponent");
            }

            scriptBuiltInstance = false;
        }

        void OnEnable()
        {
            gameObject.hideFlags = 0;
        }

        void OnDisable()
        {
            //gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        public void Recycle()
        {
            pool.Recycle(this);
        }

        public static Poolable AddPoolableComponent(GameObject newInstance, ObjectPool pool, string id)
        {
            scriptBuiltInstance = true;
            var instance = newInstance.AddComponent<Poolable>();
            instance.poolableId = id;
            instance.pool = pool;
            return instance;
        }

    }

//============================================================================

    public interface IPoolableAware : IEventSystemHandler
    {
        void PoolableAwoke(Poolable p);
    }

//============================================================================

    public interface IRecyclable
    {
        void Recycle();
    }

//============================================================================

    public abstract class Parkable : MonoBehaviour
    {
        public virtual void Park()
        {
            gameObject.SetActive(false);
        }

        public virtual void Unpark()
        {
            gameObject.SetActive(true);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniBehaviours
{
    public class DamageOnTouch : MonoBehaviour
    {
        private Health _health;
        private GameBehaviour _gameBehaviour;
        private Collider2D _collider2D;
        
        public LayerMask TargetLayerMask;
        public int damage_OnTouch = 10;
        public int damage_OnCollideWithNonDamageable = 0;
        public float immuneToDamageDuration = 0.5f;
        public Vector2 knockbackForce = new Vector2(10,10);
        
        protected Vector3 _collisionPoint;
        protected Health _colliderHealth;
        protected List<GameObject> _ignoredGameObjects = new List<GameObject>();
        
        public static bool LayerInLayerMask(int layer, LayerMask layerMask)
        {
            return ((1 << layer) & layerMask) != 0;
        }

        private void Start()
        {
            _health = GetComponent<Health>();
        }

        /// <summary>
        /// On trigger enter 2D, we call our colliding endpoint
        /// </summary>
        /// <param name="collider"></param>S
        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            Colliding(collider.gameObject);
        }
        
        /// <summary>
        /// When a collision with the player is triggered, we give damage to the player and knock it back
        /// </summary>
        /// <param name="collider">what's colliding with the object.</param>
        public virtual void OnTriggerStay2D(Collider2D collider)
        {
            Colliding(collider.gameObject);
        }

        
        protected virtual void Colliding(GameObject collider)
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            // if the object we're colliding with is part of our ignore list, we do nothing and exit
            if (_ignoredGameObjects.Contains(collider))
            {
                return;
            }

            // if what we're colliding with isn't part of the target layers, we do nothing and exit
            if (!LayerInLayerMask(collider.layer, TargetLayerMask))
            {
                return;
            }

            _collisionPoint = this.transform.position;
            _colliderHealth = collider.gameObject.GetComponent<Health>();

            // if what we're colliding with is damageable
            if (_colliderHealth != null)
            {
                if (_colliderHealth.CurrentHP > 0)
                {
                    OnCollideWithDamageable(_colliderHealth);
                }
            }

            // if what we're colliding with can't be damaged
            else
            {
                OnCollideWithNonDamageable();
            }
        }

        public void OnCollideWithDamageable(Health health)
        {
            var colliderGameBehaviour = health.GetComponent<GameUnit>();
            var colliderRigitbody = health.GetComponent<Rigidbody2D>();
            
            _colliderHealth.Damage(damage_OnTouch, immuneToDamageDuration);

            if (colliderRigitbody != null && colliderGameBehaviour!=null && colliderGameBehaviour.isAbleToKnockBack)
            {
                colliderRigitbody.AddForce(transform.up* knockbackForce.x);
            }
            
            
        }

        public void OnCollideWithNonDamageable()
        {
            SelfDamage();
        }

        public void SelfDamage()
        {
            if (damage_OnCollideWithNonDamageable > 0)
            {
                _health.Damage(damage_OnCollideWithNonDamageable, immuneToDamageDuration);
            }
        }
        
        
        
        /// <summary>
        /// Adds the gameobject set in parameters to the ignore list
        /// </summary>
        /// <param name="newIgnoredGameObject">New ignored game object.</param>
        public virtual void IgnoreGameObject(GameObject newIgnoredGameObject)
        {
            _ignoredGameObjects.Add(newIgnoredGameObject);
        }

        /// <summary>
        /// Removes the object set in parameters from the ignore list
        /// </summary>
        /// <param name="ignoredGameObject">Ignored game object.</param>
        public virtual void StopIgnoringObject(GameObject ignoredGameObject)
        {
            _ignoredGameObjects.Remove(ignoredGameObject);
        }
        
    }
}
using System;
using Game_Basics;
using UnityEngine;

namespace MiniBehaviours
{
    public class AutoRespawn : MonoBehaviour
    {
        public float autoRespawnDuration = 0f;
        public GameObject respawnEffect;

        private bool _reviving;
        private GameUnit _gameUnit;
        private float _timeOfDeath = 0f;

        
        private void Awake()
        {
            _gameUnit = GetComponent<GameUnit>();
        }

        public virtual void Kill()
        {
            RespawnManager.Instance.AddRespawnable(this);
        }
        
        /// <summary>
        /// Instantiates the respawn effect at the object's position
        /// </summary>
        protected virtual void InstantiateRespawnEffect()
        {
            // instantiates the destroy effect
            if (respawnEffect != null)
            {
                GameObject instantiatedEffect=(GameObject)Instantiate(respawnEffect,transform.position,transform.rotation);
                instantiatedEffect.transform.localScale = transform.localScale;
            }
        }
        /// <summary>
        /// Plays the respawn sound.
        /// </summary>
        protected virtual void PlayRespawnSound()
        {
                //GetComponent<AudioSource>().Play();
        }

        public void Revive()
        {
            if (!_gameUnit)
                return;
            _gameUnit.Spawn();
            InstantiateRespawnEffect();
            PlayRespawnSound();
        }
    }
}
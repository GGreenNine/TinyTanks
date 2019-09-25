using System;
using System.Collections;
using System.Collections.Generic;
using HappyUnity.Singletons;
using MiniBehaviours;
using UnityEngine;

namespace Game_Basics
{
    public class RespawnManager : Singleton<RespawnManager>
    {
        private Queue<AutoRespawn> queueTorRespawn = new Queue<AutoRespawn>();

        public void AddRespawnable(AutoRespawn respawn)
        {
            queueTorRespawn.Enqueue(respawn);
        }

        private void Start()
        {
            StartCoroutine(RespawnWorkflow());
        }

        public IEnumerator RespawnWorkflow()
        {
            while (true)
            {
                if (queueTorRespawn.Count <= 0)
                    yield return null;
                foreach (var respawn in queueTorRespawn)
                {
                    StartCoroutine(AutoRespawnUnit(respawn));
                }
                queueTorRespawn.Clear();
            }
        }

        private IEnumerator AutoRespawnUnit(AutoRespawn respawn)
        {
            yield return new WaitForSeconds(respawn.autoRespawnDuration);
            respawn.gameObject.SetActive(true);
            respawn.Revive();
        }
    }
}
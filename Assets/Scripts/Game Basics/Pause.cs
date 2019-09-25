using System;
using System.Collections;
using HappyUnity.Singletons;
using UnityEngine;

namespace Asteroids
{
    public class Pause : LazyPersistentSingleton<Pause>
    {
        private static bool IsPaused = false;
        public static WaitForSeconds Long()
        {
            return new WaitForSeconds(2f);
        }

        public static WaitForSeconds Brief()
        {
            return new WaitForSeconds(1f);
        }

        public static void Set()
        {
            IsPaused = true;
        }

        public static void Unset()
        {
            IsPaused = false;
        }

        private void Update()
        {
            if(!IsPaused) return;
            if(Input.GetKey(KeyCode.Escape))
                GameManager.Instance.SetGameState(GameManager.GameState.Quit);
            if(Input.GetKey(KeyCode.Space))
                GameManager.Instance.SetGameState(GameManager.GameState.Playing);
        }
    }
}
using Asteroids;
using HappyUnity.Singletons;
using UnityEngine;

namespace Game_Basics
{
    public class UIManager : Singleton<UIManager>
    {
        public ProgressBar playerBar;

        /// <summary>
        /// Updates the health bar.
        /// </summary>
        /// <param name="currentHealth">Current health.</param>
        /// <param name="minHealth">Minimum health.</param>
        /// <param name="maxHealth">Max health.</param>
        /// <param name="playerID">Player I.</param>
        public virtual void UpdatePlayerBar(float currentHealth, float minHealth, float maxHealth)
        {
            if (playerBar == null)
            {
                return;
            }

            playerBar.UpdateBar(currentHealth, minHealth, maxHealth);
        }
    }
}
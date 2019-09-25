using UnityEngine;

namespace Asteroids
{
    public class Score
    {
        public delegate void Score_Added(int count);

        public static event Score_Added OnScoreAded;

        public static int Total_Score_Earned { get; private set; }

        private static void OnOnScoreAded(int count)
        {
            OnScoreAded?.Invoke(count);
        }
        
        /// <summary>
        /// Adds value to the total score
        /// </summary>
        /// <param name="points"></param>
        public static void Earn(int points)
        {
            Total_Score_Earned += points;
            OnOnScoreAded(points);
        }
        
        /// <summary>
        /// Sets the total score to zero
        /// </summary>
        public static void Reset()
        {
            Total_Score_Earned = 0;
            OnOnScoreAded(0);
        }
    }
}
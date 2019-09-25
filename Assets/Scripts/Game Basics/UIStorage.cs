using System;
using HappyUnity.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids
{
    public class UIStorage : Singleton<UIStorage>
    {
        public TextMeshProUGUI mainscreen_Text;
        public ScoreUI ScoreUi;
        
        private void Awake()
        {
            ScoreUi.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ScoreUi.OnEnable();
        }

        [Serializable]
        public class ScoreUI
        {
            [SerializeField]
            Color textColor = Color.white;
            private GameScreenLogger scoreLogger;
            [SerializeField]
            private TextMeshProUGUI scoreText;
            
            public void Awake()
            {
                scoreLogger = GameScreenLogger.New(scoreText);
                scoreText.color = textColor;
            }
            
            public void OnEnable()
            {
                scoreLogger.PushTextMessage(scoreText, Score.Total_Score_Earned.ToString());
                Score.OnScoreAded += ScoreEarned;
            }
            
            void ScoreEarned(int points)
            {
                scoreLogger.PushTextMessage(scoreText, Score.Total_Score_Earned.ToString());
            }
            
            void Reset()
            {
                scoreLogger = GameScreenLogger.New(scoreText);
                scoreLogger.Clear_Text();
            }
        }
        
        
    }
}
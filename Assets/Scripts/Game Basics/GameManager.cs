using System;
using System.Collections;
using System.Collections.Generic;
using HappyUnity.Cameras;
using HappyUnity.Singletons;
using HappyUnity.Spawners;
using HappyUnity.Spawners.ObjectPools;
using HappyUnity.TransformUtils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class GameManager : Singleton<GameManager>
    {
        public GameObject Tank_Prefab;
        public GameObject Enemy_Rd2d;
        public GameObject Enemy_Chicken;
        public GameObject Enemy_Robox25;

        public GameField GameField;

        private GameScreenLogger gameScreenLogger;
        private FollowingCamera2D _followingCamera2D;
        TinyTank tank;

        private ObjectPool _chicken_Pool;
        private ObjectPool _r2d2_Pool;
        private ObjectPool _robox25_Pool;

        private List<ObjectPool> EnemyPools = new List<ObjectPool>();

        public enum GameState
        {
            Pause,
            Playing,
            End,
            Quit
        }

        bool requestTitleScreen = true;
        public static GameState CurrentGameState;

        public void SetGameState(GameState state)
        {
            switch (state)
            {
                case GameState.Quit:
                    Application.Quit();
                    break;
                case GameState.Pause:
                    Pause.Set();
                    break;
                case GameState.Playing:
                    Pause.Unset();
                    break;
                case GameState.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            CurrentGameState = state;
        }

        private void Awake()
        {
            _r2d2_Pool = ObjectPool.Build(Enemy_Rd2d, 25, 25);
            _chicken_Pool = ObjectPool.Build(Enemy_Chicken, 25, 25);
            _robox25_Pool = ObjectPool.Build(Enemy_Robox25, 25, 25);
            _followingCamera2D = Camera.main.GetComponent<FollowingCamera2D>();

            EnemyPools.Add(_r2d2_Pool);
            EnemyPools.Add(_chicken_Pool);
            EnemyPools.Add(_robox25_Pool);

            gameScreenLogger = GameScreenLogger.New(UIStorage.Instance.mainscreen_Text);
            //AsteroidsManager.New(_bigAsteroidsPool, _smallAsteroidsPool);
        }

        private void Start()
        {
            tank = TinyTank.Create(Tank_Prefab);
            tank._health.On_Death += HealthOnOnDeath;
            tank.RemoveFromGame();
            TinyTank.OnTankDeath += delegate { SetGameState(GameState.End); };
            StartCoroutine(Background_Game_Workflow());
        }

        private void HealthOnOnDeath()
        {
            SetGameState(GameState.End);
        }

        /// <summary>
        /// Works in backgroud, changing the game Behaviour
        /// for current game state
        /// </summary>
        /// <returns></returns>
        IEnumerator Background_Game_Workflow()
        {
            while (true)
            {
                if (requestTitleScreen)
                {
                    requestTitleScreen = false;
                    yield return StartCoroutine(ShowingPreGameTitle());
                }

                yield return StartCoroutine(StartLevel());
                yield return StartCoroutine(PlayLevel());
                yield return StartCoroutine(EndLevel());
                GC.Collect();
            }
        }

        IEnumerator ShowingPreGameTitle()
        {
            SetGameState(GameState.Pause);
            gameScreenLogger.Show_StartTitle();
            //AsteroidsManager.Instance.ShowAsteroids();
            while (CurrentGameState != GameState.Playing) yield return null;
            //AsteroidsManager.Instance.HideAsteroids();
        }

        IEnumerator StartLevel()
        {
            _followingCamera2D.SetTarget(tank.transform);
            tank.Spawn();
            tank.EnableControls();
            gameScreenLogger.Clear_Text();
            yield return Pause.Long();
            SpawnEnemies(10);
        }

        IEnumerator PlayLevel()
        {
            SetGameState(GameState.Playing);
            gameScreenLogger.Clear_Text();
            while (CurrentGameState == GameState.Playing)
            {
                yield return null;
            }
            DefineNewGame();
        }

        IEnumerator EndLevel()
        {
            gameScreenLogger.Show_GameOverTitle();
            RemoveRemainingGameTokens();
            Score.Reset();
            yield return Pause.Brief();
            gameScreenLogger.Clear_Text();
            yield return Pause.Long();
        }

        void RemoveRemainingGameTokens()
        {
            foreach (var a in FindObjectsOfType<GameUnit>())
            {
                a.RemoveFromGame();
            }
        }

        void DefineNewGame()
        {
            requestTitleScreen = true;
        }

        void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ObjectPool pool = EnemyPools[Random.Range(0, EnemyPools.Count - 1)];
                var enemy = pool.GetRecyclable<EnemyUnit>();
                enemy.Spawn();
            }
        }
    }
}
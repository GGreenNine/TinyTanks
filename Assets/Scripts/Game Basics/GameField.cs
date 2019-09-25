using System;
using System.Collections.Generic;
using HappyUnity.Singletons;
using HappyUnity.TransformUtils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class GameField : Singleton<GameField>
    {
        public Collider2D top;
        public Collider2D bot;
        public Collider2D right;
        public Collider2D left;

        public Transform[] spawnPoints;
        
        public static List<Vector2> Spawn_TopPoints = new List<Vector2>();
        public static List<Vector2> Spawn_BottomPoints = new List<Vector2>();
        public static List<Vector2> Spawn_LeftPoints = new List<Vector2>();
        public static List<Vector2> Spawn_RightPoints = new List<Vector2>();
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            foreach (Vector2 point in Spawn_TopPoints)
            {
                Gizmos.DrawSphere(new Vector2(point.x, point.y + top.offset.y - 2), 1);
            }

            Gizmos.color = Color.red;

            foreach (Vector2 point in Spawn_BottomPoints)
            {
                Gizmos.DrawSphere(point, 1);
            }

            Gizmos.color = Color.yellow;

            foreach (Vector2 point in Spawn_LeftPoints)
            {
                Gizmos.DrawSphere(point, 1);
            }

            Gizmos.color = Color.green;

            foreach (Vector2 point in Spawn_RightPoints)
            {
                Gizmos.DrawSphere(point, 1f);
            }
        }

        public static Vector2 GetRandomSpawnPointPosition()
        {
            var randomPoint = Random.Range(0, Instance.spawnPoints.Length);
            return Instance.spawnPoints[randomPoint].position;
        }


        /// <summary>
        /// Getting random position in the game field
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetRandomSpawnPosition()
        {
            var randomSide = Random.Range(0, 3);
            LayerMask layer = LayerMask.GetMask("Enemy");
            
            switch (randomSide)
            {
                case 0:
                    return RandomCirclePositionGeneration.GetrandomPointFromBounds(Instance.top.bounds, 6f,layerMask: layer);
                    break;
                case 1:
                    return RandomCirclePositionGeneration.GetrandomPointFromBounds(Instance.bot.bounds, 6f,layerMask: layer);
                    break;
                case 2:
                    return RandomCirclePositionGeneration.GetrandomPointFromBounds(Instance.left.bounds, 6f,layerMask: layer);
                    break;
                case 3:
                    return RandomCirclePositionGeneration.GetrandomPointFromBounds(Instance.right.bounds, 6f,layerMask: layer);
                    break;
            }

            throw new Exception("Не удалось получить место спауна");
        }
    }

    public static class Viewport
    {
        public static Vector2 GenerateWorld(Vector3 viewPoint)
        {
            if (Camera.main == null) throw new NullReferenceException("Шо камера то не нужна?");
            var position = Camera.main.transform.position;
            return new Vector2(position.x * 2, position.y * 2);
        }
    }
}
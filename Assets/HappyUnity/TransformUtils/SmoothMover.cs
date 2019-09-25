using HappyUnity.Math;
using UnityEngine;

namespace HappyUnity.TransformUtils
{
    public class SmoothMover : MonoBehaviour
    {
        public Transform TargetPosition { get; set; }
        public float smoothTime;
        public float maxSpeed;
       
        private Vector3 velocity = Vector3.zero;
        private bool _moving;
        public bool facing;
        public bool stopWhenReach;
        
        public float endPrecision = 0.0001f;
        public bool useFixedDeltaTime;

        public void BeginMoving(bool stopWhenReach = true)
        {
            this.stopWhenReach = stopWhenReach;
            _moving = true;
        }

        public void StopMoving()
        {
            _moving = false;
            velocity = Vector3.zero;
        }


        /// <summary>
        /// Should be played in Update mode
        /// </summary>
        public void Move()
        {
            if (_moving)
            {
                if (TargetPosition.position.PrecisionEquals(transform.position, endPrecision))
                {
                    transform.position = TargetPosition.position;
                    velocity = Vector3.zero;
                    if (stopWhenReach)
                    {
                        _moving = false;
                    }
                }
                else
                {
                    Vector3 newPosition = Vector3.SmoothDamp(transform.position, TargetPosition.position, ref velocity,
                        smoothTime,
                        maxSpeed, useFixedDeltaTime ? Time.fixedDeltaTime : Time.deltaTime);
                    transform.position = newPosition;
                }
            }
        }
    }
}
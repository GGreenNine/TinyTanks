using System;
using CryoDI;
using Game_Basics;
using UnityEngine;

namespace MiniBehaviours
{
    public class TankMovement : CryoBehaviour
    {
        public float maxSpeed = 300f;
        public float thrust = 1000f;
        public float torque = 500f;
        private float thrustInput;
        private float torqueInput;
        private Rigidbody2D rb;

        [TypeDependency(typeof(TankMovement))] private IController TankInput { get; set; }

        private void Move()
        {
            Vector3 thrustForce = thrustInput * Time.deltaTime * thrust * transform.up;
            rb.AddForce(thrustForce);
        }

        private void Turn()
        {
            var angles = transform.rotation.eulerAngles;
            angles.z += -Time.deltaTime * 10 * torqueInput * torque;
            transform.rotation = Quaternion.Euler(angles);
        }

        private void Reset()
        {
            thrustInput = 0f;
        }

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody2D>();
        }

        private void ClampSpeed()
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        private void OnEnable()
        {
            Reset();
        }

        private void Update()
        {
            thrustInput = TankInput.GetForwardAxis;
            torqueInput = TankInput.GetTurnAxis;
        }

        private void FixedUpdate()
        {
            Move();
            Turn();
            ClampSpeed();
        }
    }
}
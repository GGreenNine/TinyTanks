using System;
using UnityEngine;

namespace MiniBehaviours
{
    public class TankMovement : MonoBehaviour
    {
        public float maxSpeed = 300f;
        public float thrust = 1000f;
        public float torque = 500f;
        private float thrustInput;
        private float torqueInput;
        private Rigidbody2D rb;

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

        private void Awake()
        {
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
            thrustInput = TankInput.GetForwardThrust();
            torqueInput = TankInput.GetTurnAxis();
        }

        private void FixedUpdate()
        {
            Move();
            Turn();
            ClampSpeed();
        }
    }
}
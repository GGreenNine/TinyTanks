using Game_Basics;
using UnityEngine;

namespace MiniBehaviours
{
    public class TankInput : IController
    {
        public bool IsShooting => Input.GetButtonDown("Fire1");

        public bool ChangeWeapon => (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E));

        public float GetTurnAxis => Input.GetAxis("Horizontal");

        public float GetForwardAxis
        {
            get
            {
                float axis = Input.GetAxis("Vertical");
                return Mathf.Clamp(axis, -1, 1);
            }
        }
    }
}
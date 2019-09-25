using System;
using UnityEngine;

namespace MiniBehaviours
{
    public class TowerBehaviour : MonoBehaviour
    {
        private void Update()
        {
            LookAtMouse();
        }

        private void LookAtMouse()
        {
            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            diff.Normalize();
 
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }
}
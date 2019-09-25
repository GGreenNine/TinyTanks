using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HappyUnity.Gravity
{
    public class Gravity : MonoBehaviour {
 
        public float Radius;
        public float GravitationalPull; // Pull force
        public float MinRadius; // Minimum distance to pull from
        public float DistanceMultiplier;
 
        public LayerMask LayersToPull;
 
        void FixedUpdate()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, Radius, LayersToPull);
 
            foreach (var collider in colliders)
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
 
                if (rb == null) continue;
 
                Vector2 direction = gameObject.transform.position - collider.transform.position;
 
               
 
                float distance = direction.sqrMagnitude*DistanceMultiplier + 1; // Учитываем дальность от центра
 
                // Учитываем массу обьекта
                rb.velocity /=1.3f;
                rb.AddForce(direction.normalized * (GravitationalPull / distance) * rb.mass * Time.fixedDeltaTime);
            }
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(gameObject.transform.position, Radius);
        }
 
    }
}

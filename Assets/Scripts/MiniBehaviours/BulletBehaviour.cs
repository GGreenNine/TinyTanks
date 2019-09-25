using UnityEngine;

namespace MiniBehaviours
{
    public class BulletBehaviour : GameBehaviour
    {
        Rigidbody2D rb;

        public virtual void Fire(Vector2 position, Quaternion rotation, Vector2 velocity)
        {
            transform.position = position;
            transform.rotation = rotation;
            rb.velocity = velocity;
        }

        void Awake() { rb = GetComponent<Rigidbody2D>(); }
    }
}
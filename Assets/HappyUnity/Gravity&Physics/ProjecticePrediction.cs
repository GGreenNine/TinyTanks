using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HappyUnity.Gravity
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class ProjecticePrediction : MonoBehaviour {

        public static Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
        {
            Vector2[] results = new Vector2[steps];
 
            float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
            Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;
            float drag = 1f - timestep * rigidbody.drag;
            Vector2 moveStep = velocity * timestep;
 
            for (int i = 0; i < steps; ++i)
            {
                moveStep += gravityAccel;
                moveStep *= drag;
                pos += moveStep;
                results[i] = pos;
            }
 
            return results;
        }
    }
}

using UnityEngine;

namespace HappyUnity.TransformUtils
{
    public class RotationMotor : MonoBehaviour
    {
        public Vector3 Axis = new Vector3(0.0f, 0.0f, 1.0f);
        public float AnglularSpeed;

        protected virtual void Update()
        {
            transform.Rotate(Axis, AnglularSpeed * Time.deltaTime);
        }
    }
}
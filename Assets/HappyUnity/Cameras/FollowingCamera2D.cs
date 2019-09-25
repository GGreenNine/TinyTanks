using UnityEngine;

namespace HappyUnity.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class FollowingCamera2D : MonoBehaviour
    {
        public float DampTime;
        public Transform Target;
        public Vector3 Offset;

        private new Camera camera;
        private Vector3 velocity = Vector3.zero;

        public bool AllowedPositiveX = true;
        public bool AllowedNegativeX = true;
        public bool AllowedPositiveY = true;
        public bool AllowedNegativeY = true;

        public Vector3 UpperPositionLimit = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        public Vector3 LowerPositionLimit = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        public void SetTarget(Transform target)
        {
            Target = target;
        }
        

        // Update is called once per frame
        private void LateUpdate()
        {
            if (Target)
            {
                var targetPosition = Target.position;

                targetPosition.x = Mathf.Clamp(targetPosition.x, LowerPositionLimit.x, UpperPositionLimit.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, LowerPositionLimit.y, UpperPositionLimit.y);
                targetPosition.z = Mathf.Clamp(targetPosition.z, LowerPositionLimit.z, UpperPositionLimit.z);

                targetPosition += Offset;

                var targetViewportPosition = camera.WorldToViewportPoint(targetPosition);
                var delta = targetPosition -
                            camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, targetViewportPosition.z));
                var oldPosition = transform.position;
                var destination = oldPosition + delta;
                if (!AllowedPositiveX && destination.x > oldPosition.x ||
                    !AllowedNegativeX && destination.x < oldPosition.x)
                {
                    destination.x = oldPosition.x;
                }

                if (!AllowedPositiveY && destination.y > oldPosition.y ||
                    !AllowedNegativeY && destination.y < oldPosition.y)
                {
                    destination.y = oldPosition.y;
                }

                var newPosition = Vector3.SmoothDamp(oldPosition, destination, ref velocity, DampTime);

                transform.position = newPosition;
            }
        }
    }
}
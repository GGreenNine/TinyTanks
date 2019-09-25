using UnityEngine;

namespace HappyUnity.TransformUtils
{
    public class CameraScaleKeeper : MonoBehaviour
    {
        private Vector3 startScale;
        private float oldDepth;

        public float ExtraScale;

        private void Awake()
        {
            startScale = transform.localScale;
        }

        private void Start()
        {
            Update();
        }

        private void Update()
        {
            float currentDepth = CalculateCurrentDepth();
            if (System.Math.Abs(currentDepth - oldDepth) > 0.001f)
            {
                oldDepth = currentDepth;

                transform.localScale = new Vector3(
                    currentDepth * startScale.x * ExtraScale,
                    currentDepth * startScale.y * ExtraScale,
                    currentDepth * startScale.z * ExtraScale
                );
            }
        }

        private float CalculateCurrentDepth()
        {
            return Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        }
    }
}
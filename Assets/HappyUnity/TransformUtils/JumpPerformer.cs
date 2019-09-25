using System.Collections;
using UnityEngine;

namespace HappyUnity.TransformUtils
{
    public class JumpPerformer : MonoBehaviour
    {
        private IEnumerator activeJumpCoroutine;

        public float JumpProgress { get; private set; }
        public Vector3 Destination { get; private set; }

        public void Jump(Vector3 destination, float maxHeight, float time, bool finishPrevJump = false)
        {
            if (activeJumpCoroutine != null)
            {
                StopCoroutine(activeJumpCoroutine);
                activeJumpCoroutine = null;
                if (finishPrevJump)
                {
                    transform.position = Destination;
                }

                JumpProgress = 0.0f;
            }
            activeJumpCoroutine = JumpCoroutine(destination, maxHeight, time);
            StartCoroutine(activeJumpCoroutine);
        }

        private IEnumerator JumpCoroutine(Vector3 destination, float maxHeight, float time)
        {
            Destination = destination;
            var startPos = transform.position;
            while (JumpProgress <= 1.0)
            {
                JumpProgress += Time.deltaTime / time;
                var height = Mathf.Sin(Mathf.PI * JumpProgress) * maxHeight;
                if (height < 0f)
                {
                    height = 0f;
                }

                transform.position = Vector3.Lerp(startPos, destination, JumpProgress) + Vector3.up * height;
                yield return null;
            }
            transform.position = destination;
        }
    }
}
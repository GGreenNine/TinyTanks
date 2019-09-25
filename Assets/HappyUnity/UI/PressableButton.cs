using UnityEngine.Events;
using UnityEngine.UI;

namespace HappyUnity.UI
{
    public class PressableButton : Button
    {
        public UnityEvent OnPressed;

        private bool pressHappened;

        protected virtual void Update()
        {
            if (!pressHappened)
            {
                if (IsPressed())
                {
                    OnPressed.Invoke();
                    pressHappened = true;
                }
            }
            else
            {
                if (!IsPressed())
                {
                    pressHappened = false;
                }
            }
        }
    }
}
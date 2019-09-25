using UnityEngine;
using UnityEngine.UI;

namespace HappyUnity.UI
{
    [RequireComponent(typeof(Button))]
    public class FollowLinkButton : MonoBehaviour
    {
        public string Link;

        protected virtual void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Application.OpenURL(Link);
        }
    }
}
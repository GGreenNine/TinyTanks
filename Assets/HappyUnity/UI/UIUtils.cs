using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HappyUnity.UI
{
    public static class UIUtils
    {
        public static bool PointerOverUserInterface
        {
            get
            {
                PointerEventData eventDataCurrentPosition =
                    new PointerEventData(EventSystem.current)
                    {
                        position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
                    };
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                return results.Count > 0;
            }
        }
    }
}
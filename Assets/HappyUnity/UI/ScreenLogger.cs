using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HappyUnity.UI
{
    public class ScreenLogger : ScriptableObject
    {
        const int Lines = 18;
        static string[] logCache = new string[Lines];
        static string _outPut = string.Empty;
        private Rect border;
        private GUIStyle LogArea;

        public static void PushMessage(string message)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < logCache.Length - 1; i++)
            {
                logCache[i] = logCache[i + 1];
                builder.AppendLine(logCache[i]);
            }

            logCache[logCache.Length - 1] = message;
            builder.AppendLine(message);
            _outPut = builder.ToString();
        }

        void OnGUI()
        {
            LogArea.wordWrap = true;
            LogArea.clipping = TextClipping.Overflow;
            LogArea.normal.textColor = new Color(0.5f, .3f, 1, 0.9f);
            LogArea.fontSize = 16;

            GUI.Label(border, _outPut, LogArea);
        }
        
        public virtual void PushTextMessage(in TextMeshProUGUI text, in string message)
        {
            if (text)
                text.text = message;
        }
        
        public virtual void PushTextMessage(in Text text, in string message)
        {
            if (text)
                text.text = message;
        }

        private void Start()
        {
            border = new Rect(1, 1, Screen.width + 10, Screen.height + 10);
            LogArea = new GUIStyle();
        }
    }
}
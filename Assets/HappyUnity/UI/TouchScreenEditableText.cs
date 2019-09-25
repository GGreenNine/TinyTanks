using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HappyUnity.UI
{
    [RequireComponent(typeof(Text))]
    public class TouchScreenEditableText : MonoBehaviour
    {
        private Text text;
        private TouchScreenKeyboard keyboard;

        public TouchScreenKeyboardType TouchScreenKeyboardType = TouchScreenKeyboardType.Default;
        public bool Autocorrection;
        public bool Multiline;
        public bool Secure;
        public bool Alert;

        public int MaxLength = Int32.MaxValue;
        public char ErrorSymbol;
        public String[] SupportedSymbolsStrings;

        protected virtual void Awake()
        {
            text = GetComponentInChildren<Text>();
        }

        public void BeginTextEditing()
        {
            keyboard = TouchScreenKeyboard.Open(text.text, TouchScreenKeyboardType, Autocorrection, Multiline, Secure,
                Alert);
        }

        protected void Update()
        {
            if (keyboard != null)
            {
                if (keyboard.active)
                {
                    if (keyboard.text.Length > MaxLength)
                    {
                        keyboard.text = keyboard.text.Substring(0, MaxLength);
                    }

                    bool shitHappened = false;
                    var stringBuilder = new StringBuilder(keyboard.text);
                    for (int i = 0; i < stringBuilder.Length; ++i)
                    {
                        char symbol = stringBuilder[i];
                        if (!IsSymbolSupported(symbol))
                        {
                            stringBuilder[i] = ErrorSymbol;
                            shitHappened = true;
                            break;
                        }
                    }

                    if (shitHappened)
                    {
                        keyboard.text = stringBuilder.ToString();
                    }
                }
                else if (keyboard.done)
                {
                    text.text = keyboard.text;
                    keyboard = null;
                }
            }
        }

        private bool IsSymbolSupported(char symbol)
        {
            foreach (var symbolsString in SupportedSymbolsStrings)
            {
                if (symbolsString.Contains(symbol))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;
using HappyUnity.Math;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class ProgressBar : MonoBehaviour
    {
        #region Progress bar parameters
        public string PlayerID;

        public enum FillModes
        {
            LocalScale,
            FillAmount
        }

        public enum BarDirections
        {
            LeftToRight,
            RightToLeft,
            UpToDown,
            DownToUp
        }

        /// <summary>
        /// Progress bar fill mode
        /// </summary>
        public FillModes fillMode = FillModes.LocalScale;

        /// <summary>
        /// Progress bar fill direction
        /// </summary>
        public BarDirections barDirection = BarDirections.LeftToRight;

        /// <summary>
        /// Progress bar start value
        /// </summary>
        public float startValue = 0f;

        /// <summary>
        /// Progress bar end value
        /// </summary>
        public float endValue = 1f;

        /// <summary>
        /// Is foreground must be lerping?
        /// </summary>
        public bool isLerpingForeground = true;

        /// <summary>
        /// Foreground lerping speed
        /// </summary>
        public float foregroundBarSpeed = 15f;

        /// <summary>
        /// Delay before delayed image start lerping
        /// </summary>
        public float delay = 1f;

        /// <summary>
        /// Is delayed bar must be lerping?
        /// </summary>
        public bool isLerpingDelayedBar = true;

        /// <summary>
        /// Delayed bar learping speed
        /// </summary>
        public float lerpDelayedBarSpeed = 15f;

        [Header("Delayed bar object(with image)")]
        public Transform delayedBar;

        [Header("Foreground bar object(with image)")]
        public Transform foregroundBar;

        /// <summary>
        /// Is progress bar must be auto updating?
        /// </summary>
        public bool AutoUpdating = false;

        [Range(0f, 1f)] public float BarProgress;


        protected float TargetFillAmount;
        protected Vector3 _targetLocalScale = Vector3.one;
        protected float _newPercent;
        protected float _lastUpdateTimestamp;
        protected Color _initialColor;
        protected Vector3 _initialScale;
        protected Vector3 _newScale;


        protected Image _foregroundImage;
        protected Image _delayedImage;
        protected bool _initialized;

        #endregion

        private void Start()
        {
            _initialScale = this.transform.localScale;

            if (foregroundBar != null)
            {
                _foregroundImage = foregroundBar.GetComponent<Image>();
            }

            if (delayedBar != null)
            {
                _delayedImage = delayedBar.GetComponent<Image>();
            }

            _initialized = true;
        }

        protected virtual void AutoUpdate()
        {
            if (!AutoUpdating)
            {
                return;
            }

            _newPercent = HappyMath.Remap(BarProgress, 0f, 1f, startValue, endValue);
            TargetFillAmount = _newPercent;
            //_lastUpdateTimestamp = Time.time;
        }

        private void Update()
        {
            AutoUpdate();
            UpdateFrontBar();
            UpdateDelayedBar();
        }

        /// <summary>
        /// Updates the front bar's scale
        /// </summary>
        protected virtual void UpdateFrontBar()
        {
            if (foregroundBar != null)
            {
                if (fillMode == FillModes.LocalScale)
                {
                    _targetLocalScale = Vector3.one;
                    switch (barDirection)
                    {
                        case BarDirections.LeftToRight:
                            _targetLocalScale.x = TargetFillAmount;
                            break;
                        case BarDirections.RightToLeft:
                            _targetLocalScale.x = 1f - TargetFillAmount;
                            break;
                        case BarDirections.DownToUp:
                            _targetLocalScale.y = TargetFillAmount;
                            break;
                        case BarDirections.UpToDown:
                            _targetLocalScale.y = 1f - TargetFillAmount;
                            break;
                    }

                    if (isLerpingForeground)
                    {
                        _newScale = Vector3.Lerp(foregroundBar.localScale, _targetLocalScale,
                            Time.deltaTime * foregroundBarSpeed);
                    }
                    else
                    {
                        _newScale = _targetLocalScale;
                    }

                    foregroundBar.localScale = _newScale;
                }

                if ((fillMode == FillModes.FillAmount) && (_foregroundImage != null))
                {
                    if (isLerpingForeground)
                    {
                        _foregroundImage.fillAmount = Mathf.Lerp(_foregroundImage.fillAmount, TargetFillAmount,
                            Time.deltaTime * foregroundBarSpeed);
                    }
                    else
                    {
                        _foregroundImage.fillAmount = TargetFillAmount;
                    }
                }
            }
        }

        protected virtual void UpdateDelayedBar()
        {
            if (delayedBar == null) return;
            if ((Time.time - _lastUpdateTimestamp < delay)) return;
            if (fillMode == FillModes.LocalScale)
            {
                _targetLocalScale = Vector3.one;

                switch (barDirection)
                {
                    case BarDirections.LeftToRight:
                        _targetLocalScale.x = TargetFillAmount;
                        break;
                    case BarDirections.RightToLeft:
                        _targetLocalScale.x = 1f - TargetFillAmount;
                        break;
                    case BarDirections.DownToUp:
                        _targetLocalScale.y = TargetFillAmount;
                        break;
                    case BarDirections.UpToDown:
                        _targetLocalScale.y = 1f - TargetFillAmount;
                        break;
                }

                if (isLerpingDelayedBar)
                {
                    _newScale = Vector3.Lerp(delayedBar.localScale, _targetLocalScale,
                        Time.deltaTime * lerpDelayedBarSpeed);
                }
                else
                {
                    _newScale = _targetLocalScale;
                }

                delayedBar.localScale = _newScale;
            }

            if ((fillMode != FillModes.FillAmount) || (_delayedImage == null)) return;
            if (isLerpingDelayedBar)
            {
                _delayedImage.fillAmount = Mathf.Lerp(_delayedImage.fillAmount, TargetFillAmount,
                    Time.deltaTime * lerpDelayedBarSpeed);
            }
            else
            {
                _delayedImage.fillAmount = TargetFillAmount;
            }
        }

        /// <summary>
        /// Updates the bar's values based on the specified parameters
        /// </summary>
        /// <param name="currentValue">Current value.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Max value.</param>
        public virtual void UpdateBar(float currentValue, float minValue, float maxValue)
        {
            _newPercent = HappyMath.Remap(currentValue, minValue, maxValue, startValue, endValue);
            BarProgress = _newPercent;
            TargetFillAmount = _newPercent;
            _lastUpdateTimestamp = Time.time;
        }

        [ContextMenu("Test bar")]
        public void TestBar()
        {
            var value = Random.Range(1, 99);
            UpdateBar(value, 0, 100);
        }
    }
}

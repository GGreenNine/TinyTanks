using System;
using System.Collections;
using HappyUnity.TransformUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids
{
    public class HealthBar : MonoBehaviour
    {
        public enum HealthBarTypes
        {
            Prefab,
            Drawn
        }

        public HealthBarTypes HealthBarType = HealthBarTypes.Drawn;

        [Header("Select a Prefab")]
        /// the prefab to use as the health bar
        public ProgressBar HealthBarPrefab;

        [Header("Drawn Healthbar Settings ")]
        /// if the healthbar is drawn, its size in world units
        public Vector2 Size = new Vector2(1f, 0.2f);

        public Vector2 BackgroundPadding = new Vector2(0.01f, 0.01f);
        public Gradient ForegroundColor;
        public Gradient DelayedColor;
        public Gradient BorderColor;
        public Gradient BackgroundColor;
        public string SortingLayerName = "UI";
        public float Delay = 0.5f;
        public bool LerpFrontBar = true;
        public float LerpFrontBarSpeed = 15f;
        public int renderOrder;
        public bool LerpDelayedBar = true;
        public float LerpDelayedBarSpeed = 15f;
        public FollowTarget.Modes FollowTargetMode = FollowTarget.Modes.LateUpdate;

        [Header("Death")] public GameObject InstantiatedOnDeath;

        [Header("Offset")] public Vector3 HealthBarOffset = new Vector3(0f, 1f, 0f);

        [Header("Display")] public bool AlwaysVisible = true;
        public float DisplayDurationOnHit = 1f;
        public bool HideBarAtZero = true;
        public float HideBarAtZeroDelay = 1f;

        protected ProgressBar _progressBar;
        protected FollowTarget _followTransform;
        protected float _lastShowTimestamp = 0f;
        protected bool _showBar = false;
        protected Image _backgroundImage = null;
        protected Image _borderImage = null;
        protected Image _foregroundImage = null;
        protected Image _delayedImage = null;
        protected bool _finalHideStarted = false;

        /// <summary>
        /// On Start, creates or sets the health bar up
        /// </summary>
        protected virtual void Start()
        {
            if (HealthBarType == HealthBarTypes.Prefab)
            {
                if (HealthBarPrefab == null)
                {
                    Debug.LogWarning(
                        this.name + " : the HealthBar has no prefab associated to it, nothing will be displayed.");
                    return;
                }

                _progressBar =
                    Instantiate(HealthBarPrefab, transform.position + HealthBarOffset, transform.rotation) as
                        ProgressBar;
                _progressBar.transform.SetParent(this.transform);
                _progressBar.gameObject.name = "HealthBar";
            }

            if (HealthBarType == HealthBarTypes.Drawn)
            {
                DrawHealthBar();
                UpdateDrawnColors();
            }

            if (!AlwaysVisible)
            {
                _progressBar.gameObject.SetActive(false);
            }

            if (_progressBar != null)
            {
                _progressBar.UpdateBar(100f, 0f, 100f);
            }
        }

        /// <summary>
        /// Draws the health bar.
        /// </summary>
        protected virtual void DrawHealthBar()
        {
            GameObject newGameObject = new GameObject();
            newGameObject.name = "HealthBar|" + this.gameObject.name;

            _progressBar = newGameObject.AddComponent<ProgressBar>();

            _followTransform = newGameObject.AddComponent<FollowTarget>();
            _followTransform.Offset = HealthBarOffset;
            _followTransform.Target = this.transform;
            _followTransform.InterpolatePosition = false;
            _followTransform.InterpolateRotation = false;
            _followTransform.UpdateMode = FollowTargetMode;

            Canvas newCanvas = newGameObject.AddComponent<Canvas>();
            newCanvas.renderMode = RenderMode.WorldSpace;
            newCanvas.transform.localScale = Vector3.one;
            newCanvas.GetComponent<RectTransform>().sizeDelta = Size;
            newCanvas.sortingOrder = renderOrder;

            if (SortingLayerName != "")
            {
                newCanvas.sortingLayerName = SortingLayerName;
            }

            GameObject borderImageGameObject = new GameObject();
            borderImageGameObject.transform.SetParent(newGameObject.transform);
            borderImageGameObject.name = "HealthBar Border";
            _borderImage = borderImageGameObject.AddComponent<Image>();
            _borderImage.transform.position = Vector3.zero;
            _borderImage.transform.localScale = Vector3.one;
            _borderImage.GetComponent<RectTransform>().sizeDelta = Size;
            _borderImage.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            GameObject bgImageGameObject = new GameObject();
            bgImageGameObject.transform.SetParent(newGameObject.transform);
            bgImageGameObject.name = "HealthBar Background";
            _backgroundImage = bgImageGameObject.AddComponent<Image>();
            _backgroundImage.transform.position = Vector3.zero;
            _backgroundImage.transform.localScale = Vector3.one;
            _backgroundImage.GetComponent<RectTransform>().sizeDelta = Size - BackgroundPadding * 2;
            _backgroundImage.GetComponent<RectTransform>().anchoredPosition =
                -_backgroundImage.GetComponent<RectTransform>().sizeDelta / 2;
            _backgroundImage.GetComponent<RectTransform>().pivot = Vector2.zero;

            GameObject delayedImageGameObject = new GameObject();
            delayedImageGameObject.transform.SetParent(newGameObject.transform);
            delayedImageGameObject.name = "HealthBar Delayed Foreground";
            _delayedImage = delayedImageGameObject.AddComponent<Image>();
            _delayedImage.transform.position = Vector3.zero;
            _delayedImage.transform.localScale = Vector3.one;
            _delayedImage.GetComponent<RectTransform>().sizeDelta = Size - BackgroundPadding * 2;
            _delayedImage.GetComponent<RectTransform>().anchoredPosition =
                -_delayedImage.GetComponent<RectTransform>().sizeDelta / 2;
            _delayedImage.GetComponent<RectTransform>().pivot = Vector2.zero;

            GameObject frontImageGameObject = new GameObject();
            frontImageGameObject.transform.SetParent(newGameObject.transform);
            frontImageGameObject.name = "HealthBar Foreground";
            _foregroundImage = frontImageGameObject.AddComponent<Image>();
            _foregroundImage.transform.position = Vector3.zero;
            _foregroundImage.transform.localScale = Vector3.one;
            _foregroundImage.GetComponent<RectTransform>().sizeDelta = Size - BackgroundPadding * 2;
            _foregroundImage.GetComponent<RectTransform>().anchoredPosition =
                -_foregroundImage.GetComponent<RectTransform>().sizeDelta / 2;
            _foregroundImage.GetComponent<RectTransform>().pivot = Vector2.zero;

            _progressBar.isLerpingDelayedBar = LerpDelayedBar;
            _progressBar.isLerpingForeground = LerpFrontBar;
            _progressBar.lerpDelayedBarSpeed = LerpDelayedBarSpeed;
            _progressBar.foregroundBarSpeed = LerpFrontBarSpeed;
            _progressBar.foregroundBar = _foregroundImage.transform;
            _progressBar.delayedBar = _delayedImage.transform;
            _progressBar.delay = Delay;
        }

        private void OnDisable()
        {
            if (_progressBar != null)
                _progressBar.gameObject.SetActive(false);
        }

        /// <summary>
        /// On Update, we hide or show our healthbar based on our current status
        /// </summary>
        protected virtual void Update()
        {
            if (_progressBar == null)
            {
                return;
            }

            if (_finalHideStarted)
            {
                return;
            }

            UpdateDrawnColors();

            if (AlwaysVisible)
            {
                return;
            }

            if (_showBar)
            {
                _progressBar.gameObject.SetActive(true);
                if (Time.time - _lastShowTimestamp > DisplayDurationOnHit)
                {
                    _showBar = false;
                }
            }
            else
            {
                _progressBar.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Hides the bar when it reaches zero
        /// </summary>
        /// <returns>The hide bar.</returns>
        protected virtual IEnumerator FinalHideBar()
        {
            _finalHideStarted = true;
            if (InstantiatedOnDeath != null)
            {
                Instantiate(InstantiatedOnDeath, this.transform.position + HealthBarOffset, this.transform.rotation);
            }

            if (HideBarAtZeroDelay == 0)
            {
                _showBar = false;
                _progressBar.gameObject.SetActive(false);
            }
            else
            {
                yield return new WaitForSeconds(HideBarAtZeroDelay);
                _showBar = false;
                _progressBar.gameObject.SetActive(false);
            }
        }

        public void FinalShowBar()
        {
            _progressBar.gameObject.SetActive(true);
        }

        public void DestroyBar()
        {
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Updates the colors of the different bars
        /// </summary>
        protected virtual void UpdateDrawnColors()
        {
            if (HealthBarType != HealthBarTypes.Drawn)
            {
                return;
            }

            if (_borderImage != null)
            {
                _borderImage.color = BorderColor.Evaluate(_progressBar.BarProgress);
            }

            if (_backgroundImage != null)
            {
                _backgroundImage.color = BackgroundColor.Evaluate(_progressBar.BarProgress);
            }

            if (_delayedImage != null)
            {
                _delayedImage.color = DelayedColor.Evaluate(_progressBar.BarProgress);
            }

            if (_foregroundImage != null)
            {
                _foregroundImage.color = ForegroundColor.Evaluate(_progressBar.BarProgress);
            }
        }

        /// <summary>
        /// Updates the bar
        /// </summary>
        /// <param name="currentHealth">Current health.</param>
        /// <param name="minHealth">Minimum health.</param>
        /// <param name="maxHealth">Max health.</param>
        /// <param name="show">Whether or not we should show the bar.</param>
        public virtual void UpdateBar(float currentHealth, float minHealth, float maxHealth, bool show)
        {
            if (_progressBar != null)
            {
                _progressBar.UpdateBar(currentHealth, minHealth, maxHealth);

                if (HideBarAtZero && _progressBar.BarProgress <= 0)
                {
                    StartCoroutine(FinalHideBar());
                }
            }

            // if the healthbar isn't supposed to be always displayed, we turn it on for the specified duration
            if (!AlwaysVisible && show)
            {
                _showBar = true;
                _lastShowTimestamp = Time.time;
            }
        }
    }
}
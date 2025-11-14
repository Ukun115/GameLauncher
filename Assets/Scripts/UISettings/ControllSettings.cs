using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Lancher
{
    /// <summary>
    /// 操作設定
    /// </summary>
    public class ControllSettings : MonoBehaviour
    {
        [Header("マウス反転テキスト"), SerializeField]
        private TMP_Text _invertMouseText;

        [Header("BGMスライダー"), SerializeField]
        private Slider _BGMSlider;

        [Header("感度Xスライダー"), SerializeField]
        private Slider _sensitivityXSlider;

        [Header("感度Yスライダー"), SerializeField]
        private Slider _sensitivityYSlider;

        [Header("マウススムージングスライダー"), SerializeField]
        private Slider _mouseSmoothSlider;

        /// <summary>
        /// スライダー値：X感度
        /// </summary>
        private float _sliderValueXSensitivity = 0.0f;

        /// <summary>
        /// スライダー値：Y感度
        /// </summary>
        private float _sliderValueYSensitivity = 0.0f;

        /// <summary>
        /// スライダー値：マウススムージング
        /// </summary>
        private float _sliderValueSmoothing = 0.0f;

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            // 初期化
            Init();
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            // 感度Xスライダー
            _sliderValueXSensitivity = _sensitivityXSlider.value;
            // 感度Yスライダー
            _sliderValueYSensitivity = _sensitivityYSlider.value;
            // マウススムージングスライダー
            _sliderValueSmoothing = _mouseSmoothSlider.value;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Init()
        {
            // BGMスライダー
            _BGMSlider.value = PlayerPrefs.GetFloat(PlayerPrefsData.BGMVolumeKey);
            // 感度Xスライダー
            _sensitivityXSlider.value = PlayerPrefs.GetFloat(PlayerPrefsData.XSensitivityKey);
            // 感度Yスライダー
            _sensitivityYSlider.value = PlayerPrefs.GetFloat(PlayerPrefsData.YSensitivityKey);
            // マウススムージングスライダー
            _mouseSmoothSlider.value = PlayerPrefs.GetFloat(PlayerPrefsData.MouseSmoothingKey);
            // マウス反転テキスト
            var isInvertMouse = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsData.InvertedKey));
            _invertMouseText.text = isInvertMouse ? "on":"off";
        }

        /// <summary>
        /// BGM用スライダー
        /// </summary>
        public void OnBGMSlider()
        {
            PlayerPrefs.SetFloat(PlayerPrefsData.BGMVolumeKey, _BGMSlider.value);
        }

        /// <summary>
        /// X感度用スライダー
        /// </summary>
        public void OnSensitivityXSlider()
        {
            PlayerPrefs.SetFloat(PlayerPrefsData.XSensitivityKey, _sliderValueXSensitivity);
        }

        /// <summary>
        /// Y感度用スライダー
        /// </summary>
        public void OnSensitivityYSlider()
        {
            PlayerPrefs.SetFloat(PlayerPrefsData.YSensitivityKey, _sliderValueYSensitivity);
        }

        /// <summary>
        /// マウススムージング用スライダー
        /// </summary>
        public void OnSensitivitySmoothing()
        {
            PlayerPrefs.SetFloat(PlayerPrefsData.MouseSmoothingKey, _sliderValueSmoothing);
        }

        /// <summary>
        /// マウス反転
        /// </summary>
        public void OnInvertMouse()
        {
            var isInvertMouse = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsData.InvertedKey));
            _invertMouseText.text = isInvertMouse ? "on":"off";
        }
    }
}
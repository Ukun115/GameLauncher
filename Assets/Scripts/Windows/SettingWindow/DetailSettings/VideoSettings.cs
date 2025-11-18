using UnityEngine;
using TMPro;
using System;

namespace Lancher
{
    /// <summary>
    /// ビデオ設定
    /// </summary>
    public class VideoSettings : MonoBehaviour
    {
        [Header("フルスクリーンテキスト"), SerializeField]
        private TMP_Text _fullScreenText;

        [Header("アンビエントオクルージョン(AO)テキスト"), SerializeField]
        private TMP_Text _ambientOcclusionText;

        [Header("シャドウオフテキストオブジェクト"), SerializeField]
        private GameObject _shadowOffTextLineObj;

        [Header("シャドウ低テキストオブジェクト"), SerializeField]
        private GameObject _shadowLowTextLineObj;

        [Header("シャドウ高テキストオブジェクト"), SerializeField]
        private GameObject _shadowHighTextLineObj;

        [Header("VSyncテキスト"), SerializeField]
        private TMP_Text _vsyncText;

        [Header("モーションブラーテキスト"), SerializeField]
        private TMP_Text _motionBlurText;

        [Header("テクスチャ低テキストオブジェクト"), SerializeField]
        private GameObject _textureLowTextLineObj;

        [Header("テクスチャ中テキストオブジェクト"), SerializeField]
        private GameObject _textureMedTextLineObj;

        [Header("テクスチャ高テキストオブジェクト"), SerializeField]
        private GameObject _textureHighTextLineObj;

        [Header("カメラエフェクトテキスト"), SerializeField]
        private TMP_Text _cameraEffectsText;

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            // 初期化
            Init();   
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Init()
        {
            // フルスクリーンテキスト設定
            _fullScreenText.text = Screen.fullScreen?"on":"off";

            // VSyncテキスト設定
            _vsyncText.text = Convert.ToBoolean(QualitySettings.vSyncCount) ? "on":"off";

            // モーションブラーテキスト設定
            _motionBlurText.text = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsData.MotionBlurKey)) ? "on":"off";

            // アンビエントオクルージョン設定
            _ambientOcclusionText.text = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsData.AmbientOcclusionKey)) ? "on":"off";

            // シャドウ品質設定
            switch((PlayerPrefsData.ShadowType)PlayerPrefs.GetInt(PlayerPrefsData.ShadowsKey))
            {
                // オフ
                case PlayerPrefsData.ShadowType.Off:
                SetShadowSettings(0,0,true,false,false);
                break;

                // 低
                case PlayerPrefsData.ShadowType.Low:
                SetShadowSettings(2,75,false,true,false);
                break;

                // 高
                case PlayerPrefsData.ShadowType.High:
                SetShadowSettings(4,500,false,false,true);
                break;
            }

            // テクスチャ品質設定
            switch((PlayerPrefsData.TextureType)PlayerPrefs.GetInt(PlayerPrefsData.TexturesKey))
            {
                // 低
                case PlayerPrefsData.TextureType.Low:
                SetTextureSettings(2,true,false,false);
                break;

                // 中
                case PlayerPrefsData.TextureType.Med:
                SetTextureSettings(1,false,true,false);
                break;

                // 高
                case PlayerPrefsData.TextureType.High:
                SetTextureSettings(0,false,false,true);
                break;
            }
        }

        /// <summary>
        /// シャドウ設定の設定
        /// </summary>
        /// <param name="shadowCascades"></param>
        /// <param name="shadowDistance"></param>
        /// <param name="isOff"></param>
        /// <param name="isLow"></param>
        /// <param name="isHigh"></param>
        private void SetShadowSettings(int shadowCascades,int shadowDistance,bool isOff,bool isLow,bool isHigh)
        {
            QualitySettings.shadowCascades = shadowCascades;
                QualitySettings.shadowDistance = shadowDistance;
                _shadowOffTextLineObj.SetActive(isOff);
                _shadowLowTextLineObj.SetActive(isLow);
                _shadowHighTextLineObj.SetActive(isHigh);
        }

        /// <summary>
        /// シャドウオフ
        /// </summary>
        public void OnShadowsOff()
        {
            PlayerPrefs.SetInt(PlayerPrefsData.ShadowsKey, 0);
            SetShadowSettings(0,0,true,false,false);
        }

        /// <summary>
        /// シャドウロー
        /// </summary>
        public void OnShadowsLow()
        {
            PlayerPrefs.SetInt(PlayerPrefsData.ShadowsKey, 1);
            SetShadowSettings(2,75,false,true,false);
        }

        /// <summary>
        /// シャドウハイ
        /// </summary>
        public void OnShadowsHigh()
        {
            PlayerPrefs.SetInt(PlayerPrefsData.ShadowsKey, 2);
            SetShadowSettings(4,500,false,false,true);
        }

        /// <summary>
        /// フルスクリーン
        /// </summary>
        public void OnFullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            _fullScreenText.text = Screen.fullScreen?"on":"off";
        }

        /// <summary>
        /// VSync
        /// </summary>
        public void OnVSync()
        {
            var isOn = Convert.ToBoolean(QualitySettings.vSyncCount);
            _vsyncText.text = isOn ? "on":"off";
            QualitySettings.vSyncCount = Convert.ToInt32(!isOn);
        }

        /// <summary>
        /// モーションブラー
        /// </summary>
        public void OnMotionBlur()
        {
            var isOn = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsData.MotionBlurKey));
            PlayerPrefs.SetInt(PlayerPrefsData.MotionBlurKey, Convert.ToInt32(!isOn));
            _motionBlurText.text = isOn ? "on":"off";
        }

        /// <summary>
        /// AO(アンビエントオクルージョン)
        /// </summary>
        public void OnAmbientOcclusion()
        {
            var isOn = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsData.AmbientOcclusionKey));
            PlayerPrefs.SetInt(PlayerPrefsData.AmbientOcclusionKey, Convert.ToInt32(!isOn));
            _ambientOcclusionText.text = isOn ? "on":"off";
        }

        /// <summary>
        /// カメラエフェクト
        /// </summary>
        public void OnCameraEffects()
        {
            var isOn = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsData.CameraEffectsKey));
            PlayerPrefs.SetInt(PlayerPrefsData.CameraEffectsKey, Convert.ToInt32(!isOn));
            _cameraEffectsText.text = isOn?"on":"off";
        }

        /// <summary>
        /// テクスチャ設定の設定
        /// </summary>
        private void SetTextureSettings(int globalTextureMipmapLimit,bool isLow,bool isMed,bool isHigh)
        {
            QualitySettings.globalTextureMipmapLimit = globalTextureMipmapLimit;
            _textureLowTextLineObj.SetActive(isLow);
            _textureMedTextLineObj.SetActive(isMed);
            _textureHighTextLineObj.SetActive(isHigh);
        }

        /// <summary>
        /// テクスチャロー
        /// </summary>
        public void OnTexturesLow()
        {
            PlayerPrefs.SetInt(PlayerPrefsData.TexturesKey, 0);
            SetTextureSettings(2,true,false,false);
        }

        /// <summary>
        /// テクスチャミディアム
        /// </summary>
        public void OnTexturesMed()
        {
            PlayerPrefs.SetInt(PlayerPrefsData.TexturesKey, 1);
            SetTextureSettings(1,false,true,false);
        }

        /// <summary>
        /// テクスチャハイ
        /// </summary>
        public void OnTexturesHigh()
        {
            PlayerPrefs.SetInt(PlayerPrefsData.TexturesKey, 2);
            SetTextureSettings(0,false,false,true);
        }
    }
}
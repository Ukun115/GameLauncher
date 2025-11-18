using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace Lancher
{
    /// <summary>
    /// ゲーム設定
    /// </summary>
    public class GameSettings : MonoBehaviour
    {
        [Header("BGMスライダー"), SerializeField]
        private Slider _BGMSlider;

        [Header("HUD表示テキスト"), SerializeField]
        private TMP_Text _showHudText;

        [Header("ツールチップテキスト"), SerializeField]
        private TMP_Text _toolTipsText;

        [Header("ノーマル難易度テキストオブジェクト"), SerializeField]
        private GameObject _difficultyNormalTextLineObj;

        [Header("ハードコア難易度テキストオブジェクト"), SerializeField]
        private GameObject _difficultyHardCoreTextLineObj;

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
            // BGMスライダー
            _BGMSlider.value = PlayerPrefs.GetFloat(PlayerPrefsData.BGMVolumeKey);

            // 難易度設定に応じたテキスト表示
            var isNormal = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsData.DifficultyKey));
            _difficultyNormalTextLineObj.SetActive(isNormal);
            _difficultyHardCoreTextLineObj.SetActive(!isNormal);

            // HUD表示設定に応じたテキスト表示
            var isShowHUD = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsData.ShowHUDKey));
            _showHudText.text = isShowHUD ? "on" : "off";

            // ツールチップ表示設定に応じたテキスト表示
            var isToolTips = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsData.ToolTipsKey));
            _toolTipsText.text = isToolTips ? "on" : "off";
        }

        /// <summary>
        /// BGM用スライダー
        /// </summary>
        public void OnBGMSlider()
        {
            PlayerPrefs.SetFloat(PlayerPrefsData.BGMVolumeKey, _BGMSlider.value);
        }

        /// <summary>
        /// HUD
        /// <param name="isShow"> HUD表示かどうか </param>
        /// </summary>
        public void OnHUD(bool isShow)
        {
            _showHudText.text = isShow ? "on" : "off";
            PlayerPrefs.SetInt(PlayerPrefsData.ShowHUDKey, Convert.ToInt32(isShow));
        }

        /// <summary>
        /// ツールチップ
        /// <param name="isShow"> ツールチップ表示かどうか </param>
        /// </summary>
        public void OnToolTips(bool isShow)
        {
            _toolTipsText.text = isShow ? "on" : "off";
            PlayerPrefs.SetInt(PlayerPrefsData.ToolTipsKey, Convert.ToInt32(isShow));
        }

        /// <summary>
        /// 難易度
        /// <param name="isNormal">ノーマル難易度かどうか</param>
        /// </summary>
        public void OnDifficulty(bool isNormal)
        {
            _difficultyNormalTextLineObj.SetActive(isNormal);
            _difficultyHardCoreTextLineObj.SetActive(!isNormal);
            PlayerPrefs.SetInt(PlayerPrefsData.DifficultyKey, Convert.ToInt32(isNormal));
        }
    }
}
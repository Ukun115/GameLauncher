using UnityEngine;
using UnityEngine.UI;

namespace Lancher
{
    /// <summary>
    /// 設定ウィンドウボタン監視
    /// </summary>
    public class SettingWindowButtonObserver : MonoBehaviour
    {
        [Header("設定ウィンドウボタンリスナー"), SerializeField]
        private SettingWindowButtonListener _buttonListener;

        [Header("ゲームボタン"), SerializeField]
        private Button _gameButton;

        [Header("操作ボタン"), SerializeField]
        private Button _controlsButton;

        [Header("ビデオボタン"), SerializeField]
        private Button _videoButton;

        [Header("戻るボタン"), SerializeField]
        private Button _returnButton;

        [Header("キーバインドボタン"), SerializeField]
        private Button _keyBindingsButton;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            // ボタンリスナー登録
            AddButtonListeners();
        }

        /// <summary>
        /// ボタンリスナー登録
        /// </summary>
        private void AddButtonListeners()
        {
            // 各ボタンクリック時のリスナー登録
            _gameButton.onClick.AddListener(_buttonListener.OnClickGameButton);
            _controlsButton.onClick.AddListener(_buttonListener.OnClickControlsButton);
            _videoButton.onClick.AddListener(_buttonListener.OnClickVideoButton);
            // NOTE:iseki ラムダ式じゃないとリスナー登録時にOnClickReturnButtonメソッドが呼ばれてしまう
            _returnButton.onClick.AddListener(() => _buttonListener.OnClickReturnButton().Forget());
            _keyBindingsButton.onClick.AddListener(_buttonListener.OnClickKeyBindingsButton);
        }
    }
}
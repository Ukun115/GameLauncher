using UnityEngine;
using UnityEngine.UI;

namespace Lancher
{
    /// <summary>
    /// メインウィンドウボタン監視
    /// </summary>
    public class MainWindowButtonObserver : MonoBehaviour
    {
        [Header("メインウィンドウボタンリスナー"), SerializeField]
        private MainWindowButtonListener _buttonLisner;

        [Header("学生作品ボタン"), SerializeField]
        private Button _studentProductionsButton;

        [Header("開発情報ボタン"), SerializeField]
        private Button _developInfomationButton;

        [Header("ヘルプボタン"), SerializeField]
        private Button _helpButton;

        [Header("設定ボタン"), SerializeField]
        private Button _settingButton;

        [Header("終了ボタン"), SerializeField]
        private Button _exitButton;

        [Header("ランチャー説明ボタン"), SerializeField]
        private Button _explanationLancherButton;

        [Header("FAQボタン"), SerializeField]
        private Button _FAQButton;

        [Header("終了(いいえ)ボタン"), SerializeField]
        private Button _exitNoButton;

        [Header("終了(はい)ボタン"), SerializeField]
        private Button _exitYesButton;

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
            _studentProductionsButton.onClick.AddListener(_buttonLisner.OnClickStudentProductionsButton);
            _developInfomationButton.onClick.AddListener(_buttonLisner.OnClickDevelopInfomationButton);
            _helpButton.onClick.AddListener(_buttonLisner.OnHelpButton);
            _settingButton.onClick.AddListener(_buttonLisner.OnSettingButton);
            _exitButton.onClick.AddListener(_buttonLisner.OnExitButton);
            _explanationLancherButton.onClick.AddListener(_buttonLisner.OnExplanationLancherButton);
            _FAQButton.onClick.AddListener(_buttonLisner.OnFAQButton);
            _exitNoButton.onClick.AddListener(_buttonLisner.OnExitNoButton);
            _exitYesButton.onClick.AddListener(_buttonLisner.OnExitYesButton);
        }
    }
}
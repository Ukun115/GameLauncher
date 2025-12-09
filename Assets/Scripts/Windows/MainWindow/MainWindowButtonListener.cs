using Cysharp.Threading.Tasks;
using SlimUI.ModernMenu;
using UnityEngine;
using static SlimUI.ModernMenu.ThemedUIData;

namespace Launcher
{
    /// <summary>
    /// メインウィンドウボタンリスナー
    /// </summary>
    public class MainWindowButtonListener : MonoBehaviour
    {
        [Header("テーマ"), SerializeField]
        private Theme _theme;

        [Header("テーマコントローラー"), SerializeField]
        private ThemedUIData _themeController;

        [Header("学生作品ウィンドウオブジェクト"), SerializeField]
        private GameObject _studentProductionsWindowObj;

        [Header("ヘルプウィンドウオブジェクト"), SerializeField]
        private GameObject _helpWindowObj;

        [Header("カメラマネージャー"), SerializeField]
        private CameraManager _cameraManager;

        [Header("設定ウィンドウオブジェクト"), SerializeField]
        private GameObject _settingsWindowObj;

        [Header("終了ダイアログオブジェクト"), SerializeField]
        private GameObject _exitDialogObj;

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
            // テーマに応じてカラー設定
            switch(_theme)
            {
                case Theme.Custom1:
                    SetThemeColors(_themeController.Custom1.Graphic,_themeController.Custom1.Text);
                    break;
                case Theme.Custom2:
                    SetThemeColors(_themeController.Custom2.Graphic,_themeController.Custom2.Text);
                    break;
                case Theme.Custom3:
                    SetThemeColors(_themeController.Custom3.Graphic,_themeController.Custom3.Text);
                    break;
            }
        }

        /// <summary>
        /// 全てのウィンドウ非表示
        /// </summary>
        private void HiddenAllWindow()
        {
            _studentProductionsWindowObj.SetActive(false);
            _helpWindowObj.SetActive(false);
            _exitDialogObj.SetActive(false);
        }

        /// <summary>
        /// テーマカラー設定
        /// </summary>
        private void SetThemeColors(Color graphicColor,Color32 textColor)
        {
            _themeController.CurrentColor = graphicColor;
            _themeController.TextColor = textColor;
        }

        /// <summary>
        /// 学生作品ボタン押下時
        /// </summary>
        public void OnClickStudentProductionsButton()
        {
            HiddenAllWindow();
            _studentProductionsWindowObj.SetActive(true);
            _cameraManager.MoveToPosition2Task().Forget();
        }

        /// <summary>
        /// 開発情報ボタン押下時
        /// </summary>
        public void OnClickDevelopInfomationButton()
        {
            // TODO:iseki 開発情報Canvas未実装のため未実装
        }

        /// <summary>
        /// ヘルプボタン押下時
        /// </summary>
        public void OnHelpButton()
        {
            if(_helpWindowObj.activeSelf)
            {
                return;
            }

            HiddenAllWindow();
            _helpWindowObj.SetActive(true);
        }

        /// <summary>
        /// 設定ボタン押下時
        /// </summary>
        public void OnSettingButton()
        {
            HiddenAllWindow();
            _settingsWindowObj.SetActive(true);
            _cameraManager.MoveToPosition2Task().Forget();
        }

        /// <summary>
        /// 終了ボタン押下時
        /// </summary>
        public void OnExitButton()
        {
            if(_exitDialogObj.activeSelf)
            {
                return;
            }

            HiddenAllWindow();
            _exitDialogObj.SetActive(true);
        }

        /// <summary>
        /// ランチャー説明ボタン押下時
        /// </summary>
        public void OnExplanationLancherButton()
        {
            // TODO:iseki ランチャー説明Canvas未実装のため未実装
        }

        /// <summary>
        /// FAQボタン押下時
        /// </summary>
        public void OnFAQButton()
        {
            // TODO:iseki FAQCanvas未実装のため未実装
        }

        /// <summary>
        /// 終了(いいえ)ボタン押下時
        /// </summary>
        public void OnExitNoButton()
        {
            _exitDialogObj.SetActive(false);
        }

        /// <summary>
        /// 終了(はい)ボタン押下時
        /// </summary>
        public void OnExitYesButton()
        {
            OnQuitGame();
        }

        /// <summary>
        /// ゲーム終了
        /// </summary>
        public void OnQuitGame()
        {
#if UNITY_EDITOR
            // エディター上で実行している場合は再生モードを停止
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }
    }
}
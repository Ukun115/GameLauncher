using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lancher
{
    /// <summary>
    /// 設定ウィンドウボタンリスナー
    /// </summary>
    public class SettingWindowButtonListener : MonoBehaviour
    {
        [Header("ゲームパネルオブジェクト"), SerializeField]
        private GameObject _gamePanelObj;

        [Header("ゲームボタンハイライト"), SerializeField]
        private GameObject _gameButtonHighlight;

        [Header("操作パネルオブジェクト"), SerializeField]
        private GameObject _controlsPanelObj;

        [Header("操作ボタンハイライト"), SerializeField]
        private GameObject _controlsButtonHighlight;

        [Header("ビデオパネルオブジェクト"), SerializeField]
        private GameObject _videoPanelObj;

        [Header("ビデオボタンハイライト"), SerializeField]
        private GameObject _videoButtonHighlight;

        [Header("キーバインドパネルオブジェクト"), SerializeField]
        private GameObject _keyBindingsPanelObj;

        [Header("キーバインドボタンハイライト"), SerializeField]
        private GameObject _keyBindingsButtonHighlight;

        [Header("カメラマネージャー"), SerializeField]
        private CameraManager _cameraManager;

        /// <summary>
        /// 全てのウィンドウ非表示
        /// </summary>
        private void HiddenAllWindow()
        {
            _gamePanelObj.SetActive(false);
            _controlsPanelObj.SetActive(false);
            _videoPanelObj.SetActive(false);
            _keyBindingsPanelObj.SetActive(false);
        }

        /// <summary>
        /// 全てのハイライト非表示
        /// </summary>
        private void HiddenAllHighlight()
        {
            _gameButtonHighlight.SetActive(false);
            _controlsButtonHighlight.SetActive(false);
            _videoButtonHighlight.SetActive(false);
            _keyBindingsButtonHighlight.SetActive(false);
        }

        /// <summary>
        /// ゲームボタン押下時
        /// </summary>
        public void OnClickGameButton()
        {
            HiddenAllWindow();
            HiddenAllHighlight();
            _gameButtonHighlight.SetActive(true);
            _gamePanelObj.SetActive(true);
        }

        /// <summary>
        /// 操作ボタン押下時
        /// </summary>
        public void OnClickControlsButton()
        {
            HiddenAllWindow();
            HiddenAllHighlight();
            _controlsButtonHighlight.SetActive(true);
            _controlsPanelObj.SetActive(true);
        }

        /// <summary>
        /// ビデオボタン押下時
        /// </summary>
        public void OnClickVideoButton()
        {
            HiddenAllWindow();
            HiddenAllHighlight();
            _videoButtonHighlight.SetActive(true);
            _videoPanelObj.SetActive(true);
        }

        /// <summary>
        /// 戻るボタン押下時
        /// </summary>
        public async UniTaskVoid OnClickReturnButton()
        {
            await _cameraManager.MoveToPosition1Task();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// キーバインドボタン押下時
        /// </summary>
        public void OnClickKeyBindingsButton()
        {
            HiddenAllWindow();
            HiddenAllHighlight();
            _keyBindingsButtonHighlight.SetActive(true);
            _keyBindingsPanelObj.SetActive(true);
        }
    }
}
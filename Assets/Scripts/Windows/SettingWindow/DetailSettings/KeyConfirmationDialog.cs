using UnityEngine;
using UnityEngine.UI;

namespace Lancher
{
    /// <summary>
    /// キー確認ダイアログ
    /// </summary>
    public class KeyConfirmationDialog : MonoBehaviour
    {
        [Header("キャンセルボタン"), SerializeField]
        private Button _cancelButton;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            // キャンセルボタン押下時のリスナー登録
            _cancelButton.onClick.AddListener(OnClickCancelButton);
        }

        /// <summary>
        /// キャンセルボタンが押下された時
        /// </summary>
        private void OnClickCancelButton()
        {
            gameObject.SetActive(false);
        }
    }
}
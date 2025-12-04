using UnityEngine;
using UnityEngine.UI;

namespace Lancher
{
    /// <summary>
    /// ゲーム動画セル
    /// </summary>
    public class GameMovieCell : MonoBehaviour
    {
        [Header("ボタン"), SerializeField]
        private Button _button;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            // リスナー登録
            _button.onClick.AddListener(OnClickedButton);
        }

        /// <summary>
        /// ボタン押下時処理
        /// </summary>
        private void OnClickedButton()
        {
            // ゲームランチ
            Lancher.Instance.
        }
    }
}
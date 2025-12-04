using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Launcher
{
    /// <summary>
    /// ゲーム動画セル
    /// </summary>
    public class GameVideoCell : MonoBehaviour
    {
        [Header("ボタン"), SerializeField]
        private Button _button;

        [Header("ビデオプレイヤー"), SerializeField]
        private VideoPlayer _videoPlayer;

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
            // ゲームランチャーのインスタンスを取得
            Launch.Instance.Launching(001);
        }

        /// <summary>
        /// 動画クリップ設定
        /// </summary>
        public void SetMovie()
        {
            _videoPlayer.clip = Resources.Load<VideoClip>($"Videos/Video{gameObject.name}");
        }
    }
}
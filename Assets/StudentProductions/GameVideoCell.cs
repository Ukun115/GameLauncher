using TMPro;
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

        [Header("Raw画像"), SerializeField]
        private RawImage _rawImage;

        [Header("作品名&学生名"), SerializeField]
        private TextMeshProUGUI _productionAndStudentName;

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
            Launch.Instance.Launching(gameObject.name);
        }

        /// <summary>
        /// 動画クリップ設定
        /// </summary>
        public void SetMovie()
        {
            _videoPlayer.clip = Resources.Load<VideoClip>($"Videos/Video{gameObject.name}");

            var renderTexture = new RenderTexture(1920,1080,24);
            _videoPlayer.targetTexture = renderTexture;
            _rawImage.texture = renderTexture;
        }

        /// <summary>
        /// テキスト設定
        /// </summary>
        public void SetText(string productionName,string studentName)
        {
            _productionAndStudentName.text = $"タイトル：{productionName}　開発者：{studentName}";
        }
    }
}
using System.IO;
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

        private int _productionId;
        private string _productionName;
        private string _exeFileId;

        public static string VideoCacheDir => Path.Combine(Application.persistentDataPath, "Videos");
        public static string GetCachePath(int productionId) => Path.Combine(VideoCacheDir, $"{productionId:D3}.mp4");

        private void Awake()
        {
            _button.onClick.AddListener(OnClickedButton);
        }

        /// <summary>
        /// セルを初期化する
        /// </summary>
        public void Initialize(int productionId, string productionName, string studentName, string videoFileId, string exeFileId)
        {
            _productionId = productionId;
            _productionName = productionName;
            _exeFileId = exeFileId;
            _productionAndStudentName.text = $"タイトル：{productionName}　開発者：{studentName}";
            SetupVideo();
        }

        private void OnClickedButton()
        {
            Launch.Instance.Launching(_productionId, _productionName, _exeFileId);
        }

        private void SetupVideo()
        {
            var cachePath = GetCachePath(_productionId);
            if (!File.Exists(cachePath)) return;

            var renderTexture = new RenderTexture(1920, 1080, 24);
            _videoPlayer.targetTexture = renderTexture;
            _rawImage.texture = renderTexture;

            _videoPlayer.source = VideoSource.Url;
            _videoPlayer.url = $"file://{cachePath}";
            _videoPlayer.Play();
        }
    }
}

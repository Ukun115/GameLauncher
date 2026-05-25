using System;
using System.IO;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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
        private string _exeFileId;

        private string VideoCacheDir => Path.Combine(Application.persistentDataPath, "Videos");

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
            _exeFileId = exeFileId;
            _productionAndStudentName.text = $"タイトル：{productionName}　開発者：{studentName}";
            SetupVideo(videoFileId).Forget();
        }

        private void OnClickedButton()
        {
            Launch.Instance.Launching(_productionId, _exeFileId);
        }

        private async UniTaskVoid SetupVideo(string videoFileId)
        {
            if (string.IsNullOrEmpty(videoFileId)) return;

            var cachePath = Path.Combine(VideoCacheDir, $"{_productionId:D3}.mp4");

            if (!File.Exists(cachePath))
            {
                var downloaded = await DownloadVideoAsync(videoFileId, cachePath);
                if (!downloaded) return;
            }

            ApplyVideoToPlayer(cachePath);
        }

        private async UniTask<bool> DownloadVideoAsync(string fileId, string cachePath)
        {
            Directory.CreateDirectory(VideoCacheDir);

            var url = GoogleDriveClient.GetDirectDownloadUrl(fileId);
            Debug.Log($"[GameVideoCell] DL URL: {url}  FileID: {fileId}");
            using var req = UnityWebRequest.Get(url);
            req.downloadHandler = new DownloadHandlerBuffer();

            try
            {
                await req.SendWebRequest().ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameVideoCell] 動画DL失敗: {e.Message}");
                return false;
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[GameVideoCell] 動画DL失敗: {req.error}");
                return false;
            }

            File.WriteAllBytes(cachePath, req.downloadHandler.data);
            return true;
        }

        private void ApplyVideoToPlayer(string localPath)
        {
            var renderTexture = new RenderTexture(1920, 1080, 24);
            _videoPlayer.targetTexture = renderTexture;
            _rawImage.texture = renderTexture;

            _videoPlayer.source = VideoSource.Url;
            _videoPlayer.url = $"file://{localPath}";
            _videoPlayer.Play();
        }
    }
}

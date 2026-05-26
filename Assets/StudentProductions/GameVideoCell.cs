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
        private string _gameGenre;
        private string _gameDescription;
        private string _teamOrSolo;
        private string _numberOfPlayers;
        private string _gameEngine;
        private int _grade;
        private int _graduationYear;
        private string _eventType;

        public static string VideoCacheDir => Path.Combine(Application.persistentDataPath, "Videos");
        public static string GetCachePath(int productionId) => Path.Combine(VideoCacheDir, $"{productionId:D3}.mp4");

        private void Awake()
        {
            _button.onClick.AddListener(OnClickedButton);
        }

        /// <summary>
        /// セルを初期化する
        /// </summary>
        public void Initialize(StudentProductionRow row)
        {
            _productionId    = row.ProductionID;
            _productionName  = row.GameName;
            _exeFileId       = row.ExeFileId;
            _gameGenre       = row.GameGenre;
            _gameDescription = row.GameDescription;
            _teamOrSolo      = row.TeamOrSolo;
            _numberOfPlayers = row.NumberOfPlayers;
            _gameEngine      = row.GameEngine;
            _grade           = row.Grade;
            _graduationYear  = row.GraduationYear;
            _eventType       = row.EventType;
            _productionAndStudentName.text = $"タイトル：{row.GameName}　開発者：{row.StudentName}";
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

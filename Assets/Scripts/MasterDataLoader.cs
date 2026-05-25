using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Launcher
{
    /// <summary>
    /// アプリ起動時にGoogleDriveからマスターデータJSONを取得・キャッシュする
    /// </summary>
    public class MasterDataLoader : MonoBehaviour
    {
        public static MasterDataLoader Instance { get; private set; }

        [SerializeField] private LauncherConfig _config;

        public LauncherConfig Config => _config;
        public StudentProductionRow[] Rows { get; private set; }

        /// <summary>
        /// Rows が確定したときに発火する
        /// </summary>
        public event Action OnLoaded;

        private string CachePath => Path.Combine(Application.persistentDataPath, "masterdata.json");

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            LoadAsync().Forget();
        }

        private async UniTaskVoid LoadAsync()
        {
            // キャッシュがあれば即時反映してUIを素早く表示する
            if (File.Exists(CachePath))
            {
                TryLoadFromCache();
                OnLoaded?.Invoke();
            }

            // Drive から最新を取得してキャッシュを更新
            var json = await DownloadJsonAsync();
            if (json == null) return;

            File.WriteAllText(CachePath, json);

            var parsed = ParseJson(json);
            if (parsed == null) return;

            Rows = parsed;

            // キャッシュがなかった初回、またはデータ更新時に再通知
            OnLoaded?.Invoke();
        }

        private void TryLoadFromCache()
        {
            try
            {
                var json = File.ReadAllText(CachePath);
                Rows = ParseJson(json);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[MasterDataLoader] キャッシュ読み込み失敗: {e.Message}");
            }
        }

        private async UniTask<string> DownloadJsonAsync()
        {
            if (string.IsNullOrEmpty(_config?.MasterDataFileId))
            {
                Debug.LogError("[MasterDataLoader] LauncherConfig.MasterDataFileId が未設定です");
                return null;
            }

            var url = GoogleDriveClient.GetDirectDownloadUrl(_config.MasterDataFileId);

            using var req = UnityWebRequest.Get(url);
            try
            {
                await req.SendWebRequest();
            }
            catch (Exception e)
            {
                Debug.LogError($"[MasterDataLoader] マスターデータDL失敗: {e.Message}");
                return null;
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[MasterDataLoader] マスターデータDL失敗: {req.error}");
                return null;
            }

            return req.downloadHandler.text;
        }

        private static StudentProductionRow[] ParseJson(string json)
        {
            try
            {
                var set = JsonUtility.FromJson<StudentProductionRowSet>(json);
                return set?.rows;
            }
            catch (Exception e)
            {
                Debug.LogError($"[MasterDataLoader] JSONパース失敗: {e.Message}");
                return null;
            }
        }
    }
}

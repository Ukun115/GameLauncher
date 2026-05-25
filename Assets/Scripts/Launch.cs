using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace Launcher
{
    /// <summary>
    /// ゲームをランチする
    /// </summary>
    public class Launch : MonoBehaviour
    {
        /// <summary>
        /// .exeのファイル名
        /// </summary>
        private static readonly string ExeFileName = "Game.exe";

        /// <summary>
        /// Zip拡張子
        /// </summary>
        private static readonly string ZipExtention = ".zip";

        [Header("オーバーレイ画像"), SerializeField]
        private GameObject _overlayImage;

        [Header("ローディングキャンバスオブジェクト"), SerializeField]
        private GameObject _loadingCanvasObj;

        /// <summary>
        /// シングルトン
        /// </summary>
        public static Launch Instance;

        /// <summary>
        /// ローカルのインストール先ルート(ゲーム用)
        /// </summary>
        private string gamesRootDir;

        /// <summary>
        /// ローカルの一時インストール先ルート(zip用)
        /// </summary>
        private string tempDir;

        /// <summary>
        /// プロセス
        /// </summary>
        private Process _process;

        // =========================
        // 進捗通知（UI側で購読する）
        // =========================
        /// <summary>
        /// ダウンロード進捗（0..1）
        /// </summary>
        public event Action<float> OnDownloadProgress;

        /// <summary>
        /// 解凍進捗（0..1）
        /// </summary>
        public event Action<float> OnUnzipProgress;

        /// <summary>
        /// 総合進捗（0..1）
        /// </summary>
        public event Action<float> OnTotalProgress;

        /// <summary>
        /// 状態テキスト（任意）
        /// </summary>
        public event Action<string> OnStatus;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            // シングルトンチェック
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            // ローカルのインストール先
            gamesRootDir = Path.Combine(Application.persistentDataPath, "Games");
            tempDir = Path.Combine(Application.persistentDataPath, "temp");

            // ディレクトリ作成
            Directory.CreateDirectory(gamesRootDir);
            Directory.CreateDirectory(tempDir);
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            // 起動したゲームが終了されたとき
            if (_process != null && _process.HasExited)
            {
                // オーバーレイ非表示
                _overlayImage.SetActive(false);
                _process = null;
            }
        }

        /// <summary>
        /// ランチング
        /// </summary>
        public void Launching(int productionId, string gameName, string exeFileId)
        {
            Debug.Log("ランチ開始");
            LaunchTask(productionId, gameName, exeFileId).Forget();
        }

        /// <summary>
        /// ランチタスク
        /// </summary>
        private async UniTask LaunchTask(int productionId, string gameName, string exeFileId)
        {
            var productionName = $"{productionId:D3}";
            var installDir = Path.Combine(gamesRootDir, productionName);
            var exePath = Path.Combine(installDir, ExeFileName);

            // 既にインストール済みならそのまま起動
            if (File.Exists(exePath))
            {
                Debug.Log("既にDL済。起動。");
                StartProcess(exePath, installDir);
                return;
            }

            // ローディングUI表示
            _loadingCanvasObj.SetActive(true);

            var zipUrl = GoogleDriveClient.GetDirectDownloadUrl(exeFileId);
            Debug.Log($"[Launch] DL URL: {zipUrl}  FileID: {exeFileId}");
            var zipPath = Path.Combine(tempDir, $"{productionName}{ZipExtention}");

            // =========================
            // DL/解凍 進捗の合成
            // =========================
            const float DL_WEIGHT = 0.75f;
            const float UNZIP_WEIGHT = 0.25f;
            float dl = 0f;
            float uz = 0f;

            void ReportTotal()
            {
                OnTotalProgress?.Invoke(dl * DL_WEIGHT + uz * UNZIP_WEIGHT);
            }

            // =========================
            // Zip ダウンロード（進捗取得）
            // =========================
            try
            {
                OnStatus?.Invoke($"ゲームをダウンロード中...\n{gameName}");

                await DownloadToFileWithProgress(
                    zipUrl,
                    zipPath,
                    p =>
                    {
                        dl = p;
                        OnDownloadProgress?.Invoke(p);
                        ReportTotal();
                    },
                    this.GetCancellationTokenOnDestroy()
                );

                Debug.Log("ネットワークリクエスト成功");
            }
            catch (Exception e)
            {
                Debug.LogError($"ネットワークリクエスト失敗: {e.Message}");
                CleanupZipIfExists(zipPath);
                _loadingCanvasObj.SetActive(false);
                return;
            }

            // 既に同名フォルダがあれば削除（再インストール想定）
            if (Directory.Exists(installDir))
            {
                Debug.Log("既にDL済");

                Directory.Delete(installDir, true);
            }

            try
            {
                Debug.Log("Zipファイル展開");

                // =========================
                // Zipファイル展開（進捗取得）
                // =========================
                OnStatus?.Invoke($"ファイルを展開中...\n{gameName}");

                await ExtractZipWithProgress(
                    zipPath,
                    installDir,
                    p =>
                    {
                        uz = p;
                        OnUnzipProgress?.Invoke(p);
                        ReportTotal();
                    },
                    this.GetCancellationTokenOnDestroy()
                );
            }
            catch (Exception)
            {
                if (Directory.Exists(installDir))
                {
                    Debug.Log("既にDL済");

                    Directory.Delete(installDir, true);
                }

                return;
            }
            finally
            {
                // zipは必ず削除
                if (File.Exists(zipPath))
                {
                    Debug.Log("Zipファイル削除");

                    File.Delete(zipPath);
                }
            }

            // exeパス確認
            if (!File.Exists(exePath))
            {
                return;
            }

            Debug.Log("DL完了。起動。");

            // 起動
            StartProcess(exePath, installDir);
        }

        /// <summary>
        /// プロセス起動
        /// </summary>
        private void StartProcess(string exePath, string installDir)
        {
            // プロセス情報
            var processStartInfo = new ProcessStartInfo
            {
                FileName = exePath,             // .exeファイルパス
                WorkingDirectory = installDir,  // インストールしたディレクトリ
                UseShellExecute = false
            };

            // ローディングUI非表示
            _loadingCanvasObj.SetActive(false);

            // オーバーレイ表示
            _overlayImage.SetActive(true);

            // 起動
            _process = Process.Start(processStartInfo);
        }

        /// <summary>
        /// インストール済みかどうか（UIで出し分けに使える）
        /// </summary>
        public bool IsInstalled(int productionId)
        {
            var exePath = Path.Combine(gamesRootDir, $"{productionId:D3}", ExeFileName);
            return File.Exists(exePath);
        }

        /// <summary>
        /// 起動時に未キャッシュの動画を1本ずつ順番にDLする
        /// </summary>
        public async UniTask PrefetchVideosAsync(StudentProductionRow[] rows)
        {
            if (rows == null || rows.Length == 0) return;

            Directory.CreateDirectory(GameVideoCell.VideoCacheDir);

            var pending = new List<(int id, string fileId, string gameName)>();
            foreach (var row in rows)
            {
                if (string.IsNullOrEmpty(row.VideoFileId)) continue;
                if (!File.Exists(GameVideoCell.GetCachePath(row.ProductionID)))
                    pending.Add((row.ProductionID, row.VideoFileId, row.GameName));
            }

            if (pending.Count == 0) return;

            _loadingCanvasObj.SetActive(true);

            var progresses = new float[pending.Count];
            int completed = 0;

            async UniTask DownloadOne(int i)
            {
                var (id, fileId, gameName) = pending[i];
                var cachePath = GameVideoCell.GetCachePath(id);
                var url = GoogleDriveClient.GetDirectDownloadUrl(fileId);
                OnStatus?.Invoke($"動画をダウンロード中... ({completed}/{pending.Count})\n{gameName}");
                try
                {
                    await DownloadToFileWithProgress(url, cachePath, p =>
                    {
                        progresses[i] = p;
                        float sum = 0f;
                        for (int j = 0; j < progresses.Length; j++) sum += progresses[j];
                        OnTotalProgress?.Invoke(sum / progresses.Length);
                    }, this.GetCancellationTokenOnDestroy());
                    completed++;
                    OnStatus?.Invoke($"動画をダウンロード中... ({completed}/{pending.Count})\n{gameName}");
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[Launch] 動画DL失敗 ID:{id:D3} {e.Message}");
                    if (File.Exists(cachePath)) File.Delete(cachePath);
                    progresses[i] = 1f;
                    completed++;
                }
            }

            try
            {
                for (int i = 0; i < pending.Count; i++)
                    await DownloadOne(i);
            }
            finally
            {
                OnTotalProgress?.Invoke(1f);
                _loadingCanvasObj.SetActive(false);
            }
        }


        /// <summary>
        /// ZipをDLしつつ進捗を取る
        /// </summary>
        private async UniTask DownloadToFileWithProgress(
            string url,
            string savePath,
            Action<float> onProgress,
            CancellationToken token)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            using var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET)
            {
                downloadHandler = new FileStreamDownloadHandler(fs),
                disposeDownloadHandlerOnDispose = true
            };

            var op = req.SendWebRequest();
            while (!op.isDone)
            {
                onProgress?.Invoke(Mathf.Max(0f, req.downloadProgress));
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (req.result != UnityWebRequest.Result.Success)
                throw new Exception($"Download failed: {req.error}");

            onProgress?.Invoke(1f);
        }

        /// <summary>
        /// ストリーム受信したデータをファイルへ書く DownloadHandler
        /// </summary>
        private sealed class FileStreamDownloadHandler : DownloadHandlerScript
        {
            private readonly FileStream _fs;

            public FileStreamDownloadHandler(FileStream fs, int bufferSize = 64 * 1024)
                : base(new byte[bufferSize]) => _fs = fs;

            protected override bool ReceiveData(byte[] data, int dataLength)
            {
                if (data == null || dataLength <= 0) return false;
                _fs.Write(data, 0, dataLength);
                return true;
            }

            protected override void CompleteContent() => _fs.Flush();
        }



        /// <summary>
        /// Zipを展開しつつ進捗を取る
        /// </summary>
        private async UniTask ExtractZipWithProgress(
            string zipPath,
            string destDir,
            Action<float> onProgress,
            CancellationToken token)
        {
            Directory.CreateDirectory(destDir);

            using var fs = new FileStream(zipPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var archive = new ZipArchive(fs, ZipArchiveMode.Read);

            // 全エントリの合計バイト（ディレクトリは0）
            long total = 0;
            foreach (var e in archive.Entries)
                total += e.Length;

            long done = 0;

            foreach (var entry in archive.Entries)
            {
                token.ThrowIfCancellationRequested();

                var fullPath = Path.Combine(destDir, entry.FullName);

                // ディレクトリエントリ
                if (string.IsNullOrEmpty(entry.Name))
                {
                    Directory.CreateDirectory(fullPath);
                    continue;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using var entryStream = entry.Open();
                using var outStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);

                var buffer = new byte[64 * 1024];
                int read;
                while ((read = entryStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    await outStream.WriteAsync(buffer, 0, read, token);
                    done += read;

                    if (total > 0)
                        onProgress?.Invoke(Mathf.Clamp01((float)done / total));
                }

                // UI更新しやすいように1フレーム返す
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            onProgress?.Invoke(1f);
        }

        private void CleanupZipIfExists(string zipPath)
        {
            if (File.Exists(zipPath))
            {
                try { File.Delete(zipPath); }
                catch { /* 握りつぶし */ }
            }
        }
    }
}

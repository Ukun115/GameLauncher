using Cysharp.Threading.Tasks;
using System;
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
        /// GitHub リポジトリのベースURL（最後の / は付けない）
        /// </summary>
        private static readonly string GithubBaseUrl = "https://github.com/Ukun115/StudentProductions/releases/download";

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
        public void Launching(string productionName)
        {
            Debug.Log("ランチ開始");

            // 非同期処理発火
            LaunchTask(productionName).Forget();
        }

        /// <summary>
        /// ランチタスク
        /// </summary>
        private async UniTask LaunchTask(string productionName)
        {
            // インストール先フォルダ
            var installDir = Path.Combine(gamesRootDir, productionName);
            var exePath = Path.Combine(installDir, ExeFileName);

            Debug.Log(installDir);
            Debug.Log(exePath);

            // 既にインストール済みならそのまま起動
            if (File.Exists(exePath))
            {
                Debug.Log("既にDL済。起動。");

                StartProcess(exePath, installDir);
                return;
            }

            // ローディングUI表示
            _loadingCanvasObj.SetActive(true);

            // 未インストールなら GitHub から DL → 解凍
            var githubZipUrl = $"{GithubBaseUrl}/{productionName}/{productionName}{ZipExtention}";
            var zipPath = Path.Combine(tempDir, $"{productionName}{ZipExtention}");

            // 作品ごとのキャッシュキー
            var cacheKey = $"RealDownloadUrl_{productionName}";
            string zipUrl;

            // キャッシュがあれば使う
            if (PlayerPrefs.HasKey(cacheKey))
            {
                zipUrl = PlayerPrefs.GetString(cacheKey);
            }
            // なければ解決してキャッシュに保存
            else
            {
                // 最終的なURLを解決
                zipUrl = await ResolveFinalUrl(githubZipUrl);
                // キャッシュ保存
                PlayerPrefs.SetString(cacheKey, zipUrl);
                PlayerPrefs.Save();
            }

            // =========================
            // DL/解凍 進捗の合成（任意）
            // =========================
            const float DL_WEIGHT = 0.75f;
            const float UNZIP_WEIGHT = 0.25f;
            float dl = 0f;
            float uz = 0f;

            void ReportTotal()
            {
                var total = dl * DL_WEIGHT + uz * UNZIP_WEIGHT;
                OnTotalProgress?.Invoke(total);
            }

            // =========================
            // Zip ダウンロード（進捗取得）
            // =========================
            try
            {
                OnStatus?.Invoke("Downloading...");

                // ★重要：githubZipUrl ではなく zipUrl を使う（解決した最終URL）
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
                // TODO:iseki UIでエラー表示
                Debug.LogError($"ネットワークリクエスト失敗: {e.Message}");

                // キャッシュURLが古い可能性があるので、1回だけキャッシュ破棄→再解決→再DL
                PlayerPrefs.DeleteKey(cacheKey);

                CleanupZipIfExists(zipPath);

                try
                {
                    zipUrl = await ResolveFinalUrl(githubZipUrl);
                    PlayerPrefs.SetString(cacheKey, zipUrl);
                    PlayerPrefs.Save();

                    OnStatus?.Invoke("Downloading...");

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
                catch (Exception e2)
                {
                    Debug.LogError($"ネットワークリクエスト失敗: {e2.Message}");
                    CleanupZipIfExists(zipPath);
                    return;
                }
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
                OnStatus?.Invoke("Extracting...");

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
        public bool IsInstalled(string productionName)
        {
            var installDir = Path.Combine(gamesRootDir, productionName);
            var exePath = Path.Combine(installDir, ExeFileName);
            return File.Exists(exePath);
        }

        /// <summary>
        /// 最終的なURLを解決する
        /// </summary>
        /// <param name="url"> URL </param>
        /// <returns> 最終的なURL </returns>
        private async UniTask<string> ResolveFinalUrl(string url)
        {
            // 1バイトだけ取得するリクエストを送る（リダイレクト先を取得するため）
            using var req = UnityWebRequest.Get(url);
            req.SetRequestHeader("Range", "bytes=0-0");
            req.downloadHandler = new DownloadHandlerBuffer();

            // Webリクエスト送信を待つ
            await req.SendWebRequest();

            // エラーチェック
            if (req.result != UnityWebRequest.Result.Success)
            {
                throw new System.Exception($"Resolve failed: {req.error}");
            }

            Debug.Log("ネットワークリクエスト成功");

            // リダイレクト先URLを返す(objects.githubusercontent.com)
            return req.url;
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

            // Content-Length を HEAD で取得（取れないこともある）
            long totalBytes = -1;
            using (var head = UnityWebRequest.Head(url))
            {
                await head.SendWebRequest().ToUniTask(cancellationToken: token);
                if (head.result == UnityWebRequest.Result.Success)
                {
                    var lenStr = head.GetResponseHeader("Content-Length");
                    if (!string.IsNullOrEmpty(lenStr) && long.TryParse(lenStr, out var len))
                        totalBytes = len;
                }
            }

            long receivedBytes = 0;

            using var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
            var handler = new FileStreamDownloadHandler(fs, chunkLen =>
            {
                receivedBytes += chunkLen;
                if (totalBytes > 0)
                    onProgress?.Invoke(Mathf.Clamp01((float)receivedBytes / totalBytes));
            });

            using var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET)
            {
                downloadHandler = handler
            };

            // これは付けても付けなくてもOK（付けるとUnityがhandlerをDisposeする）
            // Disposeをoverrideしてないので問題なし
            req.disposeDownloadHandlerOnDispose = true;

            var op = req.SendWebRequest();

            // totalBytesが取れない場合は目安としてUnityのdownloadProgressを使う
            while (!op.isDone)
            {
                if (totalBytes <= 0)
                    onProgress?.Invoke(req.downloadProgress);

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
            private readonly Action<int> _onChunk;

            public FileStreamDownloadHandler(FileStream fs, Action<int> onChunk, int bufferSize = 64 * 1024)
                : base(new byte[bufferSize])
            {
                _fs = fs;
                _onChunk = onChunk;
            }

            protected override bool ReceiveData(byte[] data, int dataLength)
            {
                if (data == null || dataLength <= 0) return false;

                _fs.Write(data, 0, dataLength);
                _onChunk?.Invoke(dataLength);
                return true;
            }

            protected override void CompleteContent()
            {
                _fs.Flush();
            }
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

using Cysharp.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
        /// Awake
        /// </summary>
        private void Awake()
        {
            // シングルトンチェック
            if(Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            // ローカルのインストール先
            gamesRootDir = Path.Combine(Application.persistentDataPath,"Games");
            tempDir = Path.Combine(Application.persistentDataPath,"temp");

            // ディレクトリ作成
            Directory.CreateDirectory(gamesRootDir);
            Directory.CreateDirectory(tempDir);
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
            var installDir = Path.Combine(gamesRootDir,productionName);
            var exePath = Path.Combine(installDir,ExeFileName);

            Debug.Log(installDir);
            Debug.Log(exePath);

            // 既にインストール済みならそのまま起動
            if(File.Exists(exePath))
            {
                Debug.Log("既にDL済。起動。");

                StartProcess(exePath,installDir);
                return;
            }

            // 未インストールなら GitHub から DL → 解凍
            var zipUrl = $"{GithubBaseUrl}/{productionName}/{productionName}{ZipExtention}";
            var zipPath = Path.Combine(tempDir,$"{productionName}{ZipExtention}");

            using(var req = UnityWebRequest.Get(zipUrl))
            {
                req.downloadHandler = new DownloadHandlerFile(zipPath);

                // UniTaskで送信を待つ
                await req.SendWebRequest();

                if(req.result != UnityWebRequest.Result.Success)
                {
                    // TODO:iseki UIでエラー表示
                    Debug.LogError("ネットワークリクエスト失敗");

                    if(File.Exists(zipPath))
                    {
                        Debug.Log("既にZipDL済。削除する。");

                        File.Delete(zipPath);
                    }

                    return;
                }

                Debug.Log("ネットワークリクエスト成功");
            }

            // 既に同名フォルダがあれば削除（再インストール想定）
            if(Directory.Exists(installDir))
            {
                Debug.Log("既にDL済");

                Directory.Delete(installDir,true);
            }

            try
            {
                Debug.Log("Zipファイル展開");

                // Zipファイル展開
                ZipFile.ExtractToDirectory(zipPath,installDir);
            }
            catch(System.Exception exception)
            {
                if(Directory.Exists(installDir))
                {
                    Debug.Log("既にDL済");

                    Directory.Delete(installDir,true);
                }

                return;
            }
            finally
            {
                // zipは必ず削除
                if(File.Exists(zipPath))
                {
                    Debug.Log("Zipファイル削除");

                    File.Delete(zipPath);
                }
            }

            // exeパス確認
            if(!File.Exists(exePath))
            {
                return;
            }

            Debug.Log("DL完了。起動。");

            // 起動
            StartProcess(exePath,installDir);
        }

        /// <summary>
        /// プロセス起動
        /// </summary>
        private void StartProcess(string exePath,string installDir)
        {
            // プロセス情報
            var processStartInfo = new ProcessStartInfo
            {
                FileName = exePath,             // .exeファイルパス
                WorkingDirectory = installDir,  // インストールしたディレクトリ   
                UseShellExecute = false
            };

            // 起動
            Process.Start(processStartInfo);
        }

        /// <summary>
        /// インストール済みかどうか（UIで出し分けに使える）
        /// </summary>
        public bool IsInstalled(string productionName)
        {
            var installDir = Path.Combine(gamesRootDir,productionName);
            var exePath = Path.Combine(installDir,ExeFileName);
            return File.Exists(exePath);
        }
    }
}
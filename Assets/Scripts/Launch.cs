using System.Diagnostics;
using UnityEngine;
using System.IO;

namespace Launcher
{
    /// <summary>
    /// ゲームをランチする
    /// </summary>
    public class Launch : MonoBehaviour
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        public static Launch Instance;

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
        }

        /// <summary>
        /// ランチング
        /// </summary>
        public void Launching(int productionNum)
        {
            // .exeパス(フルパス)
            var exePath = Path.GetFullPath(
                // Assetsフォルダからの相対パスで指定
                Path.Combine(
                    Application.dataPath,       // Assetsフォルダのパス
                    $"../Builds/{productionNum}/Game.exe"    // 相対パス
                )
            );

            // Game.exeがあるフォルダ
            var exeDir = Path.GetDirectoryName(exePath);

            // プロセス情報設定
            var processStartInfo = new ProcessStartInfo
            {
                FileName = exePath,         // 実行ファイルパス
                WorkingDirectory = exeDir,  // 作業ディレクトリ
                UseShellExecute = false     // シェル機能を使用しない
            };

            // .exe実行
            Process.Start(processStartInfo);
        }
    }
}
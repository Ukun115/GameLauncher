using System.Diagnostics;
using UnityEngine;

namespace Lancher
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
            if (Instance)
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
        public void Launching()
        {
            // .exeパス
            var exePath = $"{Application.dataPath}/.../Builds/";

            // .exe実行
            Process.Start(exePath);
        }
    }
}
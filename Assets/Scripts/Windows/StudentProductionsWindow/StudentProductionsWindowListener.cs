using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Launcher
{
    /// <summary>
    /// 学生作品ウィンドウリスナー
    /// </summary>
    public class StudentProductionsWindowListener : MonoBehaviour
    {
        [Header("カメラマネージャー"), SerializeField]
        private CameraManager _cameraManager;

        /// <summary>
        /// 戻るボタン押下時
        /// </summary>
        public async UniTaskVoid OnClickReturnButton()
        {
            await _cameraManager.MoveToPosition1Task();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 一括DLボタン押下時
        /// </summary>
        public void OnClickBulkDownloadButton()
        {
            BulkDownloadAsync().Forget();
        }

        private async UniTaskVoid BulkDownloadAsync()
        {
            var loader = MasterDataLoader.Instance;
            if (loader?.Rows == null) return;
            await Launch.Instance.PrefetchGamesAsync(loader.Rows);
        }
    }
}
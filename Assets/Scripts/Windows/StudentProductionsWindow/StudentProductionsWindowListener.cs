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
    }
}
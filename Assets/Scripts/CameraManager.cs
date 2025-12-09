using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Launcher
{
    /// <summary>
    /// カメラマネージャー
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        /// <summary>
        /// 回転方向
        /// </summary>
        public enum RotateDirection
        {
            Left,   // 左
            Right   // 右
        }

        /// <summary>
        /// カメラを指定方向に90°回転して完了までawait（最短経路）
        /// </summary>
        private async UniTask RotateCameraAsync(RotateDirection direction,float duration = 0.5f)
        {
            // 現在角度
            var current = transform.eulerAngles;

            // ±90°で方向決定
            var deltaY = (direction == RotateDirection.Right) ? 90f : -90f;
            var target = current + new Vector3(0f,deltaY,0f);

            var tween = transform
                .DORotate(target,duration,RotateMode.Fast)
                .SetEase(Ease.OutQuad); // 緩やかに停止

            await tween.AsyncWaitForCompletion();
        }

        /// <summary>
        /// ポジション1に移動
        /// </summary>
        public async UniTask MoveToPosition1Task()
        {
            // 90度左へ回転
            await RotateCameraAsync(RotateDirection.Left);
        }

        /// <summary>
        /// ポジション2に移動
        /// </summary>
        public async UniTask MoveToPosition2Task()
        {
            // 90度右へ回転
            await RotateCameraAsync(RotateDirection.Right);
        }
    }
}
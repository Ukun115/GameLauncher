using UnityEngine;

namespace Lancher
{
    /// <summary>
    /// カメラマネージャー
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        [Header("カメラのアニメーター"), SerializeField]
        private Animator _cameraAnimator;

        /// <summary>
        /// ポジション1に移動
        /// </summary>
        public void MoveToPosition1()
        {
            _cameraAnimator.SetFloat("Animate", 0);
        }

        /// <summary>
        /// ポジション2に移動
        /// </summary>
        public void MoveToPosition2()
        {
            _cameraAnimator.SetFloat("Animate", 1);
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Launcher
{
    /// <summary>
    /// ロード
    /// </summary>
    public class Load : MonoBehaviour
    {
        [Header("ローディングスライダー"), SerializeField]
        private Slider _slider;

        [Header("ローディングテキスト"), SerializeField]
        private TextMeshProUGUI _progressValueText;

        /// <summary>
        /// アクティブ化時の処理
        /// </summary>
        private void OnEnable()
        {
            // 総合進捗イベント登録
            Launch.Instance.OnTotalProgress += HandleTotalProgress;
        }

        /// <summary>
        /// 非アクティブ化時の処理
        /// </summary>
        private void OnDisable()
        {
            // 総合進捗イベント解除
            Launch.Instance.OnTotalProgress -= HandleTotalProgress;
        }

        /// <summary>
        /// 総合進捗ハンドラ
        /// </summary>
        /// <param name="progress"> 進捗（0.0～1.0） </param>
        private void HandleTotalProgress(float progress)
        {
            // スライダー更新
            // NOTE:iseki 念のためクランプ
            _slider.value = Mathf.Clamp01(progress);

            // テキスト更新
            _progressValueText.text = $"{Mathf.Clamp01(progress) * 100f:0}%";
        }
    }
}
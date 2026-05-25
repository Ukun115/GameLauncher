using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Launcher
{
	/// <summary>
	/// ロード
	/// </summary>
	public class Load : MonoBehaviour
	{
		[Header("ローディングスライダー"),SerializeField]
		private Slider _slider;

        [Header("ローディングテキスト"), SerializeField]
        private TextMeshProUGUI _progressValueText;

        [Header("ステータステキスト"), SerializeField]
        private TextMeshProUGUI _statusText;

        /// <summary>
        /// アクティブ化時の処理
        /// </summary>
        private void OnEnable()
        {
            Launch.Instance.OnTotalProgress += HandleTotalProgress;
            Launch.Instance.OnStatus += HandleStatus;
        }

        /// <summary>
        /// 非アクティブ化時の処理
        /// </summary>
        private void OnDisable()
        {
            Launch.Instance.OnTotalProgress -= HandleTotalProgress;
            Launch.Instance.OnStatus -= HandleStatus;
        }

        private void HandleTotalProgress(float progress)
        {
            _slider.value = Mathf.Clamp01(progress);
            _progressValueText.text = $"{Mathf.Clamp01(progress) * 100f:0}%";
        }

        private void HandleStatus(string status)
        {
            if (_statusText != null)
                _statusText.text = status;
        }
    }
}
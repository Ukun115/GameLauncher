using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lancher
{
	/// <summary>
	/// シーンロード
	/// </summary>
	public class LoadScene : MonoBehaviour
	{
		[Header("メインキャンバスオブジェクト"), SerializeField]
		private GameObject _mainCanvasObj;

		[Header("ローディングメニューオブジェクト"), SerializeField]
		private GameObject _loadingMenuObj;

		[Header("ローディングスライダー"), SerializeField]
		private Slider _loadingSlider;

		[Header("ロードプロンプトテキスト"), SerializeField]
		private Text _loadPromptText;

		[Header("ユーザー入力待機"), SerializeField]
		private bool _waitForInput = true;

		[Header("ユーザープロンプトキー"), SerializeField]
		private KeyCode _userPromptKey = KeyCode.Space;

		/// <summary>
		/// シーンロード
		/// </summary>
		public void Load(string sceneName)
		{
			if (sceneName == "")
			{
				return;
			}

			// 非同期シーンロード開始
			StartCoroutine(LoadAsynchronously(sceneName));
		}

		/// <summary>
		/// 非同期シーンロード
		/// </summary>
		/// <param name="sceneName"> ロードするシーン名 </param>
		private IEnumerator LoadAsynchronously(string sceneName)
		{
			// シーン非同期ロード
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
			// シーンの自動アクティベーションを無効化
			operation.allowSceneActivation = false;

			// ローディング画面表示
			_mainCanvasObj.SetActive(false);
			_loadingMenuObj.SetActive(true);

			// ロード完了まで待機
			while (!operation.isDone)
			{
				// ロード進捗をスライダーに反映
				var progressValue = Mathf.Clamp01(operation.progress / .95f);
				// スライダーの値を更新
				_loadingSlider.value = progressValue;

				if (operation.progress >= 0.9f && _waitForInput)
				{
					// ユーザー入力待機プロンプト表示
					_loadPromptText.text = "Press " + _userPromptKey.ToString().ToUpper() + " to continue";
					// スライダーの値を更新
					_loadingSlider.value = 1;

					// ユーザー入力待機
					if (Input.GetKeyDown(_userPromptKey))
					{
						// シーンアクティベーション許可
						operation.allowSceneActivation = true;
					}
				}
				else if (operation.progress >= 0.9f && !_waitForInput)
				{
					// シーンアクティベーション許可
					operation.allowSceneActivation = true;
				}

				yield return null;
			}
		}
	}
}
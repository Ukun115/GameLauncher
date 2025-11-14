using Lancher;
using UnityEngine;

namespace SlimUI.ModernMenu
{
	/// <summary>
	/// UIメニューマネージャー
	/// </summary>
	public class UIMenuManager : MonoBehaviour
	{
		/// <summary>
		/// テーマ
		/// </summary>
		private enum Theme
		{
			Custom1,    // カスタムプリセット1
			Custom2,    // カスタムプリセット2
			Custom3     // カスタムプリセット3
		};

		[Header("カメラのアニメーター"), SerializeField]
		private Animator _cameraAnimator;

		[Space(50)]

		[Header("メニューのルートオブジェクト"), SerializeField]
		private GameObject _menuRootObj;

		[Header("最初のメニュー"), SerializeField]
		private GameObject _firstMenuObj;

		[Header("プレイメニュー")]
		private GameObject _playMenuObj;

		[Header("終了メニュー"), SerializeField]
		private GameObject _exitMenuObj;

		[Header("エクストラメニュー"), SerializeField]
		private GameObject _extrasMenuObj;

		[Space(50)]

		[Header("テーマ"), SerializeField]
		private Theme _theme;

		[Header("テーマコントローラー"), SerializeField]
		private ThemedUIData _themeController;

		[Header(""), SerializeField]
		private GameObject _panelControlsObj;

		[Header(""), SerializeField]
		private GameObject _panelVideoObj;

		[Header(""), SerializeField]
		private GameObject _panelGameObj;

		[Header(""), SerializeField]
		private GameObject _panelKeyBindingsObj;

		[Header(""), SerializeField]
		private GameObject _panelMovementObj;

		[Header(""), SerializeField]
		private GameObject _panelCombatObj;

		[Header(""), SerializeField]
		private GameObject _panelGeneralObj;

		[Space(50)]

		[Header(""), SerializeField]
		private GameObject _lineGameObj;

		[Header(""), SerializeField]
		private GameObject _lineVideoObj;

		[Header(""), SerializeField]
		private GameObject _lineControlsObj;

		[Header(""), SerializeField]
		private GameObject _lineKeyBindingsObj;

		[Header(""), SerializeField]
		private GameObject _lineMovementObj;

		[Header(""), SerializeField]
		private GameObject _lineCombatObj;

		[Header(""), SerializeField]
		private GameObject _lineGeneralObj;

		/// <summary>
		/// Start
		/// </summary>
		private void Start()
		{
			// 初期化
			Init();
		}

		/// <summary>
		/// 初期化
		/// </summary>
		private void Init()
		{
			_playMenuObj.SetActive(false);
			_exitMenuObj.SetActive(false);
			if (_extrasMenuObj){
				_extrasMenuObj.SetActive(false);
			}
			_firstMenuObj.SetActive(true);
			_menuRootObj.SetActive(true);

			SetThemeColors();
		}

		/// <summary>
		/// テーマカラー設定
		/// </summary>
		private void SetThemeColors()
		{
			// テーマに応じてカラー設定
			switch (_theme)
			{
				// カスタムプリセット1
				case Theme.Custom1:
					_themeController.CurrentColor = _themeController.Custom1.Graphic1;
					_themeController.TextColor = _themeController.Custom1.Text1;
					break;
				// カスタムプリセット2
				case Theme.Custom2:
					_themeController.CurrentColor = _themeController.Custom2.Graphic2;
					_themeController.TextColor = _themeController.Custom2.Text2;
					break;
				// カスタムプリセット3
				case Theme.Custom3:
					_themeController.CurrentColor = _themeController.Custom3.Graphic3;
					_themeController.TextColor = _themeController.Custom3.Text3;
					break;
			}
		}

		/// <summary>
		/// キャンペーンプレイ
		/// </summary>
		public void OnPlayCampaign()
		{
			_exitMenuObj.SetActive(false);
			if (_extrasMenuObj)
			{
				_extrasMenuObj.SetActive(false);
			}
			_playMenuObj.SetActive(true);
		}

		/// <summary>
		/// モバイルでキャンペーンプレイ
		/// </summary>
		public void OnPlayCampaignMobile()
		{
			_exitMenuObj.SetActive(false);
			if (_extrasMenuObj)
			{
				_extrasMenuObj.SetActive(false);
			}
			_playMenuObj.SetActive(true);
			_menuRootObj.SetActive(false);
		}

		/// <summary>
		/// メニューに戻る
		/// </summary>
		public void OnReturnMenu()
		{
			_playMenuObj.SetActive(false);
			if (_extrasMenuObj)
			{
				_extrasMenuObj.SetActive(false);
			}
			_exitMenuObj.SetActive(false);
			_menuRootObj.SetActive(true);
		}

		/// <summary>
		/// キャンペーンプレイ無効化
		/// </summary>
		public void OnDisablePlayCampaign()
		{
			_playMenuObj.SetActive(false);
		}

		/// <summary>
		/// ポジション1
		/// </summary>
		public void OnPosition1()
		{
			_cameraAnimator.SetFloat("Animate", 0);
		}

		/// <summary>
		/// ポジション2
		/// </summary>
		public void OnPosition2()
		{
			OnDisablePlayCampaign();
			_cameraAnimator.SetFloat("Animate", 1);
		}

		/// <summary>
		/// パネル無効化
		/// </summary>
		private void DisablePanels()
		{
			_panelControlsObj.SetActive(false);
			_panelVideoObj.SetActive(false);
			_panelGameObj.SetActive(false);
			_panelKeyBindingsObj.SetActive(false);

			_lineGameObj.SetActive(false);
			_lineControlsObj.SetActive(false);
			_lineVideoObj.SetActive(false);
			_lineKeyBindingsObj.SetActive(false);

			_panelMovementObj.SetActive(false);
			_lineMovementObj.SetActive(false);
			_panelCombatObj.SetActive(false);
			_lineCombatObj.SetActive(false);
			_panelGeneralObj.SetActive(false);
			_lineGeneralObj.SetActive(false);
		}

		/// <summary>
		/// ゲームパネル
		/// </summary>
		public void OnGamePanel()
		{
			DisablePanels();
			_panelGameObj.SetActive(true);
			_lineGameObj.SetActive(true);
		}

		/// <summary>
		/// ビデオパネル
		/// </summary>
		public void OnVideoPanel()
		{
			DisablePanels();
			_panelVideoObj.SetActive(true);
			_lineVideoObj.SetActive(true);
		}

		/// <summary>
		/// コントロールパネル
		/// </summary>
		public void OnControlsPanel()
		{
			DisablePanels();
			_panelControlsObj.SetActive(true);
			_lineControlsObj.SetActive(true);
		}

		/// <summary>
		/// キーバインドパネル
		/// </summary>
		public void OnKeyBindingsPanel()
		{
			DisablePanels();
			OnMovementPanel();
			_panelKeyBindingsObj.SetActive(true);
			_lineKeyBindingsObj.SetActive(true);
		}

		/// <summary>
		/// 移動パネル
		/// </summary>
		public void OnMovementPanel()
		{
			DisablePanels();
			_panelKeyBindingsObj.SetActive(true);
			_panelMovementObj.SetActive(true);
			_lineMovementObj.SetActive(true);
		}

		/// <summary>
		/// 戦闘パネル
		/// </summary>
		public void OnCombatPanel()
		{
			DisablePanels();
			_panelKeyBindingsObj.SetActive(true);
			_panelCombatObj.SetActive(true);
			_lineCombatObj.SetActive(true);
		}

		/// <summary>
		/// 一般パネル
		/// </summary>
		public void OnGeneralPanel()
		{
			DisablePanels();
			_panelKeyBindingsObj.SetActive(true);
			_panelGeneralObj.SetActive(true);
			_lineGeneralObj.SetActive(true);
		}

		/// <summary>
		/// 本当に終了するかメニュー
		/// </summary>
		public void OnAreYouSure()
		{
			_exitMenuObj.SetActive(true);
			if (_extrasMenuObj)
            {
                _extrasMenuObj.SetActive(false);
            }
			OnDisablePlayCampaign();
		}

		/// <summary>
		/// モバイルで本当に終了するかメニュー
		/// </summary>
		public void OnAreYouSureMobile()
		{
			_exitMenuObj.SetActive(true);
			if (_extrasMenuObj)
			{
				_extrasMenuObj.SetActive(false);
			}
			_menuRootObj.SetActive(false);
			OnDisablePlayCampaign();
		}

		/// <summary>
		/// エクストラメニュー
		/// </summary>
		public void OnExtrasMenu()
		{
			_playMenuObj.SetActive(false);
			if (_extrasMenuObj)
			{
				_extrasMenuObj.SetActive(true);
			}
			_exitMenuObj.SetActive(false);
		}

		/// <summary>
		/// ゲーム終了
		/// </summary>
		public void OnQuitGame()
		{
#if UNITY_EDITOR
			// エディター上で実行している場合は再生モードを停止
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
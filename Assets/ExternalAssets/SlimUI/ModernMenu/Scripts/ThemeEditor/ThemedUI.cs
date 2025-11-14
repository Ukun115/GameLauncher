using UnityEngine;

namespace SlimUI.ModernMenu{
	/// <summary>
    /// テーマUI
	/// ExecuteInEditMode　：　Editorでもスクリプトが実行されることを意味します。
	/// System.Serializable　：　クラスのインスタンスがシリアライズ可能になる
    /// </summary>
	[ExecuteInEditMode()]
	[System.Serializable]
	public class ThemedUI : MonoBehaviour {

		/// <summary>
		/// テーマコントローラー
		public ThemedUIData ThemeController;

		/// <summary>
		/// Awake
		/// </summary>
		public virtual void Awake()
		{
			OnSkinUI();
		}

		/// <summary>
		/// Update
		/// </summary>
		public virtual void Update()
		{
			OnSkinUI();
		}

		/// <summary>
		/// スキンUI
		/// </summary>
		protected virtual void OnSkinUI()
		{

		}
	}
}

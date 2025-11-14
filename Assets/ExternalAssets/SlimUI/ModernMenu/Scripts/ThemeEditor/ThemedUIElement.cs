using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	/// <summary>
    /// テーマUI要素
	/// System.Serializable　：　クラスのインスタンスがシリアライズ可能になる
    /// </summary>
	[System.Serializable]
	public class ThemedUIElement : ThemedUI {
		/// <summary>
		/// アウトラインスタイル
		/// </summary>
		public enum OutlineStyle
		{
			solidThin,	// 細線
			solidThick, // 太線
			dottedThin, // 細点線
			dottedThick // 太点線
		};

		[Header("要素")]

		/// <summary>
		/// 画像
		/// </summary>
		private Image _image;

		/// <summary>
		/// メッセージオブジェクト
		/// </summary>
		private GameObject _message;

		/// <summary>
		/// 画像があるか
		/// </summary>
		private bool _hasImage = false;

		/// <summary>
		/// テキストか
		/// </summary>
		private bool _isText = false;

		/// <summary>
		/// スキンUI
		/// </summary>
		protected override void OnSkinUI()
		{
			base.OnSkinUI();

			if (_hasImage)
			{
				// 画像にカラー適用
				_image = GetComponent<Image>();
				_image.color = ThemeController.CurrentColor;
			}

			_message = gameObject;

			if (_isText)
			{
				// テキストにカラー適用
				var text = _message.GetComponent<TextMeshPro>();
				text.color = ThemeController.TextColor;
			}
		}
	}
}
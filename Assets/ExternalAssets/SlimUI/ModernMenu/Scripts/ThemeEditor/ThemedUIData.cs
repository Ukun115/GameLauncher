using UnityEngine;

namespace SlimUI.ModernMenu
{
	/// <summary>
	/// テーマUIデータ
	/// CreateAssetMenu　：　Assets/Createメニューに追加
	/// System.Serializable　：　クラスのインスタンスがシリアライズ可能になる
	/// </summary>
	[CreateAssetMenu(menuName = "テーマ設定")]
	[System.Serializable]
	public class ThemedUIData : ScriptableObject
	{
		/// <summary>
		/// テーマ
		/// </summary>
		public enum Theme
		{
			Custom1,    // カスタムプリセット1
			Custom2,    // カスタムプリセット2
			Custom3     // カスタムプリセット3
		};

		/// <summary>
		/// カスタムプリセット1
		/// </summary>
		[System.Serializable]
		public class CustomPreset1
		{
			[Header("テキスト")]
			public Color Graphic;
			public Color32 Text;
		}

		/// <summary>
		/// カスタムプリセット2
		/// </summary>
		[System.Serializable]
		public class CustomPreset2
		{
			[Header("テキスト")]
			public Color Graphic;
			public Color32 Text;
		}

		/// <summary>
		/// カスタムプリセット3
		/// </summary>
		[System.Serializable]
		public class CustomPreset3
		{
			[Header("テキスト")]
			public Color Graphic;
			public Color32 Text;
		}

		[Header("プリセット1")]
		public CustomPreset1 Custom1;

		[Header("プリセット2")]
		public CustomPreset2 Custom2;

		[Header("プリセット3")]
		public CustomPreset3 Custom3;

		/// <summary>
		/// 現在の色
		/// </summary>
		[HideInInspector]
		public Color CurrentColor;

		/// <summary>
		/// テキストカラー
		/// </summary>
		[HideInInspector]
		public Color32 TextColor;
	}
}
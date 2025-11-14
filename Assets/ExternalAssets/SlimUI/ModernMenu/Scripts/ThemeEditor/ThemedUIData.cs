using UnityEngine;

namespace SlimUI.ModernMenu{
	/// <summary>
    /// テーマUIデータ
	/// CreateAssetMenu　：　Assets/Createメニューに追加
	/// System.Serializable　：　クラスのインスタンスがシリアライズ可能になる
    /// </summary>
	[CreateAssetMenu(menuName = "テーマ設定")]
	[System.Serializable]
	public class ThemedUIData : ScriptableObject {
		/// <summary>
        /// カスタムプリセット1
        /// </summary>
		[System.Serializable]
		public class CustomPreset1{
			[Header("テキスト")]	
			public Color Graphic1;
			public Color32 Text1;
		}

		/// <summary>
		/// カスタムプリセット2
		/// </summary>
		[System.Serializable]
		public class CustomPreset2{
			[Header("テキスト")]	
			public Color Graphic2;
			public Color32 Text2;
		}

		/// <summary>
		/// カスタムプリセット3
		/// </summary>
		[System.Serializable]
		public class CustomPreset3{
			[Header("テキスト")]	
			public Color Graphic3;
			public Color32 Text3;
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
using UnityEngine;
using UnityEditor;

/// <summary>
/// SlimUIのオンラインドキュメントを開くウィンドウ
/// </summary>
namespace SlimUI
{
	public class SlimUIWindow : EditorWindow
	{
		/// <summary>
        /// ウィンドウ表示
        /// </summary>
		[MenuItem("Window/SlimUI/オンラインドキュメント")]
		public static void ShowWindow()
		{
			// URLを開く
			Application.OpenURL("https://www.slimui.com/documentation");
		}
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu{
	/// <summary>
    /// デモをリセット
    /// </summary>
	public class ResetDemo : MonoBehaviour {
		/// <summary>
		/// Update
		/// </summary>
		void Update ()
		{
			// Rキーでシーンリロード
			if(Input.GetKeyDown("r"))
			{
				SceneManager.LoadScene(0);
			}
		}
	}
}
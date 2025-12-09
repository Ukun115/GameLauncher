using UnityEngine;
using UnityEngine.UI;

namespace Launcher
{
    /// <summary>
    /// 学生作品ウィンドウ監視
    /// </summary>
    public class StudentProductionsWindowObserver : MonoBehaviour
    {
        [Header("学生作品ウィンドウリスナー"), SerializeField]
        private StudentProductionsWindowListener _buttonListener;

        [Header("戻るボタン"), SerializeField]
        private Button _returnButton;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            // ボタンリスナー登録
            AddButtonListeners();
        }

        /// <summary>
        /// ボタンリスナー登録
        /// </summary>
        private void AddButtonListeners()
        {
            // 各ボタンクリック時のリスナー登録
            // NOTE:iseki ラムダ式じゃないとリスナー登録時にOnClickReturnButtonメソッドが呼ばれてしまう
            _returnButton.onClick.AddListener(() => _buttonListener.OnClickReturnButton().Forget());
        }
    }
}
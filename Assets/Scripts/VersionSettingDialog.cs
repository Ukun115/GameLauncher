#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Launcher
{
    public class VersionSettingDialog : EditorWindow
    {
        /// <summary>
        /// バージョン
        /// </summary>
        private string _version;

        [MenuItem("更新/手順2.アプリケーションバージョン設定")]
        private static void Open()
        {
            var window = GetWindow<VersionSettingDialog>(true,"アプリケーションバージョン設定",true);
            // 現在値を初期表示
            window._version = PlayerSettings.bundleVersion;
            window.minSize = new Vector2(300,90);
            window.ShowUtility();
        }

        /// <summary>
        /// GUI表示
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.LabelField("アプリケーションバージョンを変更。");

            _version = EditorGUILayout.TextField("バージョン(例: 1.5.0)",_version);

            GUILayout.Space(10);

            if(GUILayout.Button("適用"))
            {
                Apply();
            }
        }

        /// <summary>
        /// 適用
        /// </summary>
        private void Apply()
        {
            if(string.IsNullOrWhiteSpace(_version))
            {
                Debug.LogError("バージョンが空です。");
                return;
            }

            // バージョン更新
            PlayerSettings.bundleVersion = _version;

            Debug.Log($"Application.versionを{_version}に更新しました");
            Close();
        }
    }
}

#endif
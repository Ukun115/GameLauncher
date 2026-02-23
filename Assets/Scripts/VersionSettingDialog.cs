#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Launcher
{
    public class VersionSettingDialog : EditorWindow
    {
        private string version;

        [MenuItem("更新/手順2.アプリケーションバージョン設定")]
        private static void Open()
        {
            var window = GetWindow<VersionSettingDialog>(true,"アプリケーションバージョン設定",true);
            window.version = PlayerSettings.bundleVersion; // 現在値を初期表示
            window.minSize = new Vector2(300,90);
            window.ShowUtility();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("アプリケーションバージョンを変更。");

            version = EditorGUILayout.TextField("バージョン(例: 1.5.0)",version);

            GUILayout.Space(10);

            if(GUILayout.Button("適用"))
            {
                Apply();
            }
        }

        private void Apply()
        {
            if(string.IsNullOrWhiteSpace(version))
            {
                Debug.LogError("バージョンが空です。");
                return;
            }

            PlayerSettings.bundleVersion = version;

            Debug.Log($"Application.versionを{version}に更新しました");
            Close();
        }
    }
}

#endif
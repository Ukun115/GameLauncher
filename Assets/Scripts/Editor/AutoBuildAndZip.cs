namespace Launcher
{
    using System;
    using System.IO;
    using System.Linq;
    using System.IO.Compression;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// ビルドと zip 作成を自動化するエディタ拡張
    /// </summary>
    public static class AutoBuildAndZip
    {
        private const string MenuPath = "更新/手順3.ビルド→バイナリ.zip作成";
        private static readonly string ProductFolderPrefix = "GameLauncher_v";

        [MenuItem(MenuPath)]
        public static void BuildAndCreateZip()
        {
            // 1) Version 取得（ProjectSettings > Player > Version）
            var version = PlayerSettings.bundleVersion;
            if(string.IsNullOrWhiteSpace(version))
                version = "0.0.0";

            // 2) Downloads 配下に出す
            var downloadsPath = GetDownloadsPath();
            if(string.IsNullOrEmpty(downloadsPath) || !Directory.Exists(downloadsPath))
            {
                Debug.LogError($"Downloads フォルダが見つかりませんでした: {downloadsPath}");
                return;
            }

            // 3) 出力フォルダ: Downloads/GameLauncher_v#.#.#/
            var outRootFolderName = $"{ProductFolderPrefix}{version}";
            var outRootPath = Path.Combine(downloadsPath,outRootFolderName);

            // 4) シーン収集（Build Settings の有効シーン）
            var scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            if(scenes.Length == 0)
            {
                EditorUtility.DisplayDialog("ビルド失敗","Build Settings に有効なシーンがありません。","OK");
                return;
            }

            // 5) 出力先を作り直す（必要なら既存を削除）
            try
            {
                if(Directory.Exists(outRootPath))
                    Directory.Delete(outRootPath,true);
                Directory.CreateDirectory(outRootPath);
            }
            catch(Exception e)
            {
                Debug.LogError($"出力フォルダの作成/削除に失敗: {outRootPath}\n{e}");
                return;
            }

            // 6) プラットフォーム別の出力パス決定（BuildPipeline は「ファイルパス」を要求する場合あり）
            var target = EditorUserBuildSettings.activeBuildTarget;
            var locationPathName = GetLocationPathName(outRootPath,target);

            // 7) ビルド実行
            try
            {
                EditorUtility.DisplayProgressBar("Auto Build","Building player...",0.3f);

                var options = new BuildPlayerOptions
                {
                    scenes = scenes,
                    target = target,
                    locationPathName = locationPathName,
                    options = BuildOptions.None
                };

                var report = BuildPipeline.BuildPlayer(options);
                if(report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
                {
                    EditorUtility.ClearProgressBar();
                    EditorUtility.DisplayDialog("ビルド失敗",$"BuildResult: {report.summary.result}","OK");
                    return;
                }
            }
            catch(Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError($"ビルド中に例外が発生:\n{e}");
                return;
            }

            // 8) zip 作成（同じ Downloads 内・同じフォルダ階層に置く）
            var zipPath = Path.Combine(downloadsPath,$"{outRootFolderName}.zip");
            try
            {
                EditorUtility.DisplayProgressBar("Auto Build","Creating zip...",0.7f);

                if(File.Exists(zipPath))
                    File.Delete(zipPath);

                // outRootPath フォルダを zip 化
                ZipFile.CreateFromDirectory(
                    outRootPath,
                    zipPath,
                    System.IO.Compression.CompressionLevel.Optimal,
                    false
                );
            }
            catch(Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError($"zip 作成に失敗: {zipPath}\n{e}");
                return;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            // zip 化が成功したら、ビルド出力のフォルダは不要なので削除
            Directory.Delete(outRootPath,true);

            Debug.Log($"Build Output: {outRootPath}");
            Debug.Log($"Zip Output  : {zipPath}");

            // 9) 出力先を開く
            EditorUtility.RevealInFinder(zipPath);
            EditorUtility.DisplayDialog("完了",$"ビルド & zip 作成が完了しました。\n\n{zipPath}","OK");
        }

        private static string GetDownloadsPath()
        {
            // Windows/Mac/Linux の一般的な Downloads を狙う
            // ユーザー環境で変わる可能性はありますが、まずはここで。
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if(string.IsNullOrEmpty(home))
                return null;

            return Path.Combine(home,"Downloads");
        }

        private static string GetLocationPathName(string outRootPath,BuildTarget target)
        {
            // 「GameLauncher_v#.#.# の直下にビルドデータ」という要件に合わせて
            // outRootPath の直下に実行物が来るようにする
            // ※ BuildTarget によって locationPathName の形式が違う

            // 製品名は PlayerSettings.productName から取る（空ならフォルダ名）
            var productName = string.IsNullOrWhiteSpace(PlayerSettings.productName)
                ? Path.GetFileName(outRootPath)
                : PlayerSettings.productName;

            switch(target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return Path.Combine(outRootPath,$"{productName}.exe");

                case BuildTarget.StandaloneOSX:
                    // macOS は .app（フォルダ扱い）を outRootPath 直下に生成
                    return Path.Combine(outRootPath,$"{productName}.app");

                case BuildTarget.StandaloneLinux64:
                    // Linux は拡張子なしが一般的
                    return Path.Combine(outRootPath,productName);

                default:
                    // それ以外はフォルダ出力が必要なケースもあるので、フォルダを返してみる
                    // （必要に応じて target ごとに分岐追加してください）
                    return Path.Combine(outRootPath,productName);
            }
        }
    }
}
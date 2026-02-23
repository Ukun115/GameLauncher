// NOTE:iseki ビルド時にコンパイル対象から外れる
#if UNITY_EDITOR

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Launcher
{
    /// <summary>
    /// 学生作品インポーター
    /// </summary>
    public static class StudentProductionsSingleAssetImporter
    {
        /// <summary>
        /// 出力先ディレクトリ
        /// </summary>
        private static readonly string OutputDir = "Assets/StudentProductions/MasterData";

        /// <summary>
        /// 出力先アセットパス
        /// </summary>
        private static readonly string OutputAssetPass = $"{OutputDir}/StudentProductionsMaster.asset";

        /// <summary>
        /// メニューから、Jsonファイルから学生作品情報をインポート
        /// </summary>
        [MenuItem("学生作品情報を生成/Jsonファイルから学生作品データを生成")]
        public static void ImportFromJson()
        {
            // ファイル選択ダイアログを開き、Jsonファイルを選択
            var path = EditorUtility.OpenFilePanel("学生作品(Jsonファイル)を選んでください。",Application.dataPath,"json");

            if(string.IsNullOrEmpty(path))
            {
                return;
            }

            // Jsonファイルを読み込み、studentProductionsにパース
            var jsonFile = File.ReadAllText(path);
            var studentProductions = JsonUtility.FromJson<StudentProductions>(jsonFile);
            if(studentProductions == null || studentProductions.Rows == null)
            {
                Debug.LogError("Jsonのパースに失敗したか、「行(Rows)」が見つかりませんでした。");
                return;
            }

            // エクスポート可能の場合ScriptableObjectへエクスポート
            if(CanExport(studentProductions.Rows))
            {
                ExportScriptableObject(studentProductions.Rows);
            }
        }

        /// <summary>
        /// エクスポート可能かをチェック
        /// </summary>
        /// <param name="rows"> 行 </param>
        private static bool CanExport(StudentProductionRow[] rows)
        {
            // ID：重複チェックと正の整数チェック
            var dup = rows.GroupBy(r => r.ProductionID).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
            if(dup.Length > 0)
            {
                Debug.LogError($"IDに重複がありました。: {string.Join(", ",dup)}");
                return false;
            }
            if(rows.Any(r => r.ProductionID <= 0))
            {
                Debug.LogError("IDは正の整数である必要があります。");
                return false;
            }

            // ディレクトリなければ作成
            if(!AssetDatabase.IsValidFolder("Assets/StudentProductions"))
            {
                AssetDatabase.CreateFolder("Assets","StudentProductions");
            }
            if(!AssetDatabase.IsValidFolder(OutputDir))
            {
                AssetDatabase.CreateFolder("Assets/StudentProductions","MasterData");
            }

            return true;
        }

        /// <summary>
        /// ScriptableObjectへエクスポート
        /// </summary>
        private static void ExportScriptableObject(StudentProductionRow[] rows)
        {
            // 既にアセットが存在する場合はロード、存在しない場合は新規作成
            var scriptableObject = AssetDatabase.LoadAssetAtPath<StudentProductionsMaster>(OutputAssetPass);
            if(scriptableObject == null)
            {
                scriptableObject = ScriptableObject.CreateInstance<StudentProductionsMaster>();
                AssetDatabase.CreateAsset(scriptableObject,OutputAssetPass);
            }

            // ScriptableObjectにデータをセットして保存
            scriptableObject.SetAll(rows);
            EditorUtility.SetDirty(scriptableObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"{rows.Length}個の作品データを{OutputAssetPass}へエクスポートしました。");
        }
    }
}

#endif
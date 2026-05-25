// NOTE:iseki ビルド時にコンパイル対象から外れる
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

namespace Launcher
{
    /// <summary>
    /// 学生作品インポーター
    /// </summary>
    public static class StudentProductionsSingleAssetImporter
    {
        private const string OUTPUT_DIR = "Assets/StudentProductions/MasterData";
        private const string OUTPUT_ASSET = OUTPUT_DIR + "/StudentProductionsMaster.asset";

        [MenuItem("Master/Import StudentProductions (Single) from JSON")]
        public static void ImportFromJson()
        {
            var path = EditorUtility.OpenFilePanel("Select StudentProductions.json",Application.dataPath,"json");
            if(string.IsNullOrEmpty(path)) return;
            var json = File.ReadAllText(path);
            var set = JsonUtility.FromJson<StudentProductionRowSet>(json);
            if(set == null || set.rows == null)
            {
                Debug.LogError("JSON parse failed or 'rows' missing.");
                return;
            }
            ImportRows(set.rows);
        }

        [MenuItem("Master/Import StudentProductions (Single) from CSV")]
        public static void ImportFromCsv()
        {
            var path = EditorUtility.OpenFilePanel("Select StudentProductions.csv",Application.dataPath,"csv");
            if(string.IsNullOrEmpty(path)) return;
            var rows = StudentProductionImporter_Util.ReadCsv(path);
            ImportRows(rows);
        }

        private static void ImportRows(StudentProductionRow[] rows)
        {
            var dup = rows.GroupBy(r => r.ProductionID).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
            if(dup.Length > 0)
            {
                Debug.LogError($"Duplicate ProductionID: {string.Join(", ",dup)}");
                return;
            }
            if(rows.Any(r => r.ProductionID <= 0))
            {
                Debug.LogError("ProductionID must be positive.");
                return;
            }

            if(!AssetDatabase.IsValidFolder("Assets/StudentProductions"))
                AssetDatabase.CreateFolder("Assets","StudentProductions");
            if(!AssetDatabase.IsValidFolder(OUTPUT_DIR))
                AssetDatabase.CreateFolder("Assets/StudentProductions","MasterData");

            var so = AssetDatabase.LoadAssetAtPath<StudentProductionsMaster>(OUTPUT_ASSET);
            if(so == null)
            {
                so = ScriptableObject.CreateInstance<StudentProductionsMaster>();
                AssetDatabase.CreateAsset(so,OUTPUT_ASSET);
            }

            so.SetAll(rows);
            EditorUtility.SetDirty(so);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Imported {rows.Length} rows into {OUTPUT_ASSET}");
        }
    }
}

#endif
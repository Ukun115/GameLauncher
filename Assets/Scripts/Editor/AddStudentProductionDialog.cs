using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Launcher
{
    public class AddStudentProductionDialog : EditorWindow
    {
        // ▼ 動画を格納したいフォルダ（必要に応じて変更）
        private const string VIDEO_TARGET_FOLDER = "Assets/Resources/Videos";

        // ▼ 追記するJson（StreamingAssetsに移動した前提）
        private const string FIXED_JSON_PATH = "Assets/StreamingAssets/StudentProductionMaster.json";

        // ▼ 固定候補（プルダウン）
        private static readonly string[] EVENT_TYPES =
        {
            "就活作品",
            "UnityGameJam",
            "UEぷちコン",
            "カッパ杯Winter",
            "カッパ杯Summer",
        };

        private static readonly string[] TEAM_OR_SOLO =
        {
            "チーム",
            "個人",
        };

        private static readonly string[] GAME_ENGINES =
        {
            "DirectX12",
            "Unity",
            "UE",
        };

        private const int GRAD_YEAR_MIN = 2026;
        private const int GRAD_YEAR_MAX = 2100;

        // 外部動画（Project外の絶対パス）
        private string externalVideoPath;

        // 追加する行
        private StudentProductionRow newRow = new StudentProductionRow();

        // 日付入力用
        private DateTime updateDate = DateTime.Today;

        // 選択インデックス（Event/Team/Engine）
        private int eventTypeIndex = 0;
        private int teamOrSoloIndex = 0;
        private int engineIndex = 0;

        // 卒業年（AdvancedDropdown）
        private AdvancedDropdownState gradYearDropdownState;
        private GraduationYearDropdown gradYearDropdown;

        // プレビュー
        private int previewNextId = -1;
        private string previewVideoAssetPath = "";

        private Vector2 scroll;

        [MenuItem("更新/手順1.学生作品マスターデータ更新")]
        private static void Open()
        {
            var w = GetWindow<AddStudentProductionDialog>(true,"追加する学生作品の指定",true);
            w.minSize = new Vector2(600,580);
            w.ShowUtility();
        }

        private void OnEnable()
        {
            EnsureFolderExists("Assets/StreamingAssets");

            // 初期値
            if(newRow.GraduationYear <= 0) newRow.GraduationYear = GRAD_YEAR_MIN;
            if(string.IsNullOrEmpty(newRow.UpdateDate)) newRow.UpdateDate = DateTime.Today.ToString("yyyy/MM/dd");

            if(DateTime.TryParse(newRow.UpdateDate,out var parsed))
                updateDate = parsed.Date;

            // AdvancedDropdown 初期化
            gradYearDropdownState = new AdvancedDropdownState();
            gradYearDropdown = new GraduationYearDropdown(
                gradYearDropdownState,
                GRAD_YEAR_MIN,
                GRAD_YEAR_MAX,
                selectedYear =>
                {
                    newRow.GraduationYear = selectedYear;
                    Repaint();
                });

            RefreshPreview();
        }

        private void OnGUI()
        {
            // ▼ 外部動画選択（mp4 only）
            using(new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("動画ファイル",GUILayout.Width(90));

                EditorGUILayout.SelectableLabel(
                    string.IsNullOrEmpty(externalVideoPath) ? "未選択" : externalVideoPath,
                    GUILayout.Height(18));

                if(GUILayout.Button("ファイル選択",GUILayout.Width(70)))
                {
                    string path = EditorUtility.OpenFilePanel("動画ファイルを選択（mp4のみ）","","mp4");
                    if(!string.IsNullOrEmpty(path))
                    {
                        externalVideoPath = path;
                        RefreshPreview();
                    }
                }
            }

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Jsonへ追加する作品情報",EditorStyles.boldLabel);

            using(new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.IntField("ProductionID（自動）",previewNextId);
                EditorGUILayout.TextField("コピー先動画（自動）",previewVideoAssetPath);
            }

            EditorGUILayout.Space(6);

            scroll = EditorGUILayout.BeginScrollView(scroll);

            newRow.GameName = EditorGUILayout.TextField("ゲーム名",newRow.GameName);
            newRow.StudentName = EditorGUILayout.TextField("学生名",newRow.StudentName);

            // ▼ 卒業年（AdvancedDropdown + 検索 + ホイール）
            DrawGraduationYearAdvancedDropdown();

            // ▼ イベント種別（プルダウン）
            eventTypeIndex = Mathf.Clamp(eventTypeIndex,0,EVENT_TYPES.Length - 1);
            eventTypeIndex = EditorGUILayout.Popup("イベント種別",eventTypeIndex,EVENT_TYPES);
            newRow.EventType = EVENT_TYPES[eventTypeIndex];

            // ▼ チーム/個人（プルダウン）
            teamOrSoloIndex = Mathf.Clamp(teamOrSoloIndex,0,TEAM_OR_SOLO.Length - 1);
            teamOrSoloIndex = EditorGUILayout.Popup("チーム/個人",teamOrSoloIndex,TEAM_OR_SOLO);
            newRow.TeamOrSolo = TEAM_OR_SOLO[teamOrSoloIndex];

            // ▼ ゲームジャンル（自由入力）
            newRow.GameGenre = EditorGUILayout.TextField("ゲームジャンル",newRow.GameGenre);

            // ▼ エンジン（プルダウン）
            engineIndex = Mathf.Clamp(engineIndex,0,GAME_ENGINES.Length - 1);
            engineIndex = EditorGUILayout.Popup("ゲームエンジン",engineIndex,GAME_ENGINES);
            newRow.GameEngine = GAME_ENGINES[engineIndex];

            // ▼ 説明文
            EditorGUILayout.LabelField("説明文");
            newRow.GameDescription = EditorGUILayout.TextArea(newRow.GameDescription,GUILayout.MinHeight(90));

            // ▼ 更新日（テキスト＋今日ボタン）
            using(new EditorGUILayout.HorizontalScope())
            {
                string dateStr = EditorGUILayout.TextField("更新日 (yyyy/MM/dd)",updateDate.ToString("yyyy/MM/dd"));

                if(GUILayout.Button("今日",GUILayout.Width(50)))
                {
                    updateDate = DateTime.Today;
                }

                if(DateTime.TryParse(dateStr,out var parsed))
                {
                    updateDate = parsed.Date;
                }
            }
            newRow.UpdateDate = updateDate.ToString("yyyy/MM/dd");

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(10);

            if(GUILayout.Button("ID/コピー先を再計算"))
            {
                RefreshPreview();
            }

            EditorGUILayout.Space(10);

            string err = Validate();
            if(!string.IsNullOrEmpty(err))
                EditorGUILayout.HelpBox(err,MessageType.Error);

            EditorGUILayout.Space();
            using(new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if(GUILayout.Button("キャンセル",GUILayout.Width(100)))
                {
                    Close();
                }

                using(new EditorGUI.DisabledScope(!string.IsNullOrEmpty(err)))
                {
                    if(GUILayout.Button("実行",GUILayout.Width(100)))
                    {
                        Run();
                        Close();
                    }
                }
            }
        }

        private void DrawGraduationYearAdvancedDropdown()
        {
            using(new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("卒業年",GUILayout.Width(EditorGUIUtility.labelWidth - 4));

                string label = (newRow.GraduationYear <= 0) ? "選択してください" : newRow.GraduationYear.ToString();

                if(GUILayout.Button(label,EditorStyles.popup))
                {
                    Rect rect = GUILayoutUtility.GetLastRect();
                    rect = new Rect(rect.x,rect.y + 18,rect.width,rect.height);
                    gradYearDropdown.Show(rect);
                }
            }

            // ▼ ホイールで増減（Shift=10年, Ctrl/Command=5年）
            Rect last = GUILayoutUtility.GetLastRect();
            Event e = Event.current;
            if(e.type == EventType.ScrollWheel && last.Contains(e.mousePosition))
            {
                int delta = (e.delta.y > 0f) ? 1 : -1;
                if(e.shift) delta *= 10;
                else if(e.control || e.command) delta *= 5;

                int current = Mathf.Clamp(newRow.GraduationYear <= 0 ? GRAD_YEAR_MIN : newRow.GraduationYear,GRAD_YEAR_MIN,GRAD_YEAR_MAX);
                newRow.GraduationYear = Mathf.Clamp(current + delta,GRAD_YEAR_MIN,GRAD_YEAR_MAX);

                e.Use();
                GUI.changed = true;
                Repaint();
            }
        }

        private string Validate()
        {
            if(string.IsNullOrEmpty(externalVideoPath))
                return "動画ファイルを選択してください。";

            if(!File.Exists(externalVideoPath))
                return "動画ファイルが存在しません。";

            // mp4 only
            var ext = Path.GetExtension(externalVideoPath)?.ToLowerInvariant();
            if(ext != ".mp4")
                return "動画は mp4 のみ対応です。";

            if(previewNextId <= 0)
                return "ProductionIDの計算に失敗しました。Jsonが壊れている可能性があります。";

            if(string.IsNullOrWhiteSpace(newRow.GameName))
                return "ゲーム名を入力してください。";

            if(string.IsNullOrWhiteSpace(newRow.StudentName))
                return "学生名を入力してください。";

            if(newRow.GraduationYear < GRAD_YEAR_MIN || newRow.GraduationYear > GRAD_YEAR_MAX)
                return $"卒業年は {GRAD_YEAR_MIN}〜{GRAD_YEAR_MAX} の範囲で選択してください。";

            if(HasDuplicateIdInJson(previewNextId))
                return $"ProductionID={previewNextId} が既にJsonに存在します。「ID/コピー先を再計算」を押してください。";

            if(VideoFileExistsById(previewNextId))
                return $"Video{previewNextId:000}.mp4 が既に存在します。「ID/コピー先を再計算」を押してください。";

            return null;
        }

        private void Run()
        {
            EnsureFolderExists("Assets/StreamingAssets");
            EnsureFolderExists(VIDEO_TARGET_FOLDER);

            // 確定ID
            newRow.ProductionID = previewNextId;

            // ▼ 動画コピー（必ず mp4）
            string fileName = $"Video{newRow.ProductionID:000}.mp4";
            string destAssetPath = Path.Combine(VIDEO_TARGET_FOLDER,fileName).Replace("\\","/");

            string destAbsPath = ToAbsolutePath(destAssetPath);
            Directory.CreateDirectory(Path.GetDirectoryName(destAbsPath));
            File.Copy(externalVideoPath,destAbsPath,false);

            AssetDatabase.ImportAsset(destAssetPath);
            AssetDatabase.Refresh();

            // ▼ Jsonへ追記（StreamingAssets）
            AppendRowToJson(newRow);

            Debug.Log($"学生作品追加完了: ID={newRow.ProductionID} / {newRow.GameName}");
        }

        private void RefreshPreview()
        {
            try
            {
                previewNextId = GetNextAvailableProductionId();
                previewVideoAssetPath = $"{VIDEO_TARGET_FOLDER}/Video{previewNextId:000}.mp4";
            }
            catch(Exception e)
            {
                previewNextId = -1;
                previewVideoAssetPath = "";
                Debug.LogError(e);
            }
        }

        private static void EnsureFolderExists(string folder)
        {
            if(AssetDatabase.IsValidFolder(folder)) return;

            // "Assets/A/B/C" を順に作る
            string[] parts = folder.Split('/');
            string current = parts[0]; // Assets
            for(int i = 1;i < parts.Length;i++)
            {
                string next = current + "/" + parts[i];
                if(!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current,parts[i]);
                current = next;
            }
        }

        private static string ToAbsolutePath(string assetPath)
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string abs = Path.Combine(projectRoot,assetPath);
            return Path.GetFullPath(abs);
        }

        private static bool HasDuplicateIdInJson(int id)
        {
            try
            {
                var data = LoadJson();
                if(data?.Rows == null) return false;
                return data.Rows.Any(r => r != null && r.ProductionID == id);
            }
            catch
            {
                return false;
            }
        }

        private static bool VideoFileExistsById(int id)
        {
            try
            {
                var absFolder = ToAbsolutePath(VIDEO_TARGET_FOLDER);
                if(!Directory.Exists(absFolder)) return false;

                // mp4 only
                var file = $"Video{id:000}.mp4";
                return File.Exists(Path.Combine(absFolder,file));
            }
            catch
            {
                return false;
            }
        }

        private static int GetNextAvailableProductionId()
        {
            int baseId = GetNextProductionIdFromJson();

            int id = baseId;
            while(true)
            {
                if(!HasDuplicateIdInJson(id) && !VideoFileExistsById(id))
                    return id;
                id++;
            }
        }

        private static int GetNextProductionIdFromJson()
        {
            var data = LoadJson();
            if(data?.Rows == null || data.Rows.Length == 0) return 1;

            int max = 0;
            foreach(var r in data.Rows)
            {
                if(r != null && r.ProductionID > max)
                    max = r.ProductionID;
            }
            return max + 1;
        }

        private static StudentProductions LoadJson()
        {
            // StreamingAssets の Json は Editor でも普通にファイルとして読める
            if(!File.Exists(FIXED_JSON_PATH))
            {
                // 無ければ空データとして扱う（初回）
                return new StudentProductions { Rows = Array.Empty<StudentProductionRow>() };
            }

            var json = File.ReadAllText(FIXED_JSON_PATH);
            return JsonUtility.FromJson<StudentProductions>(json) ?? new StudentProductions { Rows = Array.Empty<StudentProductionRow>() };
        }

        private static void AppendRowToJson(StudentProductionRow row)
        {
            if(row == null) throw new ArgumentNullException(nameof(row));

            var data = LoadJson();
            var list = new List<StudentProductionRow>();
            if(data.Rows != null) list.AddRange(data.Rows);

            // 追記
            list.Add(new StudentProductionRow
            {
                ProductionID = row.ProductionID,
                GameName = row.GameName ?? "",
                StudentName = row.StudentName ?? "",
                GraduationYear = row.GraduationYear,
                EventType = row.EventType ?? "",
                TeamOrSolo = row.TeamOrSolo ?? "",
                GameGenre = row.GameGenre ?? "",
                GameEngine = row.GameEngine ?? "",
                GameDescription = row.GameDescription ?? "",
                UpdateDate = row.UpdateDate ?? "",
            });

            data.Rows = list.ToArray();

            var outJson = JsonUtility.ToJson(data,true);
            File.WriteAllText(FIXED_JSON_PATH,outJson);

            AssetDatabase.ImportAsset(FIXED_JSON_PATH);
            AssetDatabase.Refresh();
        }

        // ----------------------------
        // AdvancedDropdown
        // ----------------------------
        internal sealed class GraduationYearDropdown : AdvancedDropdown
        {
            private readonly int _minYear;
            private readonly int _maxYear;
            private readonly Action<int> _onSelected;

            public GraduationYearDropdown(AdvancedDropdownState state,int minYear,int maxYear,Action<int> onSelected)
                : base(state)
            {
                _minYear = minYear;
                _maxYear = maxYear;
                _onSelected = onSelected;
                minimumSize = new Vector2(240,320);
            }

            protected override AdvancedDropdownItem BuildRoot()
            {
                var root = new AdvancedDropdownItem("卒業年を選択（検索できます）");

                var buckets = new Dictionary<int,AdvancedDropdownItem>();
                for(int y = _minYear;y <= _maxYear;y++)
                {
                    int decade = (y / 10) * 10;
                    if(!buckets.TryGetValue(decade,out var bucket))
                    {
                        bucket = new AdvancedDropdownItem($"{decade}s");
                        buckets.Add(decade,bucket);
                        root.AddChild(bucket);
                    }
                    bucket.AddChild(new AdvancedDropdownItem(y.ToString()) { id = y });
                }

                return root;
            }

            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                if(item != null && item.id >= _minYear && item.id <= _maxYear)
                    _onSelected?.Invoke(item.id);
            }
        }
    }
}
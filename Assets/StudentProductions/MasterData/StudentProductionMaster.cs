using UnityEngine;
using System;
using System.Collections.Generic;

namespace Launcher
{
    /// <summary>
    /// 学生制作物マスターデータ（単体）
    /// </summary>
    public class StudentProductionsMaster : ScriptableObject
    {
        /// <summary>
        /// 学生制作物のエントリー
        /// </summary>
        [Serializable]
        public class Entry
        {
            public int ProductionID;
            public string ProductionName;
            public string StudentName;
            public int GraduationYear;
            public string EventType;
            public string TeamOrSolo;
            public string GameGenre;
            public string GameEngine;
            public string GameDescription;
            public string UpdateDate;
        }

        [SerializeField]
        private List<Entry> _entries = new();

        private Dictionary<int,Entry> _index;

        public IReadOnlyList<Entry> Entries => _entries;

        /// <summary>
        /// 学生制作物のエントリーを全てセットする
        /// </summary>
        /// <param name="rows"> 行 </param>
        public void SetAll(StudentProductionRow[] rows)
        {
            _entries.Clear();
            if(rows == null)
            {
                return;
            }

            foreach(var r in rows)
            {
                _entries.Add(new Entry
                {
                    ProductionID = r.ProductionID,
                    StudentName = r.StudentName ?? string.Empty,
                    ProductionName = r.GameName ?? string.Empty,
                    GraduationYear = r.GraduationYear,
                    EventType = r.EventType ?? string.Empty,
                    TeamOrSolo = r.TeamOrSolo ?? string.Empty,
                    GameGenre = r.GameGenre ?? string.Empty,
                    GameEngine = r.GameEngine ?? string.Empty,
                    GameDescription = r.GameDescription ?? string.Empty,
                    UpdateDate = r.UpdateDate ?? string.Empty,
                });
            }
            BuildIndex();
        }

        private void OnEnable() => BuildIndex();

        private void BuildIndex()
        {
            _index = new Dictionary<int,Entry>();
            foreach(var e in _entries)
            {
                if(!_index.ContainsKey(e.ProductionID))
                {
                    _index[e.ProductionID] = e;
                }
            }
        }

        public bool TryGet(int productionId,out Entry e)
        {
            if(_index == null)
            {
                BuildIndex();
            }

            return _index.TryGetValue(productionId,out e);
        }
    }
}
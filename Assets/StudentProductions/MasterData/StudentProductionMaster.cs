using UnityEngine;
using System;
using System.Collections.Generic;

namespace Launcher
{
    /// <summary>
    /// 学生制作物マスターデータ（単体）
    /// </summary>
    [CreateAssetMenu(menuName = "Master/Student Productions (Single)",fileName = "StudentProductionsMaster")]
    public class StudentProductionsMaster : ScriptableObject
    {
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

        [SerializeField] private List<Entry> entries = new();

        private Dictionary<int,Entry> index;
        public IReadOnlyList<Entry> Entries => entries;

        public void SetAll(StudentProductionRow[] rows)
        {
            entries.Clear();
            if(rows == null)
            {
                return;
            }

            foreach(var r in rows)
            {
                entries.Add(new Entry
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
            index = new Dictionary<int,Entry>();
            foreach(var e in entries)
            {
                if(!index.ContainsKey(e.ProductionID))
                {
                    index[e.ProductionID] = e;
                }
            }
        }

        public bool TryGet(int productionId,out Entry e)
        {
            if(index == null)
            {
                BuildIndex();
            }

            return index.TryGetValue(productionId,out e);
        }
    }
}
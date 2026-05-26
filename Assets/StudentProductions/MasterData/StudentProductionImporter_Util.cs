using System;
using System.Collections.Generic;
using System.IO;

namespace Launcher
{
    /// <summary>
    /// 学生制作物データ行
    /// </summary>
    public static class StudentProductionImporter_Util
    {
        public static StudentProductionRow[] ReadCsv(string csvPath)
        {
            var list = new List<StudentProductionRow>();
            using var sr = new StreamReader(csvPath);
            string line = sr.ReadLine();
            if(line == null) throw new Exception("CSV empty");
            var headers = SplitCsv(line);

            int Idx(string name) => Array.FindIndex(headers, h => string.Equals(h.Trim(), name, StringComparison.OrdinalIgnoreCase));

            int idxId              = Idx("ProductionID");
            int idxGameName        = Idx("GameName");
            int idxStudentName     = Idx("StudentName");
            int idxGrade           = Idx("Grade");
            int idxGraduationYear  = Idx("GraduationYear");
            int idxEventType       = Idx("EventType");
            int idxTeamOrSolo      = Idx("TeamOrSolo");
            int idxGameGenre       = Idx("GameGenre");
            int idxNumberOfPlayers = Idx("NumberOfPlayers");
            int idxGameEngine      = Idx("GameEngine");
            int idxGameDescription = Idx("GameDescription");
            int idxVideoFileId     = Idx("VideoFileId");
            int idxExeFileId       = Idx("ExeFileId");

            if(idxId < 0 || idxGameName < 0) throw new Exception("CSV headers must include ProductionID, GameName");

            while((line = sr.ReadLine()) != null)
            {
                if(string.IsNullOrWhiteSpace(line)) continue;
                var cells = SplitCsv(line);
                var row = new StudentProductionRow
                {
                    ProductionID    = int.TryParse(GetCell(cells, idxId), out var id) ? id : 0,
                    GameName        = GetCell(cells, idxGameName),
                    StudentName     = GetCell(cells, idxStudentName),
                    Grade           = int.TryParse(GetCell(cells, idxGrade), out var grade) ? grade : 0,
                    GraduationYear  = int.TryParse(GetCell(cells, idxGraduationYear), out var year) ? year : 0,
                    EventType       = GetCell(cells, idxEventType),
                    TeamOrSolo      = GetCell(cells, idxTeamOrSolo),
                    GameGenre       = GetCell(cells, idxGameGenre),
                    NumberOfPlayers = GetCell(cells, idxNumberOfPlayers),
                    GameEngine      = GetCell(cells, idxGameEngine),
                    GameDescription = GetCell(cells, idxGameDescription),
                    VideoFileId     = GetCell(cells, idxVideoFileId),
                    ExeFileId       = GetCell(cells, idxExeFileId),
                };
                list.Add(row);
            }
            return list.ToArray();
        }

        private static string GetCell(string[] cells,int idx) => (idx >= 0 && idx < cells.Length) ? cells[idx] : string.Empty;

        private static string[] SplitCsv(string line)
        {
            var result = new List<string>();
            bool inQuote = false; var cur = new System.Text.StringBuilder();
            for(int i = 0;i < line.Length;i++)
            {
                char c = line[i];
                if(c == '"')
                {
                    if(inQuote && i + 1 < line.Length && line[i + 1] == '"') { cur.Append('"'); i++; }
                    else inQuote = !inQuote;
                }
                else if(c == ',' && !inQuote)
                {
                    result.Add(cur.ToString()); cur.Clear();
                }
                else cur.Append(c);
            }
            result.Add(cur.ToString());
            return result.ToArray();
        }
    }
}
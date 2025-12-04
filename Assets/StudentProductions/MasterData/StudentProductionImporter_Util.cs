using System;
using System.Collections.Generic;
using System.IO;

namespace Lancher
{
    /// <summary>
    /// 
    /// </summary>
    public static class StudentProductionImporter_Util
    {
        public static StudentProductionRow[] ReadCsv(string csvPath)
        {
            var list = new List<StudentProductionRow>();
            using var sr = new StreamReader(csvPath);
            string line = sr.ReadLine();
            if (line == null) throw new Exception("CSV empty");
            var headers = SplitCsv(line);
            int idxId = Array.FindIndex(headers, h => string.Equals(h.Trim(), "ProductionID", StringComparison.OrdinalIgnoreCase));
            int idxName = Array.FindIndex(headers, h => string.Equals(h.Trim(), "GameName", StringComparison.OrdinalIgnoreCase));
            if (idxId < 0 || idxName < 0) throw new Exception("CSV headers must include ProductionID, GameName");

            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var cells = SplitCsv(line);
                var row = new StudentProductionRow
                {
                    ProductionID = int.TryParse(GetCell(cells, idxId), out var id) ? id : 0,
                    GameName = GetCell(cells, idxName),
                };
                list.Add(row);
            }
            return list.ToArray();
        }

        private static string GetCell(string[] cells, int idx) => (idx >= 0 && idx < cells.Length) ? cells[idx] : string.Empty;

        private static string[] SplitCsv(string line)
        {
            var result = new List<string>();
            bool inQuote = false; var cur = new System.Text.StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuote && i + 1 < line.Length && line[i + 1] == '"') { cur.Append('"'); i++; }
                    else inQuote = !inQuote;
                }
                else if (c == ',' && !inQuote)
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
namespace Launcher
{
    /// <summary>
    /// 作成作品(行)
    /// </summary>
    [System.Serializable]
    public class StudentProductionRow
    {
        public int ProductionID;
        public string GameName;
        public string StudentName;
        public int Grade;
        public int GraduationYear;
        public string EventType;
        public string TeamOrSolo;
        public string GameGenre;
        public string NumberOfPlayers;
        public string GameEngine;
        public string GameDescription;
        public string VideoFileId;
        public string ExeFileId;
    }
}
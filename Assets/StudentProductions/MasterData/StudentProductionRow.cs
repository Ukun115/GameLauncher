namespace Launcher
{
    /// <summary>
    /// 作成作品(行)
    /// </summary>
    [System.Serializable]
    public class StudentProductionRow
    {
        /// <summary>
        /// 作品ID
        /// </summary>
        public int ProductionID;

        /// <summary>
        /// ゲーム名
        /// </summary>
        public string GameName;

        /// <summary>
        /// 学生名
        /// </summary>
        public string StudentName;

        /// <summary>
        /// GoogleDrive上のゲームZipのFileID
        /// </summary>
        public string ExeFileId;

        /// <summary>
        /// GoogleDrive上の紹介動画のFileID
        /// </summary>
        public string VideoFileId;
    }
}
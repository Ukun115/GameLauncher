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
        /// 卒業年
        /// </summary>
        public int GraduationYear;

        /// <summary>
        /// イベント種別
        /// </summary>
        public string EventType;

        /// <summary>
        /// チーム/個人
        /// </summary>
        public string TeamOrSolo;

        /// <summary>
        /// ゲームジャンル
        /// </summary>
        public string GameGenre;

        /// <summary>
        /// ゲームエンジン
        /// </summary>
        public string GameEngine;

        /// <summary>
        /// 説明文
        /// </summary>
        public string GameDescription;

        /// <summary>
        /// 更新日
        /// </summary>
        public string UpdateDate;
    }
}
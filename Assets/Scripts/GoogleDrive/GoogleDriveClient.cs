namespace Launcher
{
    /// <summary>
    /// Google Drive ダウンロードURL生成
    /// confirm=t を付けることでウイルスチェック確認ページをスキップできる
    /// </summary>
    public static class GoogleDriveClient
    {
        private const string DownloadBase = "https://drive.usercontent.google.com/download";

        public static string GetDirectDownloadUrl(string fileId)
            => $"{DownloadBase}?id={fileId}&export=download&confirm=t";
    }
}

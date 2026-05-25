using UnityEngine;

namespace Launcher
{
    [CreateAssetMenu(menuName = "Launcher/Config", fileName = "LauncherConfig")]
    public class LauncherConfig : ScriptableObject
    {
        [Header("GoogleDrive: マスターデータJSONのFileID")]
        public string MasterDataFileId;
    }
}

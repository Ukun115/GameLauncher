namespace Launcher
{
    /// <summary>
    /// サウンド定数
    /// </summary>
    public static class SoundClipNameConst
    {
        // TODO : iseki
        // 現状だとサウンドファイル名に変更/追加があった場合、都度定数のリネーム/追加を行う必要がある。
        // アセットメニューから自動生成する仕組みを作る。
        // (ScriptableObject等でサウンド管理クラスを作成し、そこから定数クラスを自動生成する等)

        /// <summary>
        /// サンプルBGM
        /// </summary>
        public static readonly string SampleBGMClipName = "BGM_Sample";

        /// <summary>
        /// メカニカルクリック音
        /// </summary>
        public static readonly string MechanicalClickSEClipName = "SE_ClickMechanical";

        /// <summary>
        /// パンチクリック音
        /// </summary>
        public static readonly string PunchClickSEClipName = "SE_ClickPunch";

        /// <summary>
        /// シューックリック音
        /// </summary>
        public static readonly string WhooshClickSEClipName = "SE_ClickWhoosh";
    }
}
namespace Lancher
{
    /// <summary>
    /// PlayerPrefsデータ
    /// </summary>
    public static class PlayerPrefsData
    {
        /// <summary>
        /// シャドウタイプ
        /// </summary>
        public enum ShadowType
        {
            Off,    // オフ
            Low,    // 低
            High    // 高
        }

        /// <summary>
        /// テクスチャタイプ
        /// </summary>
        public enum TextureType
        {
            Low,    // 低
            Med,    // 中
            High    // 高
        }

        /// <summary>
        /// BGM音量キー
        /// </summary>
        public static readonly string BGMVolumeKey = "BGMVolume";

        /// <summary>
        /// SE音量キー
        /// </summary>
        public static readonly string SEVolumeKey = "SEVolume";

        /// <summary>
        /// 難易度キー
        /// </summary>
        public static readonly string DifficultyKey = "Difficulty";

        /// <summary>
        /// X軸感度キー
        /// </summary>
        public static readonly string XSensitivityKey = "XSensitivity";

        /// <summary>
        /// Y軸感度キー
        /// </summary>
        public static readonly string YSensitivityKey = "YSensitivity";

        /// <summary>
        /// マウススムージングキー
        /// </summary>
        public static readonly string MouseSmoothingKey = "MouseSmoothing";

        /// <summary>
        /// HUD表示キー
        /// </summary>
        public static readonly string ShowHUDKey = "ShowHUD";

        /// <summary>
        /// ツールチップキー
        /// </summary>
        public static readonly string ToolTipsKey = "ToolTips";

        /// <summary>
        /// シャドウ設定キー
        /// </summary>
        public static readonly string ShadowsKey = "Shadows";

        /// <summary>
        /// 反転キー
        /// </summary>
        public static readonly string InvertedKey = "Inverted";

        /// <summary>
        /// モーションブラーキー
        /// </summary>
        public static readonly string MotionBlurKey = "MotionBlur";

        /// <summary>
        /// アンビエントオクルージョンキー
        /// </summary>
        public static readonly string AmbientOcclusionKey = "AmbientOcclusion";

        /// <summary>
        /// テクスチャキー
        /// </summary>
        public static readonly string TexturesKey = "Textures";

        /// <summary>
        /// カメラエフェクトキー
        /// </summary>
        public static readonly string CameraEffectsKey = "CameraEffects";
    }
}
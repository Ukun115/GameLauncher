using UnityEngine;

namespace Lancher
{
    /// <summary>
    /// サウンドマネージャー
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        [Header("BGMのオーディオソース"), SerializeField]
        private AudioSource _bgmAudioSource = default;

        [Header("SEのオーディオソース"), SerializeField]
        private AudioSource _seAudioSource = default;

        /// <summary>
        /// Awake
        /// </summary>
        public void Awake()
        {
            // 音量初期化
            _bgmAudioSource.volume = PlayerPrefs.GetFloat(PlayerPrefsData.BGMVolumeKey);
            _seAudioSource.volume = PlayerPrefs.GetFloat(PlayerPrefsData.SEVolumeKey);
        }

        /// <summary>
        /// BGM再生
        /// bgmName : SoundConstのBGM名を指定
        /// </summary>
        public void PlayBGM(string bgmName, bool isLoop = true)
        {
            _bgmAudioSource.clip.name = bgmName;
            _bgmAudioSource.loop = isLoop;
            _bgmAudioSource.Play();
        }

        /// <summary>
        /// SE再生
        /// seName : SoundConstのSE名を指定
        /// </summary>
        public void PlaySE(string seName)
        {
            _seAudioSource.clip.name = seName;
            _seAudioSource.PlayOneShot(_seAudioSource.clip);
        }

        /// <summary>
        /// SE音量更新
        /// </summary>
        public void UpdateSEVolume()
        {
            _seAudioSource.volume = PlayerPrefs.GetFloat(PlayerPrefsData.SEVolumeKey);
        }

        /// <summary>
        /// BGM音量更新
        /// </summary>
        public void UpdateBGMVolume()
        {
            _bgmAudioSource.volume = PlayerPrefs.GetFloat(PlayerPrefsData.BGMVolumeKey);
        }
    }
}
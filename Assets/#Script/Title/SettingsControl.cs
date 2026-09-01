using Sound;
using Title.PlayerData;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Title
{
    /// <summary>
    /// 設定UI
    /// </summary>
    public class SettingsControl : MonoBehaviour        
    {
        [SerializeField] private AudioMixer _audioMixer;

        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _bgmVolume;
        [SerializeField] private Slider _seVolume;
        [SerializeField] private SESoundType _testSE;

        private const string MasterKey = "Master";
        private const string BgmKey = "BGM";
        private const string SEKey = "SE";

        private void Start()
        {
            // 音量変更イベント
            _masterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
            _bgmVolume.onValueChanged.AddListener(OnBGMVolumeChanged);
            _seVolume.onValueChanged.AddListener(OnSEVolumeChanged);

            // 保存データをU反映
            _masterVolume.value = PlayerDataLoader.Settings.MasterVolume;
            OnMasterVolumeChanged(_masterVolume.value);
            _bgmVolume.value = PlayerDataLoader.Settings.BgmVolume;
            OnBGMVolumeChanged(_bgmVolume.value);
            _seVolume.value = PlayerDataLoader.Settings.SEVolume;
            OnSEVolumeChanged(_seVolume.value);
        }

        private void OnMasterVolumeChanged(float value)
        {
            SetMixerVolume(MasterKey, value);
            PlayerDataLoader.Settings.SetMasterVolume(value);
        }

        private void OnBGMVolumeChanged(float value)
        {
            SetMixerVolume(BgmKey, value);
            PlayerDataLoader.Settings.SetBGMVolume(value);
        }

        private void OnSEVolumeChanged(float value)
        {
            SetMixerVolume(SEKey, value);
            SoundManager.SE.PlaySE(_testSE);
            PlayerDataLoader.Settings.SetSEVolume(value);
        }

        /// <summary>
        /// Sliderの値(0～1)をAudioMixerのdBに変換して設定
        /// </summary>
        private void SetMixerVolume(string key, float volume)
        {
            // log(0)を防ぐ
            volume = Mathf.Max(volume, 0.0001f);

            float db = Mathf.Log10(volume) * 20f;

            _audioMixer.SetFloat(key, db);
        }

        /// <summary>
        /// 設定画面を閉じる
        /// </summary>
        public void OnClose()
        {
            PlayerDataLoader.Settings.SetMasterVolume(_masterVolume.value);
            PlayerDataLoader.Settings.SetBGMVolume(_bgmVolume.value);
            PlayerDataLoader.Settings.SetSEVolume(_seVolume.value);
        }

        private void OnDestroy()
        {
            _masterVolume.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            _bgmVolume.onValueChanged.RemoveListener(OnBGMVolumeChanged);
            _seVolume.onValueChanged.RemoveListener(OnSEVolumeChanged);
        }
    }
}
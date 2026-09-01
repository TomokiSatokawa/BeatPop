using R3;
using UnityEngine;

namespace Title.PlayerData
{
    /// <summary>
    /// 設定データ
    /// </summary>
    [System.Serializable]
    public class SettingsData : IReadOnlySettingsData
    {
        [SerializeField] private float _masterVolume = 1f;
        [SerializeField] private float _bgmVolume = 1f;
        [SerializeField] private float _seVolume = 1f;

        public float MasterVolume => _masterVolume;

        public float BgmVolume => _bgmVolume;

        public float SEVolume => _seVolume;

        private Subject<Unit> _onUpdateData = new();
        /// <summary>データ更新時</summary>
        public Observable<Unit> OnUpdateData => _onUpdateData;


        public void SetBGMVolume(float volume)
        {
            _bgmVolume = volume;
            _onUpdateData.OnNext(Unit.Default);
        }

        public void SetMasterVolume(float volume)
        {
            _masterVolume = volume;
            _onUpdateData.OnNext(Unit.Default);
        }

        public void SetSEVolume(float volume)
        {
            _seVolume = volume;
            _onUpdateData.OnNext(Unit.Default);
        }
    }

    public interface IReadOnlySettingsData
    {
        public float MasterVolume { get; }
        public float BgmVolume { get; }
        public float SEVolume { get; }
        public void SetMasterVolume(float volume);
        public void SetBGMVolume(float volume);
        public void SetSEVolume(float volume);
    }
}
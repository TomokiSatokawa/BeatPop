using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sound
{
    /// <summary>
    /// SE、BGMデータ
    /// </summary>
    [CreateAssetMenu(fileName = "SoundDataBase", menuName = "Scriptable Objects/SoundDataBase")]
    public class SoundDataBase : ScriptableObject
    {
        [SerializeField] private SESoundData[] _seData;
        public IReadOnlyList<SESoundData> SEDatas => _seData;
        [SerializeField] private BGMSoundData[] _bgmData;
        public IReadOnlyList<BGMSoundData> BGMDatas => _bgmData;

        private Dictionary<SESoundType, SESoundData> _seDataMap;
        private Dictionary<SESoundType, BGMSoundData> _bgmDataMap;

        private void Initialize()
        {
            _seDataMap = _seData.ToDictionary(x => x.Type, x => x);
            _bgmDataMap = _bgmData.ToDictionary(x => x.Type, x => x);
        }

        public SESoundData GetSESound(SESoundType type)
        {
            if (_seDataMap == null)
                Initialize();

            if (_seDataMap.TryGetValue(type, out var data))
                return data;

            Debug.LogError($"[Sound] SE Sound is not found. Type:{type}");
            return null;
        }

        public BGMSoundData GetBGMSound(SESoundType type)
        {
            if (_bgmDataMap == null)
                Initialize();

            if (_bgmDataMap.TryGetValue(type, out var data))
                return data;

            Debug.LogError($"[Sound] BGM Sound is not found. Type:{type}");
            return null;
        }
    }

    [System.Serializable]
    public class SESoundData
    {
        public SESoundType Type;
        public AudioClip Clip;
        public float Volume = 1f;
    }
    [System.Serializable]
    public class BGMSoundData
    {
        public SESoundType Type;
        public AudioClip Clip;
        public float Volume = 1f;
    }
    public enum SESoundType
    {
        None,
        Tap1, Tap2, Tap3, Tap4, Tap5, Tap6, Tap7, Tap8,
        Hold1, Hold2, Hold3, Hold4,
        EmptyHit
    }
}

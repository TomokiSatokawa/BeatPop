using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [CreateAssetMenu(fileName = "SoundDataBase", menuName = "Scriptable Objects/SoundDataBase")]
    public class SoundDataBase : ScriptableObject
    {
        [SerializeField] private SESoundData[] _seData;
        public IReadOnlyList<SESoundData> SEDatas => _seData;
        [SerializeField] private BGMSoundData[] _bgmData;
        public IReadOnlyList<BGMSoundData> BGMDatas => _bgmData;

        public SESoundData GetSESound(SESoundType type)
        {
            foreach(var data in _seData)
            {
                if (data.Type == type) return data;
            }
            Debug.LogError(type + " type SE Sound is not found");
            return null;
        }
        public BGMSoundData GetBGMSound(SESoundType type)
        {
            foreach (var data in _bgmData)
            {
                if (data.Type == type) return data;
            }
            Debug.LogError(type + " type BGM Sound is not found");
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
        public float BPM = 60f;
    }
    public enum SESoundType
    {
       Npme,NormalTap,FlickTap,HoldStart,HoldEnd,Hit
    }
}

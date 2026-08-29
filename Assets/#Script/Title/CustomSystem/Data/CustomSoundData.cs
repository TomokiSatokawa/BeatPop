using System.Collections.Generic;
using UnityEngine;

namespace Title.Custom
{
    [CreateAssetMenu(fileName = "CustomColorData", menuName = "Scriptable Objects/Custom/CustomSoundData")]
    public class CustomSoundData : ScriptableObject
    {
        [SerializeField] private List<PresetData> _presetDatas;
        [SerializeField] private List<SEData> _holdSE;
        [SerializeField] private List<SEData> _tapSE;
        [SerializeField] private bool _uesDefaultPreset = true;
        [SerializeField] private SerializableDictionary<CustomSoundType, int> _defaultValue = new();
        public IReadOnlyList<PresetData> PresetDatas => _presetDatas;
        public IReadOnlyList<SEData> TapSE => _tapSE;
        public IReadOnlyList<SEData> HoldSE => _holdSE;

        public CustomSoundPattern GetDefault()
        {
            var result = new CustomSoundPattern();

            result.UsePreset = _uesDefaultPreset;

            foreach (var kv in _defaultValue.Items)
            {
                result.SetData(kv.Key, kv.Value);
            }

            return result;
        }
    }

    [System.Serializable]
    public class SEData
    {
        [SerializeField] private string _name;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _volume;
        public string Name => _name;
        public AudioClip Clip => _clip;
        public float Volume => _volume;
    }

    [System.Serializable]
    public class PresetData
    {
        [SerializeField] private string _name;
        [SerializeField] private SerializableDictionary<CustomSoundType, int> _value = new();
        public string Name => _name;
        public IReadOnlyDictionary<CustomSoundType, int> Value => _value.ToDictionary();
    }
}
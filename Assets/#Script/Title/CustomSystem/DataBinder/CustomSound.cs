using System;
using System.Collections.Generic;
using System.Linq;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Title.Custom
{
    /// <summary>
    /// ノーツSEのカスタム
    /// </summary>
    public class CustomSound : CustomDataBinder<CustomSoundPattern>
    {
        [SerializeField] private CustomSoundData _soundData;
        [Header("UI")]
        [SerializeField] private Toggle _presetToggle;
        [SerializeField] private SerializableDictionary<CustomSoundType, TMP_Dropdown> _dropDowns = new();

        private bool _enablePreview = true;
        private void Start()
        {
            var presetOption = CreateOption(_soundData.PresetDatas.Select(x => x.Name));
            var tapOption = CreateOption(_soundData.TapSE.Select(x => x.Name));
            var holdOption = CreateOption(_soundData.HoldSE.Select(x => x.Name));

            foreach (var kv in _dropDowns.Items)
            {
                switch (kv.Key)
                {
                    case CustomSoundType.Preset:
                        kv.Value.options = presetOption;
                        break;
                    case CustomSoundType.Normal:
                    case CustomSoundType.Flick:
                    case CustomSoundType.HoldStart:
                    case CustomSoundType.HoldEnd:
                    case CustomSoundType.TickNode:
                    case CustomSoundType.HighScore:
                        InitializeSEDropdown(kv.Value, tapOption, _soundData.TapSE);
                        break;
                    case CustomSoundType.HoldFill:
                        InitializeSEDropdown(kv.Value, holdOption, _soundData.HoldSE);
                        break;
                }
            }

            _presetToggle.onValueChanged.AddListener(_ => UpdateInteractable());

            OnDefault();
        }

        private void UpdateInteractable()
        {
            foreach (var kv in _dropDowns.Items)
            {
                if(kv.Key == CustomSoundType.Preset)
                {
                    kv.Value.interactable = _presetToggle.isOn;
                }
                else
                {
                    kv.Value.interactable = !_presetToggle.isOn;
                }
            }
            }

        private List<TMP_Dropdown.OptionData> CreateOption(IEnumerable<string> names)
        {
            var tapOption = new List<TMP_Dropdown.OptionData>();
            foreach (var item in names)
            {
                tapOption.Add(new(item));
            }

            return tapOption;
        }

        private void InitializeSEDropdown(TMP_Dropdown dropdown, List<TMP_Dropdown.OptionData> options
            , IReadOnlyList<SEData> soundData)
        {
            dropdown.options = options;
            dropdown.onValueChanged.AddListener(x => OnChangeValue(soundData[x].Clip));
        }

        public void OnChangeValue(AudioClip seSound)
        {
            if (!_enablePreview) return;
            SoundManager.SE.PlaySE(seSound);
        }

        public override void SetCustom(CustomSoundPattern customSound)
        {
            _enablePreview = false;
            _presetToggle.isOn = customSound.UsePreset;
            foreach (var kv in _dropDowns.Items)
            {
                kv.Value.value = customSound.GetData(kv.Key);
            }
            _enablePreview = true;
            UpdateInteractable();
        }

        public override CustomSoundPattern GetCustom()
        {
            var result = _soundData.GetDefault();
            result.UsePreset = _presetToggle.isOn;
            foreach (var kv in _dropDowns.Items)
            {
                result.SetData(kv.Key, kv.Value.value);
            }

            return result;
        }

        public override void OnDefault()
        {
            SetCustom(_soundData.GetDefault());
        }

        public static PoolPrefabType CustomTypeToPoolType(CustomSoundType custom)
        {
            return custom switch
            {
                CustomSoundType.Normal => PoolPrefabType.NormalNote,
                CustomSoundType.HighScore => PoolPrefabType.HighScoreNote,
                CustomSoundType.Flick => PoolPrefabType.FlickNote,
                CustomSoundType.HoldStart => PoolPrefabType.HoldNoteStart,
                CustomSoundType.HoldFill => PoolPrefabType.HoldNoteFill,
                CustomSoundType.HoldEnd => PoolPrefabType.HoldNoteEnd,
                CustomSoundType.TickNode => PoolPrefabType.TickNode,
                _ => throw new ArgumentOutOfRangeException(nameof(custom), custom, null)
            };
        }
    }
    [System.Serializable]
    public struct CustomSoundPattern
    {
        public bool UsePreset;
        [SerializeField] private int _preset;
        [SerializeField] private int _normal;
        [SerializeField] private int _highScore;
        [SerializeField] private int _flick;
        [SerializeField] private int _holdStart;
        [SerializeField] private int _holdFill;
        [SerializeField] private int _holdEnd;
        [SerializeField] private int _tickNode;

        public void SetData(CustomSoundType type, int data)
        {
            switch (type)
            {
                case CustomSoundType.Preset:
                    _preset = data;
                    return;
                case CustomSoundType.Normal:
                    _normal = data;
                    break;
                case CustomSoundType.Flick:
                    _flick = data;
                    break;
                case CustomSoundType.HoldStart:
                    _holdStart = data;
                    break;
                case CustomSoundType.HoldFill:
                    _holdFill = data;
                    break;
                case CustomSoundType.HoldEnd:
                    _holdEnd = data;
                    break;
                case CustomSoundType.TickNode:
                    _tickNode = data;
                    break;
                case CustomSoundType.HighScore:
                    _highScore = data;
                    break;
                default:
                    Debug.LogError($"[CustomColorData] Invalid CustomColorType: {type}");
                    break;
            }
        }
        public int GetData(CustomSoundType type)
        {
            return type switch
            {
                CustomSoundType.Preset => _preset,
                CustomSoundType.Normal => _normal,
                CustomSoundType.Flick => _flick,
                CustomSoundType.HoldStart => _holdStart,
                CustomSoundType.HoldFill => _holdFill,
                CustomSoundType.HoldEnd => _holdEnd,
                CustomSoundType.TickNode => _tickNode,
                CustomSoundType.HighScore => _highScore,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }

    public enum CustomSoundType
    {
        Preset, Normal, Flick, HoldStart, HoldFill, HoldEnd, TickNode, HighScore
    }
}
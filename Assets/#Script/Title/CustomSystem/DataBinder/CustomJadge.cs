using System;
using UnityEngine;
using UnityEngine.UI;

namespace Title.Custom
{
    /// <summary>
    /// ”»’è‚ÌƒJƒXƒ^ƒ€
    /// </summary>
    public class CustomJudge : CustomDataBinder<CustomJudgePattern>
    {
        [SerializeField] private SerializableDictionary<CustomJudgeType, Slider> _sliders = new();
        public override CustomJudgePattern GetCustom()
        {
            var result = new CustomJudgePattern();
            foreach (var slider in _sliders.Items)
            {
                result.SetLevel(slider.Key, slider.Value.value / slider.Value.maxValue);
            }

            return result;
        }

        public override void OnDefault()
        {
            foreach (var slider in _sliders.Items)
            {
                slider.Value.value = 0;
            }
        }

        public override void SetCustom(CustomJudgePattern data)
        {
            foreach (var slider in _sliders.Items)
            {
                slider.Value.value = data.GetLevel(slider.Key) * slider.Value.maxValue;
            }
        }

        public static CustomJudgeType PoolPrefabToCutomJudge(PoolPrefabType prefabType)
        {
            return prefabType switch
            {
                PoolPrefabType.NormalNote => CustomJudgeType.Normal,
                PoolPrefabType.FlickNote => CustomJudgeType.Flick,
                PoolPrefabType.HoldNoteStart => CustomJudgeType.LongStart,
                PoolPrefabType.HoldNoteEnd => CustomJudgeType.LongEnd,
                PoolPrefabType.HoldFlickEnd => CustomJudgeType.LongFlick,
                PoolPrefabType.HighScoreNote => CustomJudgeType.HighScore,
                _ => CustomJudgeType.None
            };
        }
    }

    [Serializable]
    public struct CustomJudgePattern
    {
        [SerializeField] private float _normalJudgeLevel;
        [SerializeField] private float _flickJudgeLevel;
        [SerializeField] private float _longStartJudgeLevel;
        [SerializeField] private float _longEndJudgeLevel;
        [SerializeField] private float _longFlickJudgeLevel;
        [SerializeField] private float _highScoreJudgeLevel;

        public float GetLevel(CustomJudgeType type)
        {
            return type switch
            {
                CustomJudgeType.None => 0,
                CustomJudgeType.Normal => _normalJudgeLevel,
                CustomJudgeType.Flick => _flickJudgeLevel,
                CustomJudgeType.LongStart => _longStartJudgeLevel,
                CustomJudgeType.LongEnd => _longEndJudgeLevel,
                CustomJudgeType.LongFlick => _longFlickJudgeLevel,
                CustomJudgeType.HighScore => _highScoreJudgeLevel,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void SetLevel(CustomJudgeType type, float value)
        {
            switch (type)
            {
                case CustomJudgeType.Normal:
                    _normalJudgeLevel = value;
                    break;

                case CustomJudgeType.Flick:
                    _flickJudgeLevel = value;
                    break;

                case CustomJudgeType.LongStart:
                    _longStartJudgeLevel = value;
                    break;

                case CustomJudgeType.LongEnd:
                    _longEndJudgeLevel = value;
                    break;

                case CustomJudgeType.LongFlick:
                    _longFlickJudgeLevel = value;
                    break;

                case CustomJudgeType.HighScore:
                    _highScoreJudgeLevel = value;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
    public enum CustomJudgeType
    {
      None,Normal, Flick, LongStart, LongEnd, LongFlick, HighScore
    }

}
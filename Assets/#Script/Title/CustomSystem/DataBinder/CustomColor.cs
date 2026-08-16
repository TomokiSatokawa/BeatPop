using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

namespace Title.Custom
{
    /// <summary>
    /// ノーツSEのカスタム
    /// </summary>
    public class CustomColor : CustomDataBinder<CustomColorPattern>
    {
        [SerializeField] private CustomColorData _customColorData;
        [SerializeField] private Sprite _optionImage;
        [SerializeField] private SerializableDictionary<CustomColorType, TMP_Dropdown> _dropdowns;

        private void Start()
        {
            List<TMP_Dropdown.OptionData> options = new();

            //Option作成
            foreach (var colorData in _customColorData.ColorPallet)
                options.Add(new(colorData.Name, _optionImage, colorData.Color));

            //Option代入
            foreach (var kv in _dropdowns.Items) 
                kv.Value.options = options;

        }

        public override CustomColorPattern GetCustom()
        {
            var result = _customColorData.GetDefault();

            //UIからデータを取得し保存
            foreach (var kv in _dropdowns.Items)
            {
                result.SetData(kv.Key, kv.Value.value);
            }

            return result;
        }

        public override void OnDefault()
        {
            SetCustom(_customColorData.GetDefault());
        }

        public override void SetCustom(CustomColorPattern data)
        {
            //UIにデータを入れる
            foreach (var kv in _dropdowns.Items)
            {
                kv.Value.value = data.GetColorIndex(kv.Key);
            }
        }
    }
    [System.Serializable]
    public struct CustomColorPattern
    {
        [SerializeField] private int NormalColor;
        [SerializeField] private int FlickColor;
        [SerializeField] private int LongColor;
        [SerializeField] private int LongFlickColor;
        [SerializeField] private int HighScoreColor;
        [SerializeField] private int TickColor;

        public void SetData(CustomColorType type, int colorIndex)
        {
            switch (type)
            {
                case CustomColorType.Normal:
                    NormalColor = colorIndex;
                    break;

                case CustomColorType.Flick:
                    FlickColor = colorIndex;
                    break;

                case CustomColorType.Long:
                    LongColor = colorIndex;
                    break;

                case CustomColorType.LongFlick:
                    LongFlickColor = colorIndex;
                    break;

                case CustomColorType.HighScore:
                    HighScoreColor = colorIndex;
                    break;

                case CustomColorType.Tick:
                    TickColor = colorIndex;
                    break;

                default:
                    Debug.LogError($"[CustomColorData] Invalid CustomColorType: {type}");
                    break;
            }
        }

        public int GetColorIndex(CustomColorType type)
        {
            return type switch
            {
                CustomColorType.Normal => NormalColor,
                CustomColorType.Flick => FlickColor,
                CustomColorType.Long => LongColor,
                CustomColorType.LongFlick => LongFlickColor,
                CustomColorType.HighScore => HighScoreColor,
                CustomColorType.Tick => TickColor,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }

    public enum CustomColorType
    {
        Normal, Flick, Long, LongFlick, HighScore,Tick
    }
}
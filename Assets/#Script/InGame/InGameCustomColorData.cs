using System;
using Common.PlaySystem;
using R3;
using Title.Custom;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// InGame内でColorのカスタム内容を簡単に取得する
    /// </summary>
    public class InGameCustomColorData : SingletonMonoBehaviour<InGameCustomColorData>
    {
        [SerializeField] private CustomColorData _customColorData;

        public Color GetNodeColor(PoolPrefabType type)
        {
            var customData = SongPlayContext.I?.PatternData?.ColorPattern ?? _customColorData.GetDefault();

            //小節線
            if(type == PoolPrefabType.Line)
                return Color.white;

            var colorType  = type switch
            {
                PoolPrefabType.NormalNote => CustomColorType.Normal,
                PoolPrefabType.FlickNote => CustomColorType.Flick,
                PoolPrefabType.HoldNoteStart=> CustomColorType.Long,
                PoolPrefabType.HoldNoteFill => CustomColorType.Long,
                PoolPrefabType.HoldNoteEnd=> CustomColorType.Long,
                PoolPrefabType.HoldFlickEnd=> CustomColorType.LongFlick,
                PoolPrefabType.HighScoreNote=> CustomColorType.HighScore,
                PoolPrefabType.TickNode=> CustomColorType.Tick,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

                var color =  _customColorData.GetColor(customData.GetColorIndex(colorType)).Color;

            if(type == PoolPrefabType.HoldNoteFill)
                return GetFillColor(color);

            return color;
        }
        private Color GetFillColor(Color color)
        {
            color.a = _customColorData.FillNodeAlpha;
            return color;
        }
    }
}
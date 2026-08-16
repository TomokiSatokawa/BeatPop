using System;
using Common.PlaySystem;
using R3;
using Title.Custom;
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

            var colorType  = type switch
            {
                PoolPrefabType.NormalNote => CustomColorType.Normal,
                PoolPrefabType.FlickNote => CustomColorType.Flick,
                PoolPrefabType.HoldNoteStart=> CustomColorType.Long,
                PoolPrefabType.HoldNoteEnd=> CustomColorType.Long,
                PoolPrefabType.HoldFlickEnd=> CustomColorType.LongFlick,
                PoolPrefabType.HighScoreNote=> CustomColorType.HighScore,
                PoolPrefabType.TickNode=> CustomColorType.Tick,
                PoolPrefabType.Line=> CustomColorType.Normal,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            return _customColorData.GetColor(customData.GetColorIndex(colorType)).Color;
        }
    }
}
using UnityEngine;

namespace Common.UI
{
    /// <summary>
    /// データをUIに表示できるようにフォーマルする
    /// </summary>
    public static class UIFormat
    {
        /// <summary>
        /// 時間（秒）をhh時間mm分ss秒の文字列にする
        /// </summary>
        /// <param name="seconds">時間（秒）</param>
        /// <returns>hh時間mm分ss秒の文字列</returns>
        public static string SecondToText(float seconds)
        {
            int hours = Mathf.FloorToInt(seconds / 3600);
            int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
            float remainSeconds = seconds % 60;

            string secondText = Mathf.Approximately(remainSeconds % 1f, 0f)
                ? $"{Mathf.FloorToInt(remainSeconds)}秒"
                : $"{remainSeconds:0.##}秒";

            if (hours > 0)
            {
                return $"{hours}時間{minutes:00}分{secondText}";
            }

            if (minutes > 0)
            {
                return $"{minutes}分{secondText}";
            }

            return secondText;
        }
    }
}
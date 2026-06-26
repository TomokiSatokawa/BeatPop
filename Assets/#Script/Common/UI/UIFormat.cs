using UnityEngine;

public static@class UIFormat
{
    public static string SecondToText(float seconds)
    {
        int hours = Mathf.FloorToInt(seconds / 3600);
        int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
        float remainSeconds = seconds % 60;

        string secondText = Mathf.Approximately(remainSeconds % 1f, 0f)
            ? $"{Mathf.FloorToInt(remainSeconds)}•b"
            : $"{remainSeconds:0.##}•b";

        if (hours > 0)
        {
            return $"{hours}ŽžŠÔ{minutes:00}•ª{secondText}";
        }

        if (minutes > 0)
        {
            return $"{minutes}•ª{secondText}";
        }

        return secondText;
    }
}

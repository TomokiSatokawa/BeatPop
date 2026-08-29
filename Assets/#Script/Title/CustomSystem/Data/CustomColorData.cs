using System.Collections.Generic;
using Title.Custom;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomColorData", menuName = "Scriptable Objects/Custom/CustomColorData")]
public class CustomColorData : ScriptableObject
{
    [SerializeField] private ColorData[] _colorPallet;
    [SerializeField] private SerializableDictionary<CustomColorType, int> _defaultValue;

    public IReadOnlyList<ColorData> ColorPallet => _colorPallet;

    [System.Serializable]
    public class ColorData
    {
        public Color Color;
        public string Name;
    }

    public ColorData GetColor(int index)
    {
        if (index < 0 || index >= _colorPallet.Length)
        {
            Debug.LogError($"[CustomColorData] 範囲外のデフォルト値です。 index:{index}");
            return default;
        }

        return _colorPallet[index];
    }

    public CustomColorPattern GetDefault()
    {
        var result = new CustomColorPattern();

        foreach (var value in _defaultValue.Items)
        {
            result.SetData(value.Key, value.Value);
        }

        return result;
    }
}

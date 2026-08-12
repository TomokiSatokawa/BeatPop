using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerIconDatabase", menuName = "Scriptable Objects/PlayerIconDatabase")]
public class PlayerIconDatabase : ScriptableObject
{
    [SerializeField] private IconData[] _iconData;
    [SerializeField] private int _defaultIconId;

    public IReadOnlyList<IconData> IconDatas => _iconData;

    public Sprite GetIcon(int id)
    {
        var icon = Array.Find(_iconData, c => c.IconId == id);
        if (icon == null)
        {
            Debug.LogError($"[PlayerIconDatabase] Icon id not found ID:{id}");
            return null;
        }

        return icon.Sprite;
    }

    public Sprite GetDefaultIcon()
    {
        return GetIcon(_defaultIconId);
    }

    [System.Serializable]
    public class IconData
    {
        [SerializeField] private int _iconId;
        [SerializeField] private Sprite _sprite;

        public int IconId => _iconId;
        public Sprite Sprite => _sprite;

    }
}

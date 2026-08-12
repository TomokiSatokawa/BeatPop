using System.Collections.Generic;
using Common.UI;
using R3.Triggers;
using Title.PlayerData;
using UnityEngine;

namespace Title.Profile
{
    public class IconSelectList : ScrollViewBase
    {
        [SerializeField] private PlayerIconDatabase _iconDatabase;
        [SerializeField] private RectTransform _select;
        [SerializeField] private IconUIControl _prefab;

        private Dictionary<int, IconUIControl> _clonedIcons = new();
        private void Start()
        {
            GenerateIconList();
        }

        public void GenerateIconList()
        {
            DeleteChildren();

            foreach (var iconData in _iconDatabase.IconDatas)
            {
                var icon = InstantiateContent(_prefab);

                icon.SetIcon(iconData.Sprite,iconData.IconId);
                icon.SetSelectAction(OnSelect);

                _clonedIcons.Add(iconData.IconId, icon);
            }
        }

        public void UpdateSelection()
        {
            var id = PlayerDataLoader.Info.IconImageId;

            _select.transform.position = _clonedIcons[id].transform.position;
        }

        private void OnSelect(int id)
        {
            PlayerDataLoader.Info.SetIconID(id);
            UpdateSelection();
        }
    }
}
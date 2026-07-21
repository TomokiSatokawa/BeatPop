using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Editor.UI
{
    /// <summary>
    /// パターン設定UIのドロップダウンUI
    /// </summary>
    public class SelectPrefabControl : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private TextMeshProUGUI _valueName;

        public void SetData(List<string> items, string name, int defaultIndex, Action<int> onChangeValue = null)
        {
            _valueName.text = name;

            _dropdown.options.Clear();
            _dropdown.AddOptions(items);
            _dropdown.value = defaultIndex;

            _dropdown.onValueChanged.RemoveAllListeners();
            _dropdown.onValueChanged.AddListener(x => onChangeValue?.Invoke(x));
        }
    }
}
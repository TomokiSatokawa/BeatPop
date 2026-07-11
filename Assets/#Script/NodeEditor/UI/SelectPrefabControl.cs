using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Editor.UI
{
    public class SelectPrefabControl : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private TextMeshProUGUI _valueName;

        public void SetData(List<string> candidate, string name, int value, Action<int> onChangeValue = null)
        {
            _valueName.text = name;

            _dropdown.options.Clear();
            _dropdown.AddOptions(candidate);
            _dropdown.value = value;

            _dropdown.onValueChanged.RemoveAllListeners();
            _dropdown.onValueChanged.AddListener(x => onChangeValue?.Invoke(x));
        }
    }
}
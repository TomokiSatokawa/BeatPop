using System;
using TMPro;
using UnityEngine;

namespace Editor.UI
{
    /// <summary>
    /// パターン設定のフィールドUI
    /// </summary>
    public class ValuePrefabControl : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private TextMeshProUGUI _valueName;

        public void SetData(bool isInteger,string name,string defaultValue,Action<string>  onChangeValue = null)
        {
            _valueName.text = name;
            _input.characterValidation = isInteger ? TMP_InputField.CharacterValidation.Integer: TMP_InputField.CharacterValidation.Decimal;
            _input.text = defaultValue;

            _input.onEndEdit.RemoveAllListeners();
            _input.onEndEdit.AddListener(x => onChangeValue?.Invoke(x));
        }
    }
}

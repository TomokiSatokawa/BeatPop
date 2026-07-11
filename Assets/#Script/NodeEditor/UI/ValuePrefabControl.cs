using System;
using TMPro;
using UnityEngine;

namespace Editor.UI
{
    public class ValuePrefabControl : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private TextMeshProUGUI _valueName;

        public void SetData(bool isInteger,string name,string value,Action<string>  onChangeValue = null)
        {
            _valueName.text = name;
            _input.characterValidation = isInteger ? TMP_InputField.CharacterValidation.Integer: TMP_InputField.CharacterValidation.Decimal;
            _input.text = value;

            _input.onEndEdit.RemoveAllListeners();
            _input.onEndEdit.AddListener(x => onChangeValue?.Invoke(x));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.UI;
using InGame.Stage;
using UnityEngine;
using UnityEngine.UI;
namespace Editor.UI
{

    public class PatternSettingsControl : ScrollViewBase
    {
        [SerializeField] private SerializableDictionary<string, Color> _colorPallet;
        [SerializeField] private Button _clauseButton;
        [SerializeField] private PanelControl _panelControl;
        [SerializeField] private ValuePrefabControl _valuePrefab;
        [SerializeField] private SelectPrefabControl _selectPrefab;

        private const string _truncateTarget = "LightPattern";
        private LightPatternBaseData _currentSettingData;
        private List<Type> _patternTypes = new();

        private void Start()
        {
            _clauseButton.onClick.RemoveAllListeners();
            _clauseButton.onClick.AddListener(() => _panelControl.OnHidden());

            var baseType = typeof(LightPatternBase<LightPatternBaseData>);

            foreach (Type type in Assembly.GetAssembly(typeof(LightPatternBaseData)).GetTypes())
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;

                if (!IsLightPatternType(type))
                    continue;

                _patternTypes.Add(type);
            }
        }
        private static bool IsLightPatternType(Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == typeof(LightPatternBase<>))
                {
                    var dataType = type.GetGenericArguments()[0];
                    return typeof(LightPatternBaseData).IsAssignableFrom(dataType);
                }

                type = type.BaseType;
            }

            return false;
        }
        public static Type GetParameterTypeFromPattern(Type patternType)
        {
            Type currentType = patternType;

            // 親クラスを遡って BasePattern<T> を探す
            while (currentType != null && currentType != typeof(object))
            {
                // ジェネリック型であり、かつそのジェネリックの定義元が BasePattern`1 であるか判定
                if (currentType.IsGenericType &&
                    currentType.GetGenericTypeDefinition() == typeof(LightPatternBase<>))
                {
                    // ジェネリック引数（Tの部分）の配列から、最初の型を返す
                    return currentType.GetGenericArguments()[0];
                }

                // さらに親クラスへ遡る
                currentType = currentType.BaseType;
            }

            return null; // 見つからなかった場合
        }
        public void ShowSettings(LightPatternBaseData data)
        {
            _panelControl.OnActive();
            if (data == _currentSettingData)
            {
                return;
            }
            DeleteChildren();
            _currentSettingData = data;

            Type type = data.GetType();

            FieldInfo[] allFields = type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic);


            foreach (FieldInfo field in allFields)
            {
                FieldInfo targetField = field;

                switch (field.FieldType.Name)
                {
                    case nameof(Int32):
                        InstantiateContent(_valuePrefab)
                            .SetData(true, targetField.Name, targetField.GetValue(data).ToString()
                            , x => targetField.SetValue(data, int.Parse(x)));
                        break;
                    case nameof(Single):
                        InstantiateContent(_valuePrefab)
                        .SetData(false, targetField.Name, targetField.GetValue(data).ToString()
                        , x => targetField.SetValue(data, float.Parse(x)));
                        break;
                    case nameof(String):
                        int selectType = _patternTypes.FindIndex(x => x.FullName == targetField.GetValue(data).ToString());

                        InstantiateContent(_selectPrefab)
                        .SetData(_patternTypes.Select(x => x.Name.Replace(_truncateTarget, "")).ToList(), targetField.Name, selectType
                        , x =>
                        {
                            targetField.SetValue(data, _patternTypes[x].FullName);
                            var newData = EditorLightData.I.ChangeType(data, GetParameterTypeFromPattern(_patternTypes[x]));
                            EditorLightGenerator.I.UpdateData(data, newData);   
                        });
                        break;
                    case nameof(ColorData):
                        int selectIndex = -1;
                        Color selectColor = ((ColorData)targetField.GetValue(data)).GetColor();

                        int i = 0;
                        foreach (var kv in _colorPallet.Items)
                        {
                            if (kv.Value == selectColor)
                            {
                                break;
                            }
                            i++;
                        }
                        InstantiateContent(_selectPrefab)
                      .SetData(_colorPallet.Items.Select(x => x.Key).ToList(), targetField.Name, i
                      , x => targetField.SetValue(data, new ColorData(_colorPallet.Items[x].Value)));
                        break;

                    default:
                        if (targetField.FieldType.IsEnum)
                        {
                            var enumNames = Enum.GetNames(targetField.FieldType).ToList();
                            var currentValue = targetField.GetValue(data);
                            int selectedIndex = Array.IndexOf(Enum.GetValues(targetField.FieldType), currentValue);

                            InstantiateContent(_selectPrefab)
                                .SetData(
                                    enumNames,
                                    targetField.Name,
                                    selectedIndex,
                                    x =>
                                    {
                                        var value = Enum.GetValues(targetField.FieldType).GetValue(x);
                                        targetField.SetValue(data, value);
                                    });
                        }
                        break;
                }
            }
        }
    }
}

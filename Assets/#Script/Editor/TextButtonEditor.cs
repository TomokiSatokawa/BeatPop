using UnityEditor;
using UnityEditor.UI;
using Common.UI;

[CustomEditor(typeof(TextButton))]
public class TextButtonEditor : ButtonEditor
{
    SerializedProperty _text;

    protected override void OnEnable()
    {
        base.OnEnable();

        _text = serializedObject.FindProperty("_text");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(_text);

        serializedObject.ApplyModifiedProperties();
    }
}
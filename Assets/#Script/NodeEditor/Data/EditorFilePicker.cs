using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// ファイル選択
    /// </summary>
    public static class EditorFilePicker
    {
        private static string _lastFileName = "SaveData";
        public static void OnImport(Action<string> importCallback = null)
        {
#if UNITY_EDITOR
            string path = EditorUtility.OpenFilePanel(
               "Open Save Data",
               "",
               "json");

            if (string.IsNullOrEmpty(path))
                return;

            string fileText = File.ReadAllText(path);

            _lastFileName = Path.GetFileNameWithoutExtension(path);
            importCallback?.Invoke(fileText);
#endif
        }

        public static void OnExport(string json)
        {
#if UNITY_EDITOR
            string path = EditorUtility.SaveFilePanel(
                "Save Save Data",
                "",
                _lastFileName,
                "json");

            if (string.IsNullOrEmpty(path))
                return;

            try
            {
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[EditorFilePicker] Export failed.\n{ex}");
            }
#endif
        }
    }
}
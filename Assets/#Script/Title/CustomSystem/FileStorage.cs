using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Title.Custom
{
    /// <summary>
    /// 指定パスの書き込み読み込みを行う
    /// </summary>
    public static class FileStorage
    {
        private const string RootFolder = "SaveData";

#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void FS_WriteFile(string path, string text);

    [DllImport("__Internal")]
    private static extern IntPtr FS_ReadFile(string path);

    [DllImport("__Internal")]
    private static extern int FS_FileExists(string path);

    [DllImport("__Internal")]
    private static extern void FS_DeleteFile(string path);

    [DllImport("__Internal")]
    private static extern void FS_RenameFile(string oldPath, string newPath);

    [DllImport("__Internal")]
    private static extern void FS_Sync();

#endif

        public static string GetPath(string folderName,string fileName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        return $"{Application.persistentDataPath}/{FolderName}/{fileName}";
#else
            return Path.Combine(
                Directory.GetParent(Application.dataPath)!.FullName,
                RootFolder ,
                folderName,
                fileName
            );
#endif
        }

        public static async UniTask<bool> TryGetText(string folderName,string fileName, Action<string> onSuccess)
        {
            string path = GetPath(folderName,fileName);

#if UNITY_WEBGL && !UNITY_EDITOR

        if (FS_FileExists(path) == 0)
            return false;

        IntPtr ptr = FS_ReadFile(path);

        if (ptr == IntPtr.Zero)
            return false;

        string text = Marshal.PtrToStringAuto(ptr);

        onSuccess?.Invoke(text);

        await UniTask.CompletedTask;
        return true;

#else

            if (!File.Exists(path))
                return false;

            onSuccess?.Invoke(File.ReadAllText(path));

            return true;

#endif
        }

        public static async UniTask CreateFile(string folderName, string fileName, string text)
        {
            string path = GetPath(folderName,fileName);

#if UNITY_WEBGL && !UNITY_EDITOR

        FS_WriteFile(path, text);
        FS_Sync();

        await UniTask.CompletedTask;

#else

            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            try
            {
                await File.WriteAllTextAsync(path, text);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CustomPattern] {ex}");
                return;
            }

#endif
}

        public static async UniTask<bool> UpdateFile(string folderName, string fileName, string text)
        {
            string path = GetPath(folderName, fileName);

#if UNITY_WEBGL && !UNITY_EDITOR

        if (FS_FileExists(path) == 0)
            return false;

        FS_WriteFile(path, text);
        FS_Sync();

        await UniTask.CompletedTask;
        return true;

#else

            if (!File.Exists(path))
                return false;
            try
            {
                await File.WriteAllTextAsync(path, text);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CustomPattern] {ex}");
                return false;
            }
            return true;

#endif
        }

        public static async UniTask<bool> RenameFile(string  folderName ,string oldFileName, string newFileName)
        {
            string oldPath = GetPath(folderName, oldFileName);
            string newPath = GetPath(folderName, newFileName);

#if UNITY_WEBGL && !UNITY_EDITOR

        if (FS_FileExists(oldPath) == 0)
            return false;

        if (FS_FileExists(newPath) == 1)
            return false;

        FS_RenameFile(oldPath, newPath);
        FS_Sync();

        await UniTask.CompletedTask;
        return true;

#else

            if (!File.Exists(oldPath))
                return false;

            if (File.Exists(newPath))
                return false;

            string dir = Path.GetDirectoryName(newPath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            try
            {
                File.Move(oldPath, newPath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CustomPattern] {ex}");
                return false;
            }

            return true;

#endif
        }

        public static async UniTask<bool> DeleteFile(string folderName, string fileName)
        {
            string path = GetPath(folderName, fileName);

#if UNITY_WEBGL && !UNITY_EDITOR

        if (FS_FileExists(path) == 0)
            return false;

        FS_DeleteFile(path);
        FS_Sync();

        await UniTask.CompletedTask;
        return true;

#else

            if (!File.Exists(path))
                return false;

            File.Delete(path);

            return true;

#endif
        }
    }
}
using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Title.Custom
{
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

        public static string GetPath(string folderName, string fileName)
        {
#if (UNITY_WEBGL || UNITY_IOS) && !UNITY_EDITOR
            return $"{Application.persistentDataPath}/{folderName}/{fileName}";
#else

            return Path.Combine(
                Application.persistentDataPath,
                RootFolder,
                folderName,
                fileName
            );
#endif
        }

        public static string GetRootFolderPath()
        {
#if (UNITY_WEBGL || UNITY_IOS) && !UNITY_EDITOR
            return $"{Application.persistentDataPath}/{RootFolder}";
#else
            return Path.Combine(
                Application.persistentDataPath,
                RootFolder
            );
#endif
        }

        public static async UniTask<bool> TryGetText(string folderName, string fileName, Action<string> onSuccess)
        {
#if UNITY_IOS && !UNITY_EDITOR
            // iOSでは常にローカル読み込みをスキップ（メモリ保持データを使用する前提）
            await UniTask.CompletedTask;
            return false;
#elif UNITY_WEBGL && !UNITY_EDITOR
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
            string path = GetPath(folderName, fileName);
            if (!File.Exists(path))
                return false;

            onSuccess?.Invoke(File.ReadAllText(path));
            return true;
#endif
        }

        public static async UniTask CreateFile(string folderName, string fileName, string text)
        {
#if UNITY_IOS && !UNITY_EDITOR
            // iOSではストレージ保存を行わない
            await UniTask.CompletedTask;
#elif UNITY_WEBGL && !UNITY_EDITOR
            string path = GetPath(folderName, fileName);
            FS_WriteFile(path, text);
            FS_Sync();
            await UniTask.CompletedTask;
#else
            string path = GetPath(folderName, fileName);
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            try
            {
                await File.WriteAllTextAsync(path, text);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[FileStorage] {ex}");
            }
#endif
        }

        public static async UniTask<bool> UpdateFile(string folderName, string fileName, string text)
        {
#if UNITY_IOS && !UNITY_EDITOR
            // メモリ上のみで完結させるため、処理成功として扱う
            await UniTask.CompletedTask;
            return true;
#elif UNITY_WEBGL && !UNITY_EDITOR
            string path = GetPath(folderName, fileName);
            if (FS_FileExists(path) == 0)
                return false;

            FS_WriteFile(path, text);
            FS_Sync();

            await UniTask.CompletedTask;
            return true;
#else
            string path = GetPath(folderName, fileName);
            if (!File.Exists(path))
                return false;
            try
            {
                await File.WriteAllTextAsync(path, text);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[FileStorage] {ex}");
                return false;
            }
            return true;
#endif
        }

        public static async UniTask<bool> RenameFile(string folderName, string oldFileName, string newFileName)
        {
#if UNITY_IOS && !UNITY_EDITOR
            await UniTask.CompletedTask;
            return true;
#elif UNITY_WEBGL && !UNITY_EDITOR
            string oldPath = GetPath(folderName, oldFileName);
            string newPath = GetPath(folderName, newFileName);

            if (FS_FileExists(oldPath) == 0)
                return false;

            if (FS_FileExists(newPath) == 1)
                return false;

            FS_RenameFile(oldPath, newPath);
            FS_Sync();

            await UniTask.CompletedTask;
            return true;
#else
            string oldPath = GetPath(folderName, oldFileName);
            string newPath = GetPath(folderName, newFileName);

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
                Debug.LogError($"[FileStorage] {ex}");
                return false;
            }

            return true;
#endif
        }

        public static async UniTask<bool> DeleteFile(string folderName, string fileName)
        {
#if UNITY_IOS && !UNITY_EDITOR
            await UniTask.CompletedTask;
            return true;
#elif UNITY_WEBGL && !UNITY_EDITOR
            string path = GetPath(folderName, fileName);
            if (FS_FileExists(path) == 0)
                return false;

            FS_DeleteFile(path);
            FS_Sync();

            await UniTask.CompletedTask;
            return true;
#else
            string path = GetPath(folderName, fileName);
            if (!File.Exists(path))
                return false;

            File.Delete(path);
            return true;
#endif
        }

        public static async UniTask DeleteAllFile()
        {
#if UNITY_IOS && !UNITY_EDITOR
            await UniTask.CompletedTask;
#elif UNITY_WEBGL && !UNITY_EDITOR
            string path = GetRootFolderPath();
            if (FS_DirectoryExists(path) == 0)
                return;

            FS_DeleteDirectory(path, true);
            FS_Sync();

            await UniTask.CompletedTask;
#else
            string path = GetRootFolderPath();
            if (!Directory.Exists(path))
                return;

            Directory.Delete(path, true);
#endif
        }
    }
}
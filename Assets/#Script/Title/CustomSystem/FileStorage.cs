using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Title.Custom
{
    public static class FileStorage
    {
        private const string RootFolder = "SaveData";
        private static readonly List<FileOperationRequest> _requests = new();

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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Application.quitting += OnApplicationQuitting;
        }

        private static async void OnApplicationQuitting()
        {
            await Save();
        }

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

        public static void CreateFile(string folderName, string fileName, string text)
        {
            string path = GetPath(folderName, fileName);
            _requests.Add(FileOperationRequest.Create(OperationType.CreateOrUpdate, path, text));
        }

        public static void UpdateFile(string folderName, string fileName, string text)
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
            _requests.Add(FileOperationRequest.Create(OperationType.CreateOrUpdate, path, text));
#endif
        }

        public static void RenameFile(string folderName, string oldFileName, string newFileName)
        {
            string oldPath = GetPath(folderName, oldFileName);
            string newPath = GetPath(folderName, newFileName);

            _requests.Add(FileOperationRequest.Create(OperationType.Rename, oldPath, newPath));
        }

        public static void DeleteFile(string folderName, string fileName)
        {
            string path = GetPath(folderName, fileName);
            _requests.Add(FileOperationRequest.Create(OperationType.Delete, path, ""));
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

        public static async UniTask Save()
        {
#if UNITY_IOS && !UNITY_EDITOR
            _requests.Clear();
            return;
#endif
            async UniTask Create(FileOperationRequest req)
            {
                string dir = Path.GetDirectoryName(req.Path);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                try
                {
                    await File.WriteAllTextAsync(req.Path, req.Text);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[FileStorage] {ex}");
                }
            }

            async UniTask Update(FileOperationRequest req)
            {
                if (!File.Exists(req.Path))
                    return;
                try
                {
                    await File.WriteAllTextAsync(req.Path, req.Text);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[FileStorage] {ex}");
                }
            }
            async UniTask Delete(FileOperationRequest req)
            {
                if (!File.Exists(req.Path))
                    return;

                File.Delete(req.Path);
            }
            async UniTask Rename(FileOperationRequest req)
            {
                if (!File.Exists(req.Path))
                    return;

                if (File.Exists(req.Text))
                    return;

                string dir = Path.GetDirectoryName(req.Path);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                try
                {
                    File.Move(req.Path, req.Text);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[FileStorage] {ex}");
                }
            }


            foreach (var req in _requests)
            {
                switch (req.Type)
                {
                    case OperationType.CreateOrUpdate:
                        if (File.Exists(req.Path))
                            await Update(req);
                        else
                            await Create(req);
                        break;
                    case OperationType.Delete:
                        await Delete(req);
                        break;
                    case OperationType.Rename:
                        await Rename(req);
                        break;

                }
                Debug.Log(req);
            }
            _requests.Clear();
        }

        private struct FileOperationRequest
        {
            public readonly OperationType Type;
            public readonly string Path;
            public readonly string Text;

            private FileOperationRequest(OperationType type, string path, string text)
            {
                this.Type = type;
                this.Path = path;
                this.Text = text;
            }

            public static FileOperationRequest Create(OperationType type, string path, string text)
            {
                return new FileOperationRequest(type, path, text);
            }

            public override string ToString()
            {
                return $"Type {Type.ToString()} Path{Path} Text {Text}";
            }
        }

        private enum OperationType
        {
            CreateOrUpdate,
            Delete,
            Rename
        }
    }
}
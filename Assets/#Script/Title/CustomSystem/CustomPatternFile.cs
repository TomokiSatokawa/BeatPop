using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class CustomPatternFile
{
    private const string FolderName = "CustomPattern";

    public static string GetPath(string fileName)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return Path.Combine(
            Application.persistentDataPath,
            FolderName,
            fileName
        );
#else
        return Path.Combine(Directory.GetParent(Application.dataPath)!.FullName, FolderName, fileName);
#endif
    }

    public static async UniTask<bool> TryGetText(string fileName, System.Action<string> onSuccess)
    {
        string path = GetPath(fileName);

#if UNITY_WEBGL && !UNITY_EDITOR

        using var request = UnityWebRequest.Get(path);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await UniTask.Yield();
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            return false;
        }

        onSuccess?.Invoke(
            request.downloadHandler.text
        );

        return true;

#else

        if (!File.Exists(path))
        {
            return false;
        }

        onSuccess?.Invoke(
            File.ReadAllText(path)
        );

        return true;

#endif
    }

    public static async UniTask CreateFile(string fileName, string text)
    {
        string path = GetPath(fileName);

#if UNITY_WEBGL && !UNITY_EDITOR

        // WebGLÇÕpersistentDataPathÇ÷ï€ë∂
        // ïKóvÇ»ÇÁéñëOÇ…JSë§ÇÃìØä˙èàóùÇí«â¡Ç∑ÇÈ

        await File.WriteAllTextAsync(
            path,
            text
        );

#else

        string directory = Path.GetDirectoryName(path);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(
            path,
            text
        );

#endif
    }
}
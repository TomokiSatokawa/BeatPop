#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>Menu画面にシーンを移動するボタンを追加</summary>
public static class SceneTabWindow
{
    [MenuItem("Scene/InGame")]
    public static void Scene0()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(0);
    }

    [MenuItem("Scene/Editor")]
    public static void Scene01()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(1);
    }

    //[MenuItem("Scene/Stage2")]
    //public static void Scene02()
    //{
    //    EditorSceneManager.SaveOpenScenes();
    //    OpenScene(2);
    //}

    //[MenuItem("Scene/Stage3")]
    //public static void Scene03()
    //{
    //    EditorSceneManager.SaveOpenScenes();
    //    OpenScene(3);
    //}

    private static void OpenScene(int sceneIndex)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        if (!string.IsNullOrEmpty(scenePath))
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
#endif
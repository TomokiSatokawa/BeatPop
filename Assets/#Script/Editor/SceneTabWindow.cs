#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>Menu画面にシーンを移動するボタンを追加</summary>
public static class SceneTabWindow
{

    [MenuItem("Scene/0 StartScreen")]
    public static void Scene00()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(0);
    }

    [MenuItem("Scene/1 Title")]
    public static void Scene01()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(1);
    }

    [MenuItem("Scene/2 InGame")]
    public static void Scene02()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(2);
    }
    [MenuItem("Scene/3 Result")]
    public static void Scene03()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(3);
    }

    [MenuItem("Scene/4 Editor")]
    public static void Scene04()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(4);
    }
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
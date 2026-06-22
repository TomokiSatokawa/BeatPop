#if UNITY_EDITOR
using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickPlay : EditorWindow
{
    private SongData _songData;
    private int _levelIndex;
    private string _errorMessage;

    private CustomSoundData _soundData;
    private bool foldout = false;

    private string _previousScenePath;
    [MenuItem("BeatPop/QuickPlay")]
    static void OpenedWindows()
    {
        GetWindow<QuickPlay>("QuickPlay");
    }
    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        EditorGUI.BeginChangeCheck();
        _songData = (SongData)EditorGUILayout.ObjectField("曲データ", _songData, typeof(SongData), true);
        _levelIndex = EditorGUILayout.Popup("難易度", _levelIndex, new[] { "Easy", "Normal", "Hard", "Expert" });

        foldout = EditorGUILayout.Foldout(foldout, "カスタムデータ");
        if (foldout)
        {
            _soundData = (CustomSoundData)EditorGUILayout.ObjectField("カスタムサウンド", _soundData, typeof(CustomSoundData), true);
        }

        if (EditorGUI.EndChangeCheck())
        {
            _errorMessage = "";
        }

        EditorGUILayout.Space(10);
        if (GUILayout.Button("プレイ開始"))
        {
            OnStart();
        }

        if (_errorMessage != "")
        {
            EditorGUILayout.HelpBox(_errorMessage, MessageType.Error);
        }
    }
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }
    public void OnStart()
    {
        if (_songData == null)
        {
            _errorMessage = "曲データを選択してください";
            return;
        }

        TextAsset nodeJson = _songData.Charts.GetChart((Difficulty)_levelIndex);
        if (nodeJson == null)
        {
            _errorMessage = $"{(Difficulty)_levelIndex} の譜面データがありません";
            return;
        }

        _previousScenePath = EditorSceneManager.GetActiveScene().path;

        //再生準備用シーンに移動
        EditorSceneManager.SaveOpenScenes();
        string scenePath = SceneUtility.GetScenePathByBuildIndex(5);
        if (!string.IsNullOrEmpty(scenePath))
        {
            EditorSceneManager.OpenScene(scenePath);
        }

        //再生
        EditorApplication.isPlaying = true;


    }
    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            CreatePlayData();
        }

        if (state == PlayModeStateChange.EnteredEditMode)
        {
            EditorSceneManager.OpenScene(_previousScenePath);
        }
    }
    public async void CreatePlayData()
    {
        Debug.Log("CreatePlay");
        var playData = Instantiate(new GameObject());
        var songPlayData = playData.AddComponent<SongPlayManager>();

        DontDestroyOnLoad(playData);

        var songSelectData = new SongSelectData(_songData, (Difficulty)_levelIndex);
        var pattern = new PatternJsonData();
        pattern.PatternName = "QuickPlay";
        pattern.SoundPattern = _soundData.GetDefaultCustom();

        songPlayData.SetData(songSelectData, pattern);

        await UniTask.DelayFrame(3);

        SceneManager.LoadScene(2);
    }
}
#endif
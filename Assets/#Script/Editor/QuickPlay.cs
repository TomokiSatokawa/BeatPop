#if UNITY_EDITOR
using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickPlay : EditorWindow
{
    private const string SongDataKey = "QuickPlay_SongData";
    private const string LevelKey = "QuickPlay_Level";
    private const string SoundDataKey = "QuickPlay_SoundData";
    private const string FoldoutKey = "QuickPlay_Foldout";
    private const string SectionKey = "QuickPlay_Section";

    private SongData _songData;
    private int _levelIndex;
    private string _errorMessage;

    private int _startSection;

    private CustomSoundData _soundData;
    private bool foldout = false;

    private string _previousScenePath;

    private bool _isQuickPlay = false;

    [MenuItem("BeatPop/QuickPlay")]
    static void OpenedWindows()
    {
        GetWindow<QuickPlay>("QuickPlay");
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
        Load();
    }

    private void OnDisable()
    {
        Save();
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);

        EditorGUI.BeginChangeCheck();

        _songData = (SongData)EditorGUILayout.ObjectField("曲データ", _songData, typeof(SongData), true);
        _levelIndex = EditorGUILayout.Popup("難易度", _levelIndex, new[] { "Easy", "Normal", "Hard", "Expert" });
        _startSection = EditorGUILayout.DelayedIntField("開始セクション", _startSection);
        foldout = EditorGUILayout.Foldout(foldout, "カスタムデータ");
        if (foldout)
        {
            _soundData = (CustomSoundData)EditorGUILayout.ObjectField("カスタムサウンド", _soundData, typeof(CustomSoundData), true);
        }

        if (EditorGUI.EndChangeCheck())
        {
            _errorMessage = "";
            Save();
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("プレイ開始"))
        {
            _isQuickPlay = true;
            OnStart();
        }

        if (!string.IsNullOrEmpty(_errorMessage))
        {
            EditorGUILayout.HelpBox(_errorMessage, MessageType.Error);
        }
    }

    private void Save()
    {
        EditorPrefs.SetString(SongDataKey, AssetDatabase.GetAssetPath(_songData));
        EditorPrefs.SetInt(LevelKey, _levelIndex);
        EditorPrefs.SetString(SoundDataKey, AssetDatabase.GetAssetPath(_soundData));
        EditorPrefs.SetBool(FoldoutKey, foldout);
        EditorPrefs.SetInt(SectionKey, _startSection);
    }

    private void Load()
    {
        string songPath = EditorPrefs.GetString(SongDataKey, "");
        if (!string.IsNullOrEmpty(songPath))
        {
            _songData = AssetDatabase.LoadAssetAtPath<SongData>(songPath);
        }

        _levelIndex = EditorPrefs.GetInt(LevelKey, 0);

        string soundPath = EditorPrefs.GetString(SoundDataKey, "");
        if (!string.IsNullOrEmpty(soundPath))
        {
            _soundData = AssetDatabase.LoadAssetAtPath<CustomSoundData>(soundPath);
        }

        foldout = EditorPrefs.GetBool(FoldoutKey, false);
        _startSection = EditorPrefs.GetInt(SectionKey, 0);
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

        EditorSceneManager.SaveOpenScenes();

        string scenePath = SceneUtility.GetScenePathByBuildIndex(5);
        if (!string.IsNullOrEmpty(scenePath))
        {
            EditorSceneManager.OpenScene(scenePath);
        }

        EditorApplication.isPlaying = true;
    }

    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (!_isQuickPlay) return;

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            CreatePlayData();
        }

        if (state == PlayModeStateChange.EnteredEditMode)
        {
            _isQuickPlay = false;

            if (!string.IsNullOrEmpty(_previousScenePath))
            {
                EditorSceneManager.OpenScene(_previousScenePath);
            }
        }
    }

    public async void CreatePlayData()
    {
        var playData = new GameObject("QuickPlayData");
        var songPlayData = playData.AddComponent<SongPlayManager>();

        DontDestroyOnLoad(playData);

        var songSelectData = new SongSelectData(_songData, (Difficulty)_levelIndex);
        if (_songData == null) return;
        var pattern = new PatternJsonData
        {
            PatternName = "QuickPlay",
            SoundPattern = _soundData.GetDefaultCustom()
        };

        songPlayData.SetData(songSelectData, pattern,_startSection);

        await UniTask.DelayFrame(3);

        SceneManager.LoadScene(2);
    }
}
#endif
using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using InGame.Node;
using Input;
using R3;
using Sound;
using TMPro;
using UnityEngine;
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private SceneLoad _sceneLoad;
    [SerializeField] private float _waitSeconds;
    [SerializeField] private AudioClip _songClip;
    [SerializeField] private float _bpm;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private float _timeOffset;
    [SerializeField] private float _resultDelay = 2f;
    public float StageTime { get; private set; } = -1;
    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;
    public float BPM => _bpm;
    public AudioClip SongClip => _songClip;

    private Subject<Unit> _onGameClear = new();
    public Observable<Unit> OnGameClear => _onGameClear;

    private double _startDspTime;
    private float _endTime = float.MaxValue;

    private void Start()
    {
        NodeGenerator.I?.OnFileLoaded.Subscribe(async fileData =>
        {
            InitializeSong(fileData.BPM);
            _endTime = fileData.Nodes[fileData.Nodes.Count - 1].Time;
            await LoadPlayAsync();
        }).AddTo(this);
    }

    private void InitializeSong(float bpm)
    {
        _bpm = bpm;
        _songClip = SongPlayManager.I.SongData.SongData.Audio;

    }

    private async UniTask LoadPlayAsync()
    {
        if (SoundManager.I == null) return;
        StageTime = float.MinValue;
        _songClip.LoadAudioData();

        await UniTask.WaitUntil(() => _songClip.loadState == AudioDataLoadState.Loaded);
        _startDspTime = SoundManager.I.PlayBGMSound(_songClip, _waitSeconds);
        StageTime = -_waitSeconds;
        _isPlaying = true;

    }

    private void Update()
    {
        UpdateDebugText();

        if (!_isPlaying) return;

        UpdateStageTime();
    }

    public void OnPause()
    {
        _isPlaying = false;
        InputManager.SetInputEnabled(false);
        SoundManager.I.IsPause(true);
    }

    public void ReStartStage()
    {
        float offset = SongPlayManager.I.SongData.SongData.StageTimeOffSet;
        _startDspTime = AudioSettings.dspTime - (StageTime - offset);
        _isPlaying = true;
        SoundManager.I.IsPause(false);
    }

    public void ReStartCountDown()
    {
        InputManager.SetInputEnabled(true);
    }

    public void Retry()
    {
        _sceneLoad.ChangeScene("InGame");
    }

    public void ReturnTitle()
    {
        _sceneLoad.ChangeScene("Title");
    }

    private void UpdateStageTime()
    {
        float offset = SongPlayManager.I.SongData.SongData.StageTimeOffSet;
        StageTime = (float)(AudioSettings.dspTime - _startDspTime) + offset;

        if (StageTime >= _endTime + _resultDelay)
        {
            _isPlaying = false;
            _onGameClear.OnNext(Unit.Default);
        }
    }

    private void UpdateDebugText()
    {
        if (_timeText != null)
        {
            _timeText.text = StageTime.ToString("N2");
        }
    }
}

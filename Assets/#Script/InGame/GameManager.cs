using Cysharp.Threading.Tasks;
using InGame.Node;
using R3;
using Sound;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager I;
    [HideInInspector] public float StageTime = -1;
    [SerializeField] private SongDataBase _songData;
    [SerializeField] private float _waitSeconds;
    [SerializeField] private AudioClip _songClip;
    [SerializeField] private float _bpm;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private float _timeOffset;
    private bool _isPlaying = false;
    private double _startDspTime;
    private float _endTime = float.MaxValue;
    public float BPM => _bpm;
    public AudioClip SongClip => _songClip;

    private Subject<Unit> _onGameClear = new();
    public Observable<Unit> OnGameClear => _onGameClear;
    public void Awake()
    {
        if (I == null) I = this;

    }
    public void Start()
    {
        NodeGenerator.I?.OnFileLoaded.Subscribe(_ =>
        {
            _endTime = NodeGenerator.I.NodeDates[NodeGenerator.I.NodeDates.Count - 1].Time;
        WaitLoad();
        }).AddTo(this);
    }
    public void SetData(float bpm, int index)
    {
        _bpm = bpm;
        if (index >= 0 && index < _songData._audioClipList.Count)
        {
            _songClip = _songData._audioClipList[index];
        }
    }
    public async void WaitLoad()
    {
        if (SoundManager.I == null) return;
        StageTime = float.MinValue;
        _songClip.LoadAudioData();

        await UniTask.WaitUntil(() => _songClip.loadState == AudioDataLoadState.Loaded);
        _startDspTime = SoundManager.I.PlayBGMSound(_songClip, _waitSeconds);
        StageTime = -_waitSeconds;
        _isPlaying = true;

    }

    void Update()
    {
        if (_timeText != null)
        {
            _timeText.text = StageTime.ToString("N2");
        }

        if (!_isPlaying) return;

        StageTime = (float)(AudioSettings.dspTime - _startDspTime) + _timeOffset;

        //if (StageTime >= _endTime + 2f)
        //{
        //    _isPlaying = false;
        //    _onGameClear.OnNext(Unit.Default);
        //}
    }
}

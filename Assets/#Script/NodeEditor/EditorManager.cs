using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EditorManager : SingletonMonoBehaviour<EditorManager>
{
    [SerializeField] private SongListDataBase _songData;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _bpm;
    [SerializeField] private Scrollbar _scrollBar;
    [SerializeField] private float _magnification;
    [SerializeField] private RectTransform _displayRange;
    [SerializeField ] private int _divition;
    [SerializeField] private float _timeOffSet;
    public float DisplayRange => _displayRange.sizeDelta.x;
    public float Magnification => _magnification;
    public float BPM => _bpm;
    public int Divition => _divition;
    public AudioClip AudioClip => _audioClip;
    public int SongIndex { get; private set; } = -1;

    private readonly ReactiveProperty<bool> _isPlaying = new();
    public ReadOnlyReactiveProperty<bool> IsPlaying => _isPlaying;

    private readonly ReactiveProperty<double> _editorTime = new();
    public ReadOnlyReactiveProperty<double> EditorTime => _editorTime;
    public double StartDsp;

    private void Start()
    {
        _editorTime.Value = 0;
        _isPlaying.Value = false;

        EditorTime
            .Subscribe(time =>
            {
                _scrollBar.SetValueWithoutNotify(
                    (float)(time / _audioClip.length));
            })
            .AddTo(this);
    }
    public void SetData(float bpm,int soundIndex)
    {
        _bpm = bpm;
        var song = _songData.GetSongData(soundIndex);
        if (song == null)
        {
            _audioClip = song.Audio;
        }
    }
    public void ReloadTime()
    {
        _editorTime.Value = _editorTime.Value;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _isPlaying.Value = !_isPlaying.Value;
            _scrollBar.interactable = !IsPlaying.CurrentValue;

            if (_isPlaying.CurrentValue)
            {
                StartDsp = AudioSettings.dspTime - _editorTime.Value;
            }

        }

        if (_isPlaying.CurrentValue)
        {
            _editorTime.Value = AudioSettings.dspTime - StartDsp + _timeOffSet;
        }
    }

    public void OnBarChangeValue(float value)
    {
        _editorTime.Value = _audioClip.length * value;

        // çƒê∂íÜÇ…ÉVÅ[ÉNÇµÇΩèÍçáÇ‡ìØä˙
        if (_isPlaying.CurrentValue)
        {
            StartDsp = AudioSettings.dspTime - _editorTime.Value;
        }
    }
    public void ChangeDivisionCount(int count)
    {
        _divition = count;
    }
}
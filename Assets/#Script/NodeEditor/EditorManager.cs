using Common;
using Common.PlaySystem;
using InGame.UI;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EditorManager : SingletonMonoBehaviour<EditorManager>
{
    [SerializeField] private SongListDataBase _songData;
    [SerializeField] private Scrollbar _scrollBar;
    [SerializeField] private RectTransform _displayRange;
    [SerializeField] private float _keyMoveAmount;
    [SerializeField] private int _division;
    [SerializeField] private Difficulty _difficulty;
    public TextMeshProUGUI _te;
    public float DisplayRange => _displayRange.sizeDelta.x;
    [SerializeField] private float _magnification;
    public float Magnification => _magnification;
    public int Division => _division;

    private AudioClip _audio;
    public AudioClip Audio => _audio;
    private void Start()
    {
        EditorNodeData.I?.LoadedFile.Subscribe(x => Initialize(x)).AddTo(this);
        EditorLightData.I?.LoadedFile.Subscribe(x => Initialize(x)).AddTo(this);
        StageTimeController.I.IsPlaying.Subscribe(x => _scrollBar.interactable = !x).AddTo(this);
    }
    public async void Initialize(NodeSaveData saveData)
    {
        if (saveData == null) return;
        GenerateSongPlayManager(saveData.SoundIndex);
        StageTimeController.I.SetPlayData(saveData);
        await StageTimeController.I.SongLoadAsync();
        StageTimeController.I.StartSongPlay();
        StageTimeController.I.Pause();
        _audio = _songData.GetSongData(saveData.SoundIndex).Audio;

    }
    public async void Initialize(StageSaveData saveData)
    {
        if (saveData == null) return;
       var data=  GenerateSongPlayManager(saveData.SongDataIndex);
        StageTimeController.I.SetPlayData(data.BPM,0,0);
        await StageTimeController.I.SongLoadAsync();
        StageTimeController.I.StartSongPlay();
        StageTimeController.I.Pause();
        _audio = _songData.GetSongData(saveData.SongDataIndex).Audio;
    }
    private void Update()
    {
        _te.text = StageTimeController.StageTime.ToString("N2");

        StageTimeController.I.UpdateStageTime();
        if (EditorNodeData.I != null && EditorNodeData.I.LoadedFile.CurrentValue == null) return;
        if (EditorLightData.I != null && EditorLightData.I.LoadedFile.CurrentValue == null) return;

        if (StageTimeController.I.IsPlaying.CurrentValue)
        {
            _scrollBar.SetValueWithoutNotify((float)(StageTimeController.StageTime / _audio.length));
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (StageTimeController.I.IsPlaying.CurrentValue)
            {
                StageTimeController.I.Pause();
            }
            else
            {
                StageTimeController.I.ReStart();
            }
        }

        //if(Keyboard.current.rightArrowKey.wasPressedThisFrame)
        //{
        //    _editorTime.Value = Mathf.Clamp((float)EditorTime.CurrentValue + _keyMoveAmount ,0, _audioClip.length);
        //}

        //if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        //{
        //    _editorTime.Value = Mathf.Clamp((float)EditorTime.CurrentValue - _keyMoveAmount ,0, _audioClip.length);
        //}
    }

    public void OnBarChangeValue(float value)
    {
        float time = _audio.length * value;
        float moveAmount = time - StageTimeController.StageTime;
        StageTimeController.I.MoveStageTime(moveAmount);
    }
    public void ChangeDivisionCount(int count)
    {
        _division = count;
    }
    public IReadOnlySongData GenerateSongPlayManager(int index)
    {
        if(SongPlayManager.I == null)
        {
            Debug.LogError("SongPlayManagerがありません");
            return null;
        }

        var songData = _songData.GetSongData(index);
        SongPlayManager.I.SetData(new SongSelectData(songData, _difficulty), null,0);

        return songData;
    }
}
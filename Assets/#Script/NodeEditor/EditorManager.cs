using InGame.UI;
using R3;
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
    public float DisplayRange => _displayRange.sizeDelta.x;
    [SerializeField] private float _magnification;
    public float Magnification => _magnification;
    public int Division => _division;

    private AudioClip _audio;
    public AudioClip Audio => _audio;
    private void Start()
    {
        EditorNodeData.I.LoadedFile.Subscribe(x => Initialize(x)).AddTo(this);
    }
    public void Initialize(NodeSaveData saveData)
    {
        StageTimeController.I.SetPlayData(saveData);
        _audio = _songData.GetSongData(saveData.SoundIndex).Audio;

    }
    private void Update()
    {
        _scrollBar.SetValueWithoutNotify((float)(StageTimeController.StageTime / _audio.length));

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
}
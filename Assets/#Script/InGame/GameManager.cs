using Cysharp.Threading.Tasks;
using InGame.Node;
using InGame.UI;
using Input;
using R3;
using Sound;
using TMPro;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private SceneLoad _sceneLoad;
    [SerializeField] private StageTimeController _stageTimeController;
    [SerializeField] private TextMeshProUGUI _timeText;

    private void Start()
    {
        NodeGenerator.I?.OnFileLoaded.Subscribe(async fileData =>
        {
            await LoadPlayAsync(fileData);
        }).AddTo(this);
    }

    private async UniTask LoadPlayAsync(NodeSaveData fileData)
    {
        if (SoundManager.I == null) return;

        _stageTimeController.SetPlayData(fileData);

        await _stageTimeController.SongLoadAsync();

        _stageTimeController.StartSongPlay();
    }

    private void Update()
    {
        UpdateDebugText();

        _stageTimeController.UpdateStageTime();
    }

    public void OnPause()
    {
        _stageTimeController.Pause();
        InputManager.SetInputEnabled(false);
        SoundManager.I.IsPause(true);
    }

    public void ReStartStage()
    {
        _stageTimeController.ReStart();
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

    private void UpdateDebugText()
    {
        if (_timeText != null)
        {
            _timeText.text = StageTimeController.StageTime.ToString("N2");
        }
    }
}

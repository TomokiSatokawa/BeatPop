using Common;
using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using InGame;
using InGame.Score;
using Input;
using R3;
using Sound;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private SceneTransition _sceneLoad;
    [SerializeField] private StageTimeController _stageTimeController;

    private void Start()
    {
        InputManager.SetInputEnabled(true);

        StageTimeController.I.OnGameClear.Subscribe(_ =>
        {
            StageTimeController.I.Pause();
            InputManager.SetInputEnabled(false);
        }).AddTo(this);

        Initialize().Forget();
    }

    private async UniTask Initialize()
    {
        InGameFileLoad.I?.OnNodeFileLoaded.Skip(1).Subscribe(async fileData =>
        {
            LoadPlayAsync(fileData).Forget();
        }).AddTo(this);

        await UniTask.Yield();
        await InGameFileLoad.I.FileLoad();
    }

    private async UniTask LoadPlayAsync(NodeSaveData fileData)
    {
        if (SoundManager.I == null) return;

        _stageTimeController.SetPlayData(fileData);

        UniTask songLoad = _stageTimeController.SongLoadAsync();
        UniTask poolClone = PoolManager.I.ClonePoolObject();

        await UniTask.WhenAll(songLoad, poolClone);

        _stageTimeController.StartSongPlay();
    }

    private void Update()
    {
        _stageTimeController.UpdateStageTime();
    }

    public void OnPause()
    {
        _stageTimeController.Pause();
        InputManager.SetInputEnabled(false);
        SoundManager.BGM.IsPause(true);
    }

    public void ReStartStage()
    {
        _stageTimeController.ReStart();
        SoundManager.BGM.IsPause(false);
    }

    public void ReStartCountDown()
    {
        InputManager.SetInputEnabled(true);
    }

    public void Retry()
    {
        DontDestroyRelease();
        _sceneLoad.ChangeScene("InGame");
    }

    public void ReturnTitle()
    {
        DontDestroyRelease();
        _sceneLoad.ChangeScene("Title");
    }

    public void Clear()
    {
        _sceneLoad.ChangeScene("Result");
    }

    public static void DontDestroyRelease()
    {
        ScoreDataManager.DisposeSingleton();
        SongPlayContext.DisposeSingleton();
    }
}

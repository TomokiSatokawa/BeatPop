using System.Linq;
using Common;
using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using InGame;
using InGame.Score;
using InGame.UI;
using Input;
using R3;
using Sound;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private SceneTransition _sceneLoad;
    [SerializeField] private StageTimeController _stageTimeController;
    [SerializeField] private TutorialUIControl _tutorialUIControl;

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
            //チュートリアルを表示する
            if (SongPlayContext.I.SongData.SongData.ShowTutorial)
            {
                _tutorialUIControl.OnClause += () => LoadPlayAsync(fileData).Forget();
                _tutorialUIControl.ShowTutorial();
            }
            else
            {
                LoadPlayAsync(fileData).Forget();
            }
        }).AddTo(this);

        await UniTask.Yield();
        await InGameFileLoad.I.FileLoad();
    }

    private async UniTask LoadPlayAsync(NodeSaveData fileData)
    {
        if (SoundManager.I == null) return;

        _stageTimeController.SetPlayData(fileData);

        UniTask songLoad = SoundManager.I.LoadAudioClipAsync(StageTimeController.I.SongClip);
        UniTask seLoad = SoundManager.I.LoadAudioClipAsync(InGameCustomSoundData.I.NodeSE.Values.Select(x => x.Clip).ToArray());
        UniTask poolClone = PoolManager.I.ClonePoolObject();

        await UniTask.WhenAll(songLoad, seLoad, poolClone);

        _stageTimeController.StartSongPlay();
    }

    private void Update()
    {
        _stageTimeController.UpdateStageTime();
    }

    public void Pause()
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
        ScoreDataManager.DisposeSingleton();
        StageConfig.DisposeSingleton();
        SoundManager.BGM.PlayBGM(null);
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
        StageConfig.DisposeSingleton();
    }
}

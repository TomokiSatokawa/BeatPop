using Common.BeatUpdate;
using Common.PlaySystem;
using InGame;
using InGame.Score;
using R3;
using Title.Custom;
using UnityEngine;

namespace Preview
{
    /// <summary>
    /// Previewä«óù
    /// </summary>
    public class PreviewManager : SingletonMonoBehaviour<PreviewManager>
    {
        [SerializeField] private StageTimeController _stageTimeController;
        [SerializeField] private PreviewNodeGenerator _previewNodeGenerator;
        [SerializeField] private CustomStageData _customStageData;
        [SerializeField] private StageConfig _stageConfig;
        [SerializeField] private float _bpm;

        private void Start()
        {
            _stageTimeController.SetPlayData(_bpm, float.MaxValue, 0);
            _stageTimeController.StartSongPlay();
            _stageTimeController.OnGameClear.Subscribe(_ => _stageTimeController.StartSongPlay()).AddTo(this);
        }

        private void Update()
        {
            _stageTimeController.UpdateStageTime();
        }

        public void OnChangeValue(float speed)
        {
            float second = _customStageData.GetSpeedSecond(speed);
            _previewNodeGenerator.Initialize(second);
            _stageConfig.ChangeArrivalSeconds(second);
        }
        public static void DontDestroyRelease()
        {
            ScoreDataManager.DisposeSingleton();
            SongPlayContext.DisposeSingleton();
            StageConfig.DisposeSingleton();
            StageTimeController.Depose();
            InGameCustomColorData.Depose();
            PoolManager.Depose();
            BeatUpdateManager.Depose();
        }
    }
}
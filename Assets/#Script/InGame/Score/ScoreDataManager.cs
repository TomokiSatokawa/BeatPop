using InGame.Node;
using R3;
using UnityEngine;

namespace InGame.Score
{
    /// <summary>
    /// スコア関連のデータを一括管理する
    /// </summary>
    public class ScoreDataManager : SingletonPersistent<ScoreDataManager>
    {
        private static readonly ScoreData _scoreData = new();
        public static IReadOnlyScoreData ScoreData => _scoreData;

        private static readonly JudgementRecorder _judgementRecorder = new();
        public static IReadOnlyJudgementRecorder JudgementRecorder => _judgementRecorder;

        private static readonly ResultDataCollector _resultDataCollector = new();
        public static IReadOnlyResultData ResultDataCollector => _resultDataCollector;

        protected override void OnAwake()
        {
            _scoreData.Initialize();
            _judgementRecorder.Initialize();
            _resultDataCollector.Initialize();
        }

        private void Start()
        {
            StageTimeController.I.OnInitialized.Subscribe(_ =>
            {
                var saveData = InGameFileLoad.I.OnNodeFileLoaded.CurrentValue;
                _scoreData.CalculateMaxScore(saveData.Nodes, StageTimeController.I.BPM);
            }).AddTo(this);
        }

        public IReadOnlyJudgementData RecordJudge(NodeData nodeData, float difference)
        {
            var judgementTable = StageConfig.I.JudgementTable;

            var judgement = judgementTable.GetJudgementResult(nodeData.PrefabType, difference);
            var judgementData = judgementTable.GetJudgementData(nodeData.PrefabType);

            _scoreData.AddScore(judgementData, judgement);

            _judgementRecorder.AddJudgeCount(judgement);

            _resultDataCollector.AddNode(nodeData, judgement);
            _resultDataCollector.AddDifferenceValue(judgement, nodeData, difference);

            return judgement;
        }
    }
}
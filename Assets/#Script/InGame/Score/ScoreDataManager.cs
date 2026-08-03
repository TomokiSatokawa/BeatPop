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
        /// <summary>スコアデータ</summary>
        private static readonly ScoreData _scoreData = new();
        public static IReadOnlyScoreData ScoreData => _scoreData;

        /// <summary>判定記録</summary>
        private static readonly JudgementRecorder _judgementRecorder = new();
        public static IReadOnlyJudgementRecorder JudgementRecorder => _judgementRecorder;

        /// <summary>リザルト用データ</summary>
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
            //BPM設定後に最大スコアを計算する
            StageTimeController.I.OnInitialized.Subscribe(_ =>
            {
                var saveData = InGameFileLoad.I.OnNodeFileLoaded.CurrentValue;
                _scoreData.CalculateMaxScore(saveData.Nodes, StageTimeController.I.BPM);
            }).AddTo(this);
        }

        /// <summary>
        ///　ノーツを判定して記録する
        /// </summary>
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
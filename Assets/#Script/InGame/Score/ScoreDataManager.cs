using InGame.Node;
using UnityEngine;

namespace InGame.Score
{
    /// <summary>
    /// スコア関連のデータを一括管理する
    /// </summary>
    public class ScoreDataManager : SingletonPersistent<ScoreDataManager>
    {
        [SerializeField] private JudgementTable _judgementTable;

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

        public IReadOnlyJudgementData RecordJudge(NodeData nodeData,float difference)
        {
            var judgement = _judgementTable.GetJudgement(nodeData.PrefabType, difference);

            _scoreData.AddScore(judgement);

            _judgementRecorder.AddJudgeCount(judgement);

            _resultDataCollector.AddNode(nodeData, judgement);
            _resultDataCollector.AddDifferenceValue(nodeData,difference);

            return judgement;
        }
    }
}
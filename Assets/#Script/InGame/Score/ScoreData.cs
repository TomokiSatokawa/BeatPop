using System.Collections.Generic;
using InGame.Node;
using R3;
using UnityEngine;

namespace InGame.Score
{
    /// <summary>
    /// 基本的なスコアを管理
    /// </summary>
    public class ScoreData : IReadOnlyScoreData
    {
        //TODO:ScriptableObjectから一括変更できるようにする
        private const int LongNoteDivisionInterval = 8;
        private int _maxScore;
        /// <summary>最大スコア</summary>
        int IReadOnlyScoreData.MaxScore => _maxScore;

        private bool _isAllPerfect;
        /// <summary>オールパーフェクトが続いているか</summary>
        bool IReadOnlyScoreData.IsAllPerfect => _isAllPerfect;

        private readonly ReactiveProperty<int> _combo = new();
        /// <summary>コンボ数</summary>
        ReadOnlyReactiveProperty<int> IReadOnlyScoreData.Combo => _combo;

        private readonly ReactiveProperty<int> _score = new();
        /// <summary>スコア</summary>
        ReadOnlyReactiveProperty<int> IReadOnlyScoreData.Score => _score;

        public void Initialize()
        {
            _isAllPerfect = true;
            _combo.Value = 0;
            _score.Value = 0;
            _maxScore = 0;
        }

        /// <summary>
        /// スコア加算
        /// </summary>
        public void AddScore(IReadOnlyJudgementData judgementData)
        {
            UpdateJudgeState(judgementData);
            _score.Value += judgementData.Score;
        }

        public void CalculateMaxScore(JudgementTable judgementTable, IReadOnlyList<NodeData> nodeDatas, float bpm)
        {
            int maxScore = 0;
            foreach (NodeData nodeData in nodeDatas)
            {
                maxScore += judgementTable.GetJudgementResult(nodeData.PrefabType, 0).Score;

                if (nodeData.PrefabType != PoolPrefabType.HoldNoteStart)
                    continue;

                //HoldFillの計算
                NodeData start = nodeData;

                if (start.Connect < 0 || start.Connect >= nodeDatas.Count)
                {
                    Debug.LogError($"[Score] HoldNoteの接続先が不正です。Time={start.Time}, Connect={start.Connect}, NodeCount={nodeDatas.Count}");
                    continue;
                }

                NodeData end = nodeDatas[start.Connect];

                float intervalTime = (60f / bpm) * (4f / LongNoteDivisionInterval);
                float duration = end.Time - start.Time;
                int count = Mathf.FloorToInt(duration / intervalTime);

                maxScore += judgementTable.GetJudgementResult(PoolPrefabType.HoldNoteFill, 0).Score * count;
            }

            _maxScore = maxScore;
        }

        private void UpdateJudgeState(IReadOnlyJudgementData judgement)
        {
            if (!judgement.IsAllPerfectContinued)
                _isAllPerfect = false;

            if (judgement.IsComboContinued)
            {
                _combo.Value++;
            }
            else
            {
                _combo.OnNext(0);
            }
        }

    }

    public interface IReadOnlyScoreData
    {
        public int MaxScore { get; }
        public bool IsAllPerfect { get; }
        public ReadOnlyReactiveProperty<int> Combo { get; }
        public ReadOnlyReactiveProperty<int> Score { get; }
    }
}

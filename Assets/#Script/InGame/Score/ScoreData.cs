using System.Collections.Generic;
using InGame.Node;
using R3;
using Result.UI;
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
        private bool _isFullCombo;
        /// <summary>オールパーフェクトが続いているか</summary>
        bool IReadOnlyScoreData.IsFullCombo => _isFullCombo;

        private readonly ReactiveProperty<int> _combo = new();
        /// <summary>コンボ数</summary>
        ReadOnlyReactiveProperty<int> IReadOnlyScoreData.Combo => _combo;

        private readonly ReactiveProperty<int> _score = new();
        /// <summary>スコア</summary>
        ReadOnlyReactiveProperty<int> IReadOnlyScoreData.Score => _score;

        public void Initialize()
        {
            _isAllPerfect = true;
            _isFullCombo = true;
            _combo.Value = 0;
            _score.Value = 0;
            _maxScore = 0;
        }

        /// <summary>
        /// スコア加算
        /// </summary>
        public void AddScore( IReadOnlyNodeJudgement nodeJudgment, IReadOnlyJudgementData result)
        {
            UpdateJudgeState(result);
            _score.Value += GetScore(nodeJudgment,result);
        }

        /// <summary>
        /// 最大値スコアを計算する
        /// </summary>
        public void CalculateMaxScore(IReadOnlyList<NodeData> nodeDatas, float bpm)
        {
            int maxScore = 0;
            var judgementTable = StageConfig.I.JudgementTable;

            //ノーツごとの最大値を足す
            foreach (NodeData nodeData in nodeDatas)
            {
                var nodeJudgment = judgementTable.GetJudgementData(nodeData.PrefabType);
                var result = judgementTable.GetJudgementResult(nodeData.PrefabType, 0);

                //最大スコアを加算
                maxScore += GetScore(nodeJudgment,result);

                if (nodeData.PrefabType != PoolPrefabType.HoldNoteStart)
                    continue;

                //HoldFillの計算
                NodeData start = nodeData;

                //Fillの接続先があるか
                if (start.Connect < 0 || start.Connect >= nodeDatas.Count)
                {
                    Debug.LogError($"[Score] HoldNoteの接続先が不正です。Time={start.Time}, Connect={start.Connect}, NodeCount={nodeDatas.Count}");
                    continue;
                }

                NodeData end = nodeDatas[start.Connect];

                //Fillの判定数を取得
                float intervalTime = (60f / bpm) * (4f / StageConfig.I.LongNoteDivisionInterval);
                float duration = end.Time - start.Time;
                int count = Mathf.FloorToInt(duration / intervalTime);

                //Fill分の最大スコアを加算
                maxScore += GetScore(judgementTable.GetJudgementData(PoolPrefabType.HoldNoteFill), judgementTable.GetJudgementResult(PoolPrefabType.HoldNoteFill, 0)) * count;
            }

            _maxScore = maxScore;
        }

        /// <summary>
        /// 現在のリザルトタイプを取得
        /// </summary>
        public ResultType GetResultType()
        {
            if (_isAllPerfect)
                return ResultType.AllPerfect;

            if(_isFullCombo) 
                return ResultType.FullCombo;

            return ResultType.Clear;
        }

        /// <summary>
        /// スコア計算
        /// </summary>
        private static int GetScore(IReadOnlyNodeJudgement type, IReadOnlyJudgementData result)
        {
            //スコア倍率0以下
            if(result.ScoreMultiplier <= 0)
            {
                return 0;
            }

            return Mathf.RoundToInt(type.MaxScore * result.ScoreMultiplier + type.MaxScore);
        }

        private void UpdateJudgeState(IReadOnlyJudgementData judgement)
        {
            //オールパーフェクトを解除
            if (!judgement.IsAllPerfectContinued)
                _isAllPerfect = false;

            //コンボ
            if (judgement.IsComboContinued)
            {
                _combo.Value++;
            }
            else
            {
                _combo.OnNext(0);
                _isFullCombo = false;
            }
        }

    }

    public interface IReadOnlyScoreData
    {
        public int MaxScore { get; }
        public bool IsAllPerfect { get; }
        public bool IsFullCombo { get; }
        public ReadOnlyReactiveProperty<int> Combo { get; }
        public ReadOnlyReactiveProperty<int> Score { get; }

        public ResultType GetResultType();
    }
}

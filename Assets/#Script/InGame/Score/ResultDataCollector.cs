using System.Collections.Generic;
using InGame.Node;

namespace InGame.Score
{
    /// <summary>
    /// リザルト用統計データを管理
    /// </summary>
    public class ResultDataCollector : IReadOnlyResultData
    {
        /// <summary>ノード別ヒット数（打率）</summary>
        private readonly Dictionary<PoolPrefabType, HitData> _nodeHitCount = new();
        public IReadOnlyDictionary<PoolPrefabType, HitData> NodeHitCount => _nodeHitCount;

        private int _fastCount;
        /// <summary>タイミングが早い数</summary>
        public int FastCount => _fastCount;
        private int _lateCount;
        /// <summary>タイミングが遅い数</summary>
        public int LateCount => _lateCount;

        public void Initialize()
        {
            _nodeHitCount.Clear();
            _fastCount = 0;
            _lateCount = 0;
        }

        /// <summary>
        /// 打率追加
        /// </summary>
        public void AddNode(NodeData node, IReadOnlyJudgementData judgementData)
        {
            var type = node.PrefabType;

            //存在しなかったら作成する
            if (!_nodeHitCount.TryGetValue(type, out var hitData))
            {
                hitData = new HitData();
                _nodeHitCount.Add(type, hitData);
            }

            //コンボ継続するか
            if (judgementData.IsComboContinued)
            {
                _nodeHitCount[type].AddHit();
            }
            else
            {
                _nodeHitCount[type].AddMiss();
            }
        }

        /// <summary>
        /// タイミング加算 
        /// </summary>
        public void AddDifferenceValue(IReadOnlyJudgementData judgementData, NodeData nodeData, float difference)
        {
            //ホールドは含まない
            if (nodeData.PrefabType == PoolPrefabType.HoldNoteFill)
                return;

            //パーフェクト、ミスは含まない
            if (judgementData.Name == JudgementType.PERFECT
                || judgementData.Name == JudgementType.MISS)
                return;

            //速い遅いを振り分ける
            if (difference > 0)
                _fastCount++;
            else
                _lateCount++;
        }


    }
    /// <summary>
    /// ノーツ別のHitカウンター
    /// </summary>
    public class HitData
    {
        public int TotalCount { get; private set; }
        public int HitCount { get; private set; }

        /// <summary>打率</summary>
        public float Accuracy => TotalCount == 0 ? 0f : (float)HitCount / TotalCount * 100f;

        public HitData()
        {
            HitCount = 0;
            TotalCount = 0;
        }

        public void AddHit()
        {
            HitCount++;
            TotalCount++;
        }

        public void AddMiss()
        {
            TotalCount++;
        }
    }

    public interface IReadOnlyResultData
    {
        public IReadOnlyDictionary<PoolPrefabType, HitData> NodeHitCount { get; }
        public int FastCount { get; }
        public int LateCount { get; }
    }
}
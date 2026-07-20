
using System.Collections.Generic;
using InGame.Node;

namespace InGame.Score
{
   /// <summary>
   /// リザルト用統計データを管理
   /// </summary>
    public class ResultDataCollector  : IReadOnlyResultData
    {
        /// <summary>ノード別ヒット数（打率）</summary>
        private readonly Dictionary<PoolPrefabType, HitData> _nodeHitCount = new();

        /// <summary>Fast / Late の数値の合計</summary>
        private float _sumDifferenceOffset = 0;
        public float SumDifferenceOffset => _sumDifferenceOffset;

        public void Initialize()
        {
            _nodeHitCount.Clear();
        }

        /// <summary>
        /// 打率追加
        /// </summary>
        public void AddNode(NodeData node , IReadOnlyJudgementData judgementData)
        {
            var type = node.PrefabType;

            if (!_nodeHitCount.ContainsKey(type))
            {
                _nodeHitCount.Add(type, new());
            }

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
       public void AddDifferenceValue(NodeData nodeData, float difference)
        {
            //ホールドは含まない
            if (nodeData.PrefabType == PoolPrefabType.HoldNoteFill)
                return;

            _sumDifferenceOffset += difference;
        }

        /// <summary>
        /// ノーツ別のHitカウンター
        /// </summary>
        public class HitData
        {
            public int TotalCount { get; private set; }
            public int HitCount { get; private set; }

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
    }
    public interface IReadOnlyResultData
    {
        public float SumDifferenceOffset { get; }
    }
}

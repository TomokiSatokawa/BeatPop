using System.Collections.Generic;
using Title.Custom;

namespace InGame.Node
{
    /// <summary>
    /// 譜面をカスタムデータを元にカスタムする
    /// </summary>
    public static class ChartModifier
    {
        public static void Modify(List<NodeData> nodes, CustomChartPattern pattern)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                NodeData node = nodes[i];
                bool convertFlick = node.ConvertLevel > pattern.FlickConvertLevel;
                bool convertHold = node.ConvertLevel > pattern.LongConvertLevel;
                switch (node.PrefabType)
                {
                    case PoolPrefabType.FlickNote:
                        if (convertFlick)
                        {
                            node.PrefabType = PoolPrefabType.NormalNote;
                        }
                        break;

                    case PoolPrefabType.HoldNoteStart:
                    case PoolPrefabType.HoldNoteEnd:
                        if (convertHold)
                        {
                            node.PrefabType = PoolPrefabType.NormalNote;
                            node.Connect = -1;
                        }
                        break;

                    case PoolPrefabType.HoldFlickEnd:
                        if (convertFlick && convertHold)
                        {
                            node.PrefabType = PoolPrefabType.NormalNote;
                            break;
                        }
                        if (convertFlick)
                        {
                            node.PrefabType = PoolPrefabType.HoldNoteEnd;
                        }
                        if (convertHold)
                        {
                            node.PrefabType = PoolPrefabType.FlickNote;
                        }
                        break;
                }
                nodes[i] = node;
            }
        }
        public static void NormalizeLongNotes(List<NodeData> nodes)
        {
            var removeList = new HashSet<int>();

            Dictionary<int, float> longConnect = new();
            // LongStartごとに処理
            foreach (var start in nodes)
            {
                if(start.PrefabType == PoolPrefabType.HoldNoteStart)
                {
                    longConnect[start.Lane] = nodes[start.Connect].Time;
                    continue;
                }

                if (start.PrefabType == PoolPrefabType.HoldNoteEnd
                    || start.PrefabType == PoolPrefabType.HoldFlickEnd)
                    continue;

                bool isHold = false;

                if (longConnect.TryGetValue(start.Lane, out var connectTime))
                {
                    if(start.Time < connectTime)
                        isHold = true;
                }

                if (isHold)
                {
                    if(start.PrefabType != PoolPrefabType.TickNode)
                    {
                        removeList.Add(start.NodeID);
                    }
                }
                else
                {
                    if (start.PrefabType == PoolPrefabType.TickNode)
                    {
                        removeList.Add(start.NodeID);
                    }
                }
            }

            nodes.RemoveAll(x => removeList.Contains(x.NodeID));
            AssignNodeIds(nodes);
            ConnectHoldNotes(nodes);
        }
        public static void ConnectHoldNotes(List<NodeData> result)
        {
            // Hold接続
            for (int i = 0; i < result.Count; i++)
            {
                var startNode = result[i];

                if (startNode.PrefabType != PoolPrefabType.HoldNoteStart)
                    continue;

                for (int j = i + 1; j < result.Count; j++)
                {
                    var targetNode = result[j];

                    // 他レーンとTickは無視
                    if (targetNode.Lane != startNode.Lane
                        || targetNode.PrefabType == PoolPrefabType.TickNode)
                        continue;

                    // 同レーンの終点発見
                    if (targetNode.PrefabType == PoolPrefabType.HoldNoteEnd
                        || targetNode.PrefabType == PoolPrefabType.HoldFlickEnd)
                    {
                        startNode.Connect = targetNode.NodeID;

                        targetNode.Connect = startNode.NodeID;
                        result[j] = targetNode;

                        break;
                    }

                    // 同レーンに別ノーツがあったら接続失敗
                    break;
                }

                result[i] = startNode;
            }
        }

        public static void AssignNodeIds(List<NodeData> result)
        {
            // NodeID振り直し
            for (int i = 0; i < result.Count; i++)
            {
                var node = result[i];
                node.NodeID = i;
                result[i] = node;
            }
        }
    }
}

using System.Collections.Generic;
using Title.Custom;

namespace InGame.Node
{
    /// <summary>
    /// 譜面をカスタムデータを元にカスタムする
    /// </summary>
    public static class ChartModifier
    {
        public static void Modifier(List<NodeData> Nodes,CustomChartPattern pattern)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeData node = Nodes[i];
                switch (node.PrefabType)
                {
                    case PoolPrefabType.FlickNote:
                        if (node.ConvertLevel > pattern.FlickConvertLevel)
                        {
                            node.PrefabType = PoolPrefabType.NormalNote;
                        }
                        break;
                    case PoolPrefabType.HoldNoteStart:
                    case PoolPrefabType.HoldNoteEnd:
                        if (node.ConvertLevel > pattern.LongConvertLevel)
                        {
                            node.PrefabType = PoolPrefabType.NormalNote;
                            node.Connect = 0;
                        }
                        break;
                }
                Nodes[i] = node;
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

                if (start.PrefabType == PoolPrefabType.HoldNoteEnd)
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
        }
    }
}

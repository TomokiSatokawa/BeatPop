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
        }
    }
}

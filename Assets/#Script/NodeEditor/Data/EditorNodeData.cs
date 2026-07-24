using System;
using System.Collections.Generic;
using System.Linq;
using InGame;
using InGame.Node;
using R3;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// エディターでのノーツデータを管理
    /// </summary>
    public class EditorNodeData : EditorDataManagerBase<EditorNodeData>
    {
        private List<NodeData> _nodes = new();
        private List<float> _sectionTime = new();
        private readonly Subject<NodeData> _onRemove = new();
        private readonly ReactiveProperty<NodeSaveData> _loadedFile = new();

        public IReadOnlyList<NodeData> Nodes => _nodes;
        public IReadOnlyList<float> SectionTime => _sectionTime;
        public Observable<NodeData> OnRemove => _onRemove;
        public ReadOnlyReactiveProperty<NodeSaveData> LoadedFile => _loadedFile;

        public void AddNode(PoolPrefabType prefab, double time, int lean,int convertLevel = 0)
        {
            if (_nodes.Exists(x => Math.Abs(x.Time - time) < Epsilon && x.Lane == lean)) return;

            _nodes.Add(new NodeData()
            {
                Time = (float)time,
                Lane = lean,
                PrefabType = prefab,
                ConvertLevel = convertLevel
            });
        }

        public void DeleteNode(double time, int lean)
        {
            var targetNode = _nodes.FindIndex(x => Math.Abs(x.Time - time) < Epsilon && x.Lane == lean);
            if (targetNode == -1) return;

            _onRemove.OnNext(_nodes[targetNode]);
            _nodes.RemoveAt(targetNode);
        }

        public void AddSection(float time)
        {
            if (_sectionTime.Exists(t => Mathf.Abs(t - time) < Epsilon))
                return;

            _sectionTime.Add(time);
        }

        public void RemoveSection(float time)
        {
            int index = _sectionTime.FindIndex(t => Mathf.Abs(t - time) < Epsilon);
            if (index == -1)
                return;

            _sectionTime.RemoveAt(index);
        }

        protected override string GetSerializeJson()
        {
            List<NodeData> nodes = FinalizeNodes(_nodes);
            List<float> sectionTime = FinalizeSection(_sectionTime);
            float bpm = StageTimeController.I.BPM;
            int soundIndex = LoadedFile.CurrentValue.SoundIndex;

            return NodeDataSerializer.SerializeJson(nodes, sectionTime, bpm, soundIndex);
        }
        private List<NodeData> FinalizeNodes(List<NodeData> nodes)
        {
            List<NodeData> result = nodes.OrderBy(x => x.Time).ToList();

            AssignNodeIds(result);
            ConnectHoldNotes(result);

            return result;
        }

        private void ConnectHoldNotes(List<NodeData> result)
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
                    if (targetNode.PrefabType == PoolPrefabType.HoldNoteEnd)
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

        private void AssignNodeIds(List<NodeData> result)
        {
            // NodeID振り直し
            for (int i = 0; i < result.Count; i++)
            {
                var node = result[i];
                node.NodeID = i;
                result[i] = node;
            }
        }

        private List<float> FinalizeSection(List<float> section)
        {
            if (!section.Contains(0f))
            {
                section.Add(0f);
            }

            return section.OrderBy(x => x).ToList();
        }

        protected override async void DeserializeJson(string json)
        {
            var data = await NodeDataSerializer.AutoDeserialize(json);
            if (data == null)
            {
                Debug.LogError("ファイル読み込みエラー");
                return;
            }
            _nodes = data.Nodes;
            _sectionTime = data.Section;
            _loadedFile.OnNext(data);
        }

        protected override void CreateNewFile()
        {
            _nodes.Clear();
            _sectionTime.Clear();
            _loadedFile.Value = new()
            {
                BPM = 120f,
                SongName = "デフォルト",
            };
        }
    }
}
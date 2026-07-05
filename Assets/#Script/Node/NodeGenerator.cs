using System.Collections.Generic;
using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using InGame.UI;
using R3;
using UnityEngine;

namespace InGame.Node
{
    public class NodeGenerator : SingletonMonoBehaviour<NodeGenerator>
    {
        [SerializeField] private NodeController _nodeController;
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private Transform[] _clonePosition;
        [SerializeField] private float _goalZ;
        [SerializeField] private float _arrivalSeconds;
        [SerializeField] private bool _isGenerating;
        [SerializeField] private HoldNodeFillManager _nodeFillRenderer;

        public List<NodeData> NodeDates { get; private set; }
        private int _nextNode = 0;
        private float _nextLine = 0;
        public float ArrivalSeconds => _arrivalSeconds; 
        private int _lineIndex =0;

        private Subject<NodeSaveData> _onFileLoaded = new();
        public Observable<NodeSaveData> OnFileLoaded => _onFileLoaded;
        void Start()
        {
            LoadFile();
        }

        private async void LoadFile()
        {
            await UniTask.Yield();
            var data = await NodeDataSerializer.AutoDeserialize(SongPlayManager.I.SongData.GetNodeJson().text);
            NodeDates = data.Nodes;
            _onFileLoaded.OnNext(data);
        }

        // Update is called once per frame
        void Update()
        {
            if (NodeDates == null) return;

            // 全ノード生成済みなら終了
            if (_nextNode >= NodeDates.Count)
            {
                _isGenerating = false;
                return;
            }

            // 同時押し対応のため while
            while (_nextNode < NodeDates.Count && NodeDates[_nextNode].Time <= StageTimeController.StageTime + _arrivalSeconds)
            {
                NodeData nodeData = NodeDates[_nextNode];
                if (nodeData.Time >= StageTimeController.I.StartSectionTime)
                {
                    CreateNode(nodeData);
                }
                _nextNode++;
            }

            if (_nextLine <= StageTimeController.StageTime + _arrivalSeconds)
            {
                var nodeData1 = new NodeData();
                nodeData1.Time = _nextLine;
                nodeData1.PrefabType = PoolPrefabType.Line;
                nodeData1.Lane = 0;
                CreateNode(nodeData1);

                var nodeData2 = new NodeData();
                nodeData2.Time = _nextLine;
                nodeData2.PrefabType = PoolPrefabType.Line;
                nodeData2.Lane = 1;
                CreateNode(nodeData2);

                _lineIndex++;
                _nextLine = _lineIndex * 60f / StageTimeController.I.BPM; 
            }
        }

        private void CreateNode(NodeData nodeData)
        {
            PoolPrefabType prefabType = nodeData.PrefabType;
            if (prefabType == PoolPrefabType.HoldNoteStart || prefabType == PoolPrefabType.HoldNoteEnd)
            {
                CreateHoldNode(nodeData);
                return;
            }
            GenerateNode<NodeObject>(nodeData);
        }

        private T GenerateNode<T>(NodeData nodeData) where T : NodeObject
        {
            var newObject = PoolManager.I.Get<T>(nodeData.PrefabType);
            Vector3 startPosition = _clonePosition[nodeData.Lane].position;
            newObject.transform.position = startPosition;
            newObject.transform.rotation = Quaternion.identity;

            newObject.SetMoveData(nodeData, startPosition);
            newObject.SetGoal(_goalZ);
            _nodeController.AddNode(newObject);
            return newObject;
        }

        private void CreateHoldNode(NodeData holdNode)
        {
            var holdObject = GenerateNode<NodeObject>(holdNode);

            if (holdNode.PrefabType == PoolPrefabType.HoldNoteStart)
            {
                int endIndex = -1;

                for (int j = holdNode.NodeID + 1; j < NodeDates.Count; j++)
                {
                    var node = NodeDates[j];

                    if (node.Lane != holdNode.Lane)
                        continue;

                    if (node.PrefabType == PoolPrefabType.HoldNoteEnd)
                    {
                        endIndex = j;
                        break;
                    }
                }

                if (endIndex < 0)
                {
                    Debug.LogError($"HoldEnd is not found. StartNodeID:{holdNode.NodeID}");
                    return;
                }

                _nodeFillRenderer.AddClone(holdNode, NodeDates[endIndex], holdObject);
            }
            else if (holdNode.PrefabType == PoolPrefabType.HoldNoteEnd)
            {
                _nodeFillRenderer.SetEndObject(holdNode, holdObject);
            }
        }
    }


    [System.Serializable]
    public struct NodeData
    {
        public int NodeID;
        public int Lane;
        public float Time;
        public PoolPrefabType PrefabType;
        public int Connect;

        public NodeData(int id = -1)
        {
            NodeID = id;
            Lane = -1;
            Time = -1;
            PrefabType = PoolPrefabType.None;
            Connect = -1;
        }
    }
}

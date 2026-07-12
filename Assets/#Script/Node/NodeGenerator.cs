using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using InGame.UI;
using R3;
using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// ノーツの生成
    /// </summary>
    public class NodeGenerator : SingletonMonoBehaviour<NodeGenerator>
    {
        [SerializeField] private NodeController _nodeController;
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private Transform[] _clonePosition;
        [SerializeField] private float _goalZ;
        [SerializeField] private float _arrivalSeconds;
        [SerializeField] private bool _isGenerating = true;
        [SerializeField] private HoldNodeFillManager _nodeFillRenderer;

        public List<NodeData> NodeDates { get; private set; }
        private int _nextNode = 0;
        public float ArrivalSeconds => _arrivalSeconds;
        void Start()
        {
            InGameFileLoad.I.OnNodeFileLoaded.Subscribe(x => NodeDates = x.Nodes).AddTo(this);

            StageTimeController.I.OnInitialized.Subscribe(_ =>
            {
                BeatUpdateManager.I.AddBeatUpdate(new BeatUpdateHandle(16, -_arrivalSeconds, (_,_) => GenerateNodes()));
                BeatUpdateManager.I.AddBeatUpdate(new BeatUpdateHandle(4, -_arrivalSeconds, (time, _)=> GenerateLines(time)));
            }).AddTo(this);
        }

        void Update()
        {
            if (NodeDates == null || !_isGenerating) return;

            // 全ノード生成済みなら終了
            if (_nextNode >= NodeDates.Count)
            {
                _isGenerating = false;
                return;
            }
        }

        private void GenerateLines(float time)
        {
            float lineTime = time + _arrivalSeconds;
            for (int lane = 0; lane < _clonePosition.Length; lane++)
            {
                CreateNode(new NodeData
                {
                    Time = lineTime,
                    PrefabType = PoolPrefabType.Line,
                    Lane = lane
                });
            }
        }

        private void GenerateNodes()
        {
            double generateTime = StageTimeController.StageTime + _arrivalSeconds;
            double startSectionTime = StageTimeController.I.StartSectionTime;

            while (_nextNode < NodeDates.Count)
            {
                NodeData nodeData = NodeDates[_nextNode];

                if (nodeData.Time > generateTime)
                    return;

                if (nodeData.Time >= startSectionTime)
                {
                    CreateNode(nodeData);
                }

                _nextNode++;
            }
        }

        private void CreateNode(NodeData nodeData)
        {
            PoolPrefabType prefabType = nodeData.PrefabType;

            switch (prefabType)
            {
                case PoolPrefabType.HoldNoteStart:
                    CreateHoldStartNode(nodeData);
                    return;

                case PoolPrefabType.HoldNoteEnd:
                    CreateHoldEndNode(nodeData);
                    return;

                case PoolPrefabType.NormalNote:
                case PoolPrefabType.FlickNote:
                case PoolPrefabType.Line:
                    GenerateNode<NodeObject>(nodeData);
                    return;

                default:
                    Debug.LogError($"Unsupported PoolPrefabType: {prefabType}");
                    return;
            }
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

        private void CreateHoldStartNode(NodeData nodeData)
        {
            var holdObject = GenerateNode<NodeObject>(nodeData);

            //終了ノーツを探す
            int endIndex = -1;

            for (int j = nodeData.NodeID + 1; j < NodeDates.Count; j++)
            {
                var node = NodeDates[j];

                if (node.Lane != nodeData.Lane)
                    continue;

                if (node.PrefabType == PoolPrefabType.HoldNoteEnd)
                {
                    endIndex = j;
                    break;
                }
            }

            if (endIndex < 0)
            {
                Debug.LogError($"HoldEnd is not found. StartNodeID:{nodeData.NodeID}");
                return;
            }

            _nodeFillRenderer.AddClone(nodeData, NodeDates[endIndex], holdObject);
        }

        private void CreateHoldEndNode(NodeData nodeData)
        {
            var holdObject = GenerateNode<NodeObject>(nodeData);
            _nodeFillRenderer.SetEndObject(nodeData, holdObject);
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

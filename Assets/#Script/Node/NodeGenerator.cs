using System.Collections.Generic;
using Common.BeatUpdate;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// ノーツの生成
    /// </summary>
    public class NodeGenerator : SingletonMonoBehaviour<NodeGenerator>//TODO：StageのSOできたら消す
    {
        [SerializeField] private NodeController _nodeController;
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private Transform[] _clonePosition;
        [SerializeField] private float _goalZ;
        [SerializeField] private float _arrivalSeconds;
        [SerializeField] private HoldNodeFillManager _nodeFillRenderer;
        public float ArrivalSeconds => _arrivalSeconds;

        private List<NodeData> _nodeDataList;
        private int _nextNode = 0;

        private void Start()
        {
            InGameFileLoad.I.OnNodeFileLoaded.Skip(1).Subscribe(x => _nodeDataList = x.Nodes).AddTo(this);

            BeatUpdateManager.BeatUpdate.Subscribe(16, -_arrivalSeconds, _ => GenerateNodes());
            BeatUpdateManager.BeatUpdate.Subscribe(4, -_arrivalSeconds, x => CreateLines(x.Time));
        }

        /// <summary>
        /// 線を生成
        /// </summary>
        private void CreateLines(float time)
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

        /// <summary>
        /// ノーツ生成の指示出し
        /// </summary>
        private void GenerateNodes()
        {
            if (!StageTimeController.I.IsPlaying.CurrentValue) return;

            double generateTime = StageTimeController.StageTime + _arrivalSeconds;
            double startSectionTime = StageTimeController.I.StartSectionTime;

            while (_nextNode < _nodeDataList.Count)
            {
                NodeData nodeData = _nodeDataList[_nextNode];

                if (nodeData.Time > generateTime)
                    return;

                if (nodeData.Time >= startSectionTime)
                {
                    CreateNode(nodeData);
                }

                _nextNode++;
            }
        }

        /// <summary>
        /// ノーツ種類別に指示を出す
        /// </summary>
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
                    CreateNodeObject<NodeObject>(nodeData);
                    return;

                default:
                    Debug.LogError($"Unsupported PoolPrefabType: {prefabType}");
                    return;
            }
        }

        /// <summary>
        /// ノーツオブジェクトの生成処理
        /// </summary>
        private T CreateNodeObject<T>(NodeData nodeData) where T : NodeObject
        {
            var newObject = PoolManager.I.Get<T>(nodeData.PrefabType);
            Vector3 startPosition = _clonePosition[nodeData.Lane].position;
            newObject.transform.position = startPosition;
            newObject.transform.rotation = Quaternion.identity;

            newObject.SetMoveData(nodeData, startPosition);
            _nodeController.AddNode(newObject);
            return newObject;
        }

        /// <summary>
        /// ロングノーツ(開始)の専用処理
        /// </summary>
        private void CreateHoldStartNode(NodeData nodeData)
        {
            var holdObject = CreateNodeObject<NodeObject>(nodeData);

            int endIndex = -1;
            endIndex = nodeData.Connect;

            if (endIndex < 0 || endIndex >= _nodeDataList.Count)
            {
                Debug.LogError($"HoldEnd is not found. StartNodeID:{nodeData.NodeID}");
                return;
            }

            _nodeFillRenderer.AddClone(nodeData, _nodeDataList[endIndex], holdObject);
        }

        /// <summary>
        /// ロングノーツ(終了)の専用処理
        /// </summary>
        private void CreateHoldEndNode(NodeData nodeData)
        {
            var holdObject = CreateNodeObject<NodeObject>(nodeData);
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

using System.Collections.Generic;
using UnityEngine;

namespace InGame.Node
{
    public class NodeGenerator : MonoBehaviour
    {
        [SerializeField] private NodeController _nodeController;
        [SerializeField] private TextAsset _textAsset;
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private Transform[] _clonePosition;
        [SerializeField] private float _goalZ;
        [SerializeField] private float _arrivalSeconds;
        [SerializeField] private bool _isGenerating;

        private List<NodeData> _nodeDatas = new();
        private int _nextNode = 0;
        private float _nextLine = 0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            LoadFile();
        }

        private void LoadFile()
        {
            var data = NodeDataSerializer.AutoDeserialize(_textAsset);
            _nodeDatas = data.Nodes;
            GameManager.I.SetData(data.BPM, data.SoundIndex);
        }

        // Update is called once per frame
        void Update()
        {
            // 全ノード生成済みなら終了
            if (_nextNode >= _nodeDatas.Count)
            {
                _isGenerating = false;
                return;
            }

            // 同時押し対応のため while
            while (_nextNode < _nodeDatas.Count && _nodeDatas[_nextNode].Time <= GameManager.I.StageTime + _arrivalSeconds)
            {
                NodeData nodeData = _nodeDatas[_nextNode];

                CreateNode(nodeData);
                _nextNode++;
            }

            if (_nextLine <= GameManager.I.StageTime + _arrivalSeconds)
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

                _nextLine += 60f / GameManager.I.BPM;
            }
        }

        private void CreateNode(NodeData nodeData)
        {
            if(nodeData.PrefabType != PoolPrefabType.Line)
            {
            Debug.Log(nodeData.NodeID);
            }
            var newObject = PoolManager.I.Get<NodeObject>(nodeData.PrefabType);
            newObject.NodeType = NodeType.Normal;
            newObject.transform.position = _clonePosition[nodeData.Lane].position;
            newObject.transform.rotation = Quaternion.identity;

            newObject.SetMoveData(nodeData);
            newObject.SetGoal(_goalZ);
            _nodeController.AddNode(newObject);
        }
    }


    [System.Serializable]
    public struct NodeData
    {
        public int NodeID;
        public int Lane;
        public float Time;
        public PoolPrefabType PrefabType;
    }
}

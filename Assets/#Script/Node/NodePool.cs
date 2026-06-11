using System.Collections.Generic;
using UnityEngine;
namespace InGame.Node
{

    public class NodePool : MonoBehaviour
    {
        [SerializeField] private List<PrefabData> _prefabDatas;
        public static NodePool I;
        private Dictionary<NodeType, Stack<NodeObject>> _poolObjects =new();
        private void Awake()
        {
            if(I == null)
            {
                I = this;
            }
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private NodeObject CreateNode(NodeType type)
        {
            NodeObject prefab = null;
            foreach(var prefabData in  _prefabDatas)
            {
                if(prefabData.Type == type)
                {
                    prefab = prefabData.Prefab;
                    continue;
                }
            }
            NodeObject newNode = Instantiate(prefab);
            newNode.NodeType = type;
            return newNode;
        }
        public NodeObject GetNode(NodeType type)
        {
            NodeObject result　= null;   
            if(!_poolObjects.TryGetValue(type, out var nodes))
            {
                _poolObjects.Add(type, new());
            }
            if (_poolObjects[type].Count == 0)
            {
                result = CreateNode(type);
            }
            else
            {
                result = _poolObjects[type].Pop();
            }
            result.gameObject.SetActive(true);

            return result;
        }
        public void Releas(NodeObject nodeObject)
        {
            nodeObject.gameObject.SetActive(false);
            _poolObjects[nodeObject.NodeType].Push(nodeObject);
        }
    }
    public enum NodeType
    {
        None, Normal,
    }
    [System.Serializable]
    public class PrefabData
    {
        public NodeObject Prefab;
        public NodeType Type;
    }
}
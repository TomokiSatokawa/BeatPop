using System.Collections.Generic;
using Input;
using R3;
using Sound;
using UnityEngine;
using UnityEngine.iOS;
namespace InGame.Node
{
    public class NodeController : MonoBehaviour
    {
        [SerializeField] private NodeJudgement _nodeJudgement;
        [SerializeField] private float _nodeSpeed;
        [SerializeField] private float _goalPos;

        private List<NodeObject> _nodes = new();

        private readonly Subject<(JudgementData, int)> _showJudge = new();

        public Observable<(JudgementData, int)> ShowJudge => _showJudge;

        public void Start()
        {
            InputManager.LeftLane.Where(b => b).Subscribe(_ => ClickLane(0,PoolPrefabType.NormalNote,SESoundType.NormalTap)).AddTo(this);
            InputManager.RightLane.Where(b => b).Subscribe(_ => ClickLane(1, PoolPrefabType.NormalNote, SESoundType.NormalTap)).AddTo(this);

            InputManager.FlickLeftLane.Where(b => b && InputManager.LeftLane.CurrentValue).Subscribe(_ => ClickLane(0,PoolPrefabType.FlickNote,SESoundType.FlickTap)).AddTo(this);
            InputManager.FlickRightLane.Where(b => b && InputManager.RightLane.CurrentValue).Subscribe(_ => ClickLane(1,PoolPrefabType.FlickNote,SESoundType.FlickTap)).AddTo(this);
        }

        public void AddNode(NodeObject node)
        {
            _nodes.Add(node);
        }

        public void Update()
        {
            List<NodeObject> removeNode = new();
            foreach (NodeObject node in _nodes)
            {
                if (node.NodeData.Time <= GameManager.I.StageTime - _nodeJudgement.DeleteTime)
                {
                    removeNode.Add(node);
                }
                Vector3 position = node.transform.position;
                position.z -= node.MoveAmount * Time.deltaTime;
                node.transform.position = position;
            }

            foreach (NodeObject node in removeNode)
            {
                if (node.Type != PoolPrefabType.Line)
                {
                    var judgeData = _nodeJudgement.JudgementDifference(node.NodeData.Time - GameManager.I.StageTime);
                    _showJudge.OnNext((judgeData, node.NodeData.Lane));
                }
                PoolManager.I.Release(node);
                _nodes.Remove(node);
            }
        }

        public void ClickLane(int lane, PoolPrefabType type, SESoundType se)
        {
            NodeObject targetNode = null;
            float bestDifference = float.MaxValue;
            float nodeTime = 0;

            foreach (var node in _nodes)
            {
                if (node.NodeData.Lane != lane) continue;
                if(node.Type != type) continue;

                float difference =
                    Mathf.Abs(node.NodeData.Time - GameManager.I.StageTime);

                if (difference < bestDifference)
                {
                    bestDifference = difference;
                    targetNode = node;
                    nodeTime = node.NodeData.Time;
                }
            }

            if (targetNode == null) return;

            if (bestDifference <= _nodeJudgement.ToleranceValue)
            {
                SoundManager.I.PlaySESound(se);

                _nodes.Remove(targetNode);
                PoolManager.I.Release(targetNode);
                var judgeData = _nodeJudgement.JudgementDifference(nodeTime - GameManager.I.StageTime);
                _showJudge.OnNext((judgeData, targetNode.NodeData.Lane));
            }
        }
    }
}
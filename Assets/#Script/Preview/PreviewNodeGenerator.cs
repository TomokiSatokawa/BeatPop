using Common.BeatUpdate;
using InGame.Node;
using UnityEngine;

namespace Preview
{
    /// <summary>
    /// プレビュー専用ノーツ生成
    /// </summary>
    public class PreviewNodeGenerator : MonoBehaviour
    {
        [SerializeField] private NodeController _nodeController;
        [SerializeField] private Transform _nodeRoot;
        [SerializeField] private Transform _clonePos;
        [SerializeField] private CustomColorData _colorData;
        private IDisposableBeat _disposableBeat;
        private float _offSet;
        private Color _nodeColor;

        private void Start()
        {
            int index = _colorData.GetDefault().GetColorIndex(Title.Custom.CustomColorType.Normal);
            _nodeColor = _colorData.GetColor(index).Color;
        }

        private void GeneratorNode(int division,float time)
        {
            if (division > 1) return;

            var node = PoolManager.I.Get<NodeObject>(PoolPrefabType.NormalNote, _nodeRoot);
            node.SetColor(_nodeColor);
            node.SetNodeData(new NodeData()
            {
                Time = time + _offSet,
                PrefabType = PoolPrefabType.NormalNote,
                Lane = 0,
            },time);
            node.transform.position = _clonePos.position;
            _nodeController.AddNode(node);
        }

        public void Initialize(float offset)
        {
            _nodeController.ReleaseAll();
            _disposableBeat?.Dispose();
            _offSet = offset;
            _disposableBeat = BeatUpdateManager.BeatUpdate.Subscribe(8, offset, x => GeneratorNode(x.Division, x.Time));
        }
    }
}
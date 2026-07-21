using InGame.Node;
using InGame;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    public class EditorNode : FollowTime
    {
        [SerializeField] private Image _nodeImage;
        private NodeData _data;
        public NodeData Data
        {
            get { return _data; }
            set
            {
                _data = value;
            }
        }
        public float LeanY { get; set; }

        public override void ChangePos()
        {
            var pos = _rect.anchoredPosition;
            pos.x = (float)(Time - StageTimeController.StageTime) * EditorManager.I.Magnification;
            pos.y = LeanY;
            _rect.anchoredPosition = pos;
        }

        public void ChangeColor(Color color)
        {
            _nodeImage.color = color;
        }
    }
}
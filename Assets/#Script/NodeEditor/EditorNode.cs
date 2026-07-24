using InGame.Node;
using InGame;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Editor
{
    /// <summary>
    /// ÉmÅ[ÉcUI
    /// </summary>
    public class EditorNode : FollowTime
    {
        [SerializeField] private Image _nodeImage;
        [SerializeField] private TextMeshProUGUI _convertText;

        public NodeData Data;
        public float LeanY;

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

        public void SetConvertLevel(int  level)
        {
            if(_convertText != null)
            {
            _convertText.text = level.ToString();
            }
        }
    }
}
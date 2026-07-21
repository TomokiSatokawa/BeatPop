using System;
using InGame.Stage;
using InGame;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    /// <summary>
    /// ライトパターンのノーツUI
    /// </summary>
    public class EditorLightNode : FollowTime
    {
        [SerializeField] private Image _nodeImage;
        [SerializeField] private Button _button;
        public LightPatternBaseData PatternBaseData { get; private set; }
        public RectTransform LeanRect { get; private set; }
        public RectTransform Content { get; private set; }

        private Action<LightPatternBaseData> _onClickAction;
        private float _leanRectPosY;

        public void SetData(LightPatternBaseData data, RectTransform lean, RectTransform content, Action<LightPatternBaseData> onClick)
        {
            PatternBaseData = data;
            LeanRect = lean;
            Content = content;
            _onClickAction = onClick;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnClick);

            Vector3 worldPos = LeanRect.TransformPoint(LeanRect.rect.center);
            Vector3 localPos = Content.InverseTransformPoint(worldPos);
            _leanRectPosY = localPos.y;
        }

        private void OnClick()
        {
            _onClickAction?.Invoke(PatternBaseData);
        }

        public void UpdatePatternData(LightPatternBaseData data)
        {
            PatternBaseData = data;
            _onClickAction?.Invoke(PatternBaseData);
        }

        public override void ChangePos()
        {
            var pos = _rect.anchoredPosition;
            pos.x = (float)(Time - StageTimeController.StageTime) * EditorManager.I.Magnification;

            pos.y = _leanRectPosY;
            _rect.anchoredPosition = pos;
        }

        public void ChangeColor(Color color)
        {
            _nodeImage.color = color;
        }
    }
}
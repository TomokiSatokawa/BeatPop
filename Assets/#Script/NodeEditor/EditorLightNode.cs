using System;
using InGame.Stage;
using InGame.UI;
using UnityEngine;
using UnityEngine.UI;

public class EditorLightNode : FollowTime
{
    [SerializeField] private Image _nodeImage;
    [SerializeField] private Button _button;
    public LightPatternBaseData PatternBaseData { get; private set; }
    public RectTransform LeanRect { get; private set; }
    public RectTransform Content { get; private set; }

    public void SetData(LightPatternBaseData data, RectTransform lean,RectTransform content,Action<LightPatternBaseData> onClick)
    {
        PatternBaseData = data;
        LeanRect = lean;
        Content = content;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => onClick?.Invoke(data));
    }

    public override void ChangePos()
    {
        var pos = _rect.anchoredPosition;
        pos.x = (float)(Time - StageTimeController.StageTime) * EditorManager.I.Magnification;

        Vector3 worldPos = LeanRect.TransformPoint(LeanRect.rect.center);
        Vector3 localPos = Content.InverseTransformPoint(worldPos);
        pos.y = localPos.y;
        _rect.anchoredPosition = pos;
    }

    public void ChangeColor(Color color)
    {
        _nodeImage.color = color;
    }
}
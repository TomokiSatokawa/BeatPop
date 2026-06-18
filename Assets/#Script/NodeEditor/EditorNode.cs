using InGame.Node;
using UnityEngine;
using UnityEngine.UI;

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
            ChangePos(EditorManager.I.EditorTime.CurrentValue);
        }
    } 
    public float LeanY { get; set; }
    
    public override void ChangePos(double time)
    {
        var pos = _rect.anchoredPosition;
        pos.x = (float)(Time - time) * EditorManager.I.Magnification;
        pos.y = LeanY;
        _rect.anchoredPosition = pos;
    }

    public void ChangeColor(Color color)
    {
        _nodeImage.color = color;
    }
}

using InGame.UI;
using UnityEngine;

public class FollowTime : PoolObject
{
    [SerializeField] protected RectTransform _rect;
    public RectTransform Rect => _rect;
    public double Time;
    public void Start()
    {
        _rect.localScale = Vector3.one;
    }
    public virtual void Update()
    {
        ChangePos();   
    }
    public virtual void ChangePos()
    {
        var pos = _rect.anchoredPosition;
        pos.x = (float)(Time - StageTimeController.StageTime ) * EditorManager.I.Magnification;
        _rect.anchoredPosition = pos;
    }
    public void SetPos(Vector3 pos)
    {
        _rect.anchoredPosition = pos;
    }
    public void SetWidth(float width)
    {
        Vector2 size = _rect.sizeDelta;
        size.x = width;
        _rect.sizeDelta = size;
    }
}

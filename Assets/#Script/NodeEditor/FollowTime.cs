using R3;
using UnityEngine;

public class FollowTime : PoolObject
{
    [SerializeField] protected RectTransform _rect;
    public RectTransform Rect => _rect;
    public double Time;
    public void Start()
    {
        _rect.localScale = Vector3.one;
        EditorManager.I.EditorTime.Subscribe(t => ChangePos(t)).AddTo(this);
    }
    public virtual void ChangePos(double time)
    {
        var pos = _rect.anchoredPosition;
        pos.x = (float)(Time - time ) * EditorManager.I.Magnification;
        _rect.anchoredPosition = pos;
    }
    public void SetPos(Vector3 pos)
    {
        _rect.anchoredPosition = pos;
    }
    public void Setwidth(float width)
    {
        Vector2 size = _rect.sizeDelta;
        size.x = width;
        _rect.sizeDelta = size;
    }
}

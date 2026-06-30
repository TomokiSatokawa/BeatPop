using DG.Tweening;
using UnityEngine;

public class LaneTapEffect : PoolObject
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _startColor;
    [SerializeField] private float _endPosZ;
    [SerializeField] private float _duration;

    public void OnEnable()
    {
        _spriteRenderer.color = _startColor;
        _spriteRenderer.DOFade(0, _duration);
        this.transform.DOMoveZ(_endPosZ, _duration);
        //DOVirtual.DelayedCall(_duration, () => PoolManager.I.Release(this));
        DOVirtual.DelayedCall(_duration, () => Destroy(this.gameObject));
    }
}

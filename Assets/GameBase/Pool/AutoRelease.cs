using UnityEngine;

public class AutoRelease : PoolObject
{
    [SerializeField] private float _duration;
    private float _timer;

    public override void OnPoolActive()
    {
        base.OnPoolActive();
        _timer = _duration;
    }
    private void Update()
    {
        if (!IsPoolActive) return;
        _timer -= Time.deltaTime;
        
        if( _timer <= 0)
        {
            PoolManager.I.Release(this);
        }
    }
}

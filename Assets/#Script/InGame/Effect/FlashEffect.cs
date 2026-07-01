using DG.Tweening;
using UnityEngine;

public class FlashEffect : PoolObject
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _emissionAmount;
    [SerializeField] private float _fadeInDuration;
    [SerializeField] private float _fadeOutDuration;
    [SerializeField] private Vector3 _maxScale;
    [SerializeField] private Vector3 _minScale;

    private MaterialPropertyBlock _mpb;
    private Sequence _seq;

    private int _baseColorId;
    private int _emissionId;

    private Color _themeColor;

    private void Awake()
    {
        _mpb = new MaterialPropertyBlock();

        _baseColorId = Shader.PropertyToID("_BaseColor");
        _emissionId = Shader.PropertyToID("_EmissionColor");
    }

    private void OnDisable()
    {
        _seq?.Kill();
        ResetVisual();
    }

    public void SetColor(Color color)
    {
        _themeColor = color;
    }
    public void PlayFlash()
    {
        _seq?.Kill();

        float intensity = 0f;

        transform.localScale = _maxScale;

        _seq = DOTween.Sequence();

        // Fade In + Scale In
        _seq.Append(DOTween.To(() => intensity, x =>
        {
            intensity = x;
            ApplyColor(intensity);
        }, _emissionAmount, _fadeInDuration)
        .SetEase(Ease.OutQuad));


        // Fade Out
        _seq.Append(DOTween.To(() => intensity, x =>
        {
            intensity = x;
            ApplyColor(intensity);
        }, 0f, _fadeOutDuration)
        .SetEase(Ease.InQuad));

        _seq.Join(transform.DOScale(_minScale, _fadeOutDuration)
            .SetEase(Ease.InQuad));

        _seq.AppendCallback(() =>
        {
            ResetVisual();
            transform.localScale = Vector3.one;
            Release();
        });
    }

    private void ApplyColor(float intensity)
    {
        _renderer.GetPropertyBlock(_mpb);

        Color emission = _themeColor * intensity;

        _mpb.SetColor(_baseColorId, _themeColor);
        _mpb.SetColor(_emissionId, emission);

        _renderer.SetPropertyBlock(_mpb);
    }

    private void ResetVisual()
    {
        _renderer.GetPropertyBlock(_mpb);

        _mpb.SetColor(_baseColorId, Color.clear);
        _mpb.SetColor(_emissionId, Color.black);

        _renderer.SetPropertyBlock(_mpb);
    }
}
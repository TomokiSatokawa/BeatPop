using DG.Tweening;
using UnityEngine;

namespace InGame.Stage
{
    public class PanelLightControl : LightControlBase
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private float _maxPower;
        [SerializeField] private float _minPower;
        [SerializeField] private float _outThreshold;
        [SerializeField] private float _outPower;

        private float _power;
        private Color _color;
        private Tween _flash;

        private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
        private MaterialPropertyBlock _mpb;
        private void Awake()
        {
            _mpb = new MaterialPropertyBlock();
        }
        public void Start()
        {
            SetColor(Color.black);
            UpdatePower(_minPower);
        }
        public override void SetColor(Color color)
        {
            _renderer.material.color = color;
            _color = color;
            UpdatePower(_power);
        }

        public void UpdatePower(float intensity)
        {
            if (intensity > _outThreshold)
            {
                _power = intensity;
            }
            else
            {
                _power = _outPower;
            }
            _renderer.GetPropertyBlock(_mpb);

            // HDRカラー
            _mpb.SetColor(EmissionColorID, _color * _power);

            _renderer.SetPropertyBlock(_mpb);
        }

        public override void Flash(float duration, float power)
        {
            _flash?.Kill(true);
            _flash = DOVirtual.Float(_maxPower * power, _minPower, duration, x =>
            {
                UpdatePower(x);
            });
        }

        public override void Refresh()
        {
            _flash?.Kill(true);
        }

        public override void SetPower(float power)
        {
            UpdatePower(_maxPower * power);
        }
    }
}
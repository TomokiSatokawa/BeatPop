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
        private Sequence _flash;
        private MaterialPropertyBlock _mpb;

        private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            _mpb = new MaterialPropertyBlock();
        }

        public void Start()
        {
            initialize();
        }

        private void initialize()
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

        public void UpdatePower(float power)
        {
            if (power > _outThreshold)
            {
                _power = power;
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
            _flash = DOTween.Sequence()
                .Append(DoLightPower(_maxPower * power, _minPower, duration));
        }
        public override void Wave(float duration, float power)
        {
            _flash?.Kill(true);

            _flash = DOTween.Sequence()
                 .Append(DoLightPower(_minPower, _maxPower * power, duration))
                 .Append(DoLightPower(_maxPower * power,_minPower, duration));
        }

        private Tweener DoLightPower(float start,float end ,float duration)
        {
            return DOVirtual.Float(start, end, duration, x =>
            {
                UpdatePower(x);
            });
        }

        public override void Refresh()
        {
            _flash?.Kill();
            _flash = null;
            UpdatePower(_minPower);
        }

        public override void SetPower(float power)
        {
            UpdatePower(_maxPower * power);
        }
    }
}
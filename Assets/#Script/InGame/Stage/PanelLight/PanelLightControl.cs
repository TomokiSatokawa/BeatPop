using DG.Tweening;
using UnityEngine;

namespace InGame.Stage
{
    /// <summary>
    /// 光るだけのライト
    /// </summary>
    public class PanelLightControl : StageLightBase
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private float _maxPower;
        [SerializeField] private float _minPower;
        [SerializeField] private float _outThreshold;
        [SerializeField] private float _outPower;
        [Header("MoveAnimation")]
        [SerializeField] private float _angle;
        [SerializeField] private float _randomDray;
        [SerializeField] private float _randomAngle;
        [SerializeField,Range(0,1)] private float _durationDividend;

        private Sequence _moveAnimation;
        private float _power;
        private Color _color;
        private Sequence _flash;
        private MaterialPropertyBlock _mpb;

        private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
        private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            _mpb = new MaterialPropertyBlock();
            Initialize();

            
        }

        private void Initialize()
        {
            SetColor(Color.black);
            UpdatePower(_minPower);
        }

        public override void SetColor(Color color)
        {
            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(BaseColorID, color);
            _renderer.SetPropertyBlock(_mpb);

            _color = color;
            UpdatePower(_power);
        }

        public void UpdatePower(float power)
        {
            //Threshold以下ならOutPower
            _power = power > _outThreshold ? power : _outPower;

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

            _moveAnimation?.Kill(true);
            _moveAnimation = DOTween.Sequence();
            Vector3 angle = new Vector3(_angle, 0, 0);
            angle.x += Random.Range(-_randomAngle, _randomAngle);
            angle.y += Random.Range(-_randomAngle, _randomAngle);
            angle.z += Random.Range(-_randomAngle, _randomAngle);
            _moveAnimation.Append(transform.DORotate(angle, duration * _durationDividend).SetDelay(Random.Range(0,_randomDray)));
            _moveAnimation.Append(transform.DORotate(-angle, duration * (1 - _durationDividend)));
        }
        public void Wave(float duration, float power)
        {
            _flash?.Kill(true);

            _flash = DOTween.Sequence()
                 .Append(DoLightPower(_minPower, _maxPower * power, duration))
                 .Append(DoLightPower(_maxPower * power, _minPower, duration));
        }

        public override void Refresh()
        {
            _flash?.Kill(true);
            _flash = null;
            UpdatePower(_minPower);
        }

        public override void SetPower(float power)
        {
            UpdatePower(_maxPower * power);
        }

        private Tweener DoLightPower(float start, float end, float duration)
        {
            return DOVirtual.Float(start, end, duration, x =>
            {
                UpdatePower(x);
            });
        }
    }
}
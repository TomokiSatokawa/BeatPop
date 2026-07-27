using Common.Effect;
using DG.Tweening;
using UnityEngine;

namespace InGame.Stage
{
    /// <summary>
    /// ‰Î‰Š•úË‰‰o
    /// </summary>
    public class FireLightControl : StageLightBase
    {
        [SerializeField] private ParticleSystemController _particleSystemController;
        private void Start()
        {
            _particleSystemController.SetEmission(false);
        }
        private Tween _tween;
        public override void Flash(float duration, float power)
        {
            _tween?.Kill();
            _particleSystemController.Play();
            _particleSystemController.SetEmission(true);
            _tween = DOVirtual.DelayedCall(duration, () => _particleSystemController.SetEmission(false));
        }

        public override void Refresh() { }

        public override void SetColor(Color color) { }

        public override void SetPower(float power) { }
    }

}
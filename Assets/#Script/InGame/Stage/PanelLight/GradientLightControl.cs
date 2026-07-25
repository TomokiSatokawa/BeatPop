using UnityEngine;

namespace InGame.Stage
{
    /// <summary>
    /// 動くグラデーションライト
    /// </summary>
    public class GradientLightControl :  StageLightBase
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _startColor1 = Color.black;
        [SerializeField] private Color _startColor2 = Color.gray;
        [SerializeField] private float _startPower = 1f;

        private MaterialPropertyBlock _mpb;

        private static readonly int SpeedID = Shader.PropertyToID("_Speed");
        private static readonly int Color1 = Shader.PropertyToID("_Color1");
        private static readonly int Color2 = Shader.PropertyToID("_Color2");
        private static readonly int Amount = Shader.PropertyToID("_Amount");

        private void Awake()
        {
            _mpb = new MaterialPropertyBlock();
            SetColor(_startColor1, _startColor2);
            SetPower(_startPower);
        }

        public override void Flash(float duration, float power)
        {
            _renderer.GetPropertyBlock(_mpb);

            _mpb.SetFloat(SpeedID, duration);
            _mpb.SetFloat(Amount, power);

            _renderer.SetPropertyBlock(_mpb);
        }

        public override void SetColor(Color color)
        {
            SetColor(color, color);
        }

        public void SetColor(Color color1, Color color2)
        {
            _renderer.GetPropertyBlock(_mpb);

            //グラデーションカラー
            _mpb.SetColor(Color1, color1);
            _mpb.SetColor(Color2, color2);

            _renderer.SetPropertyBlock(_mpb);
        }

        public override void SetPower(float power)
        {
            _renderer.GetPropertyBlock(_mpb);

            _mpb.SetFloat(Amount, power);

            _renderer.SetPropertyBlock(_mpb);
        }

        public override void Refresh() { }
    }
}
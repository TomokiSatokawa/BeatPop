using UnityEngine;

namespace InGame.Stage
{
    public class GradientLightControl : LightControlBase
    {
        [SerializeField] private Renderer _renderer;


        private MaterialPropertyBlock _mpb;

        private static readonly int SpeedID = Shader.PropertyToID("_Speed");
        private static readonly int Color1 = Shader.PropertyToID("_Color1");
        private static readonly int Color2 = Shader.PropertyToID("_Color2");
        private static readonly int Amount = Shader.PropertyToID("_Amount");

        private void Start()
        {
            _mpb = new();
        }
        public override void Flash(float duration, float power)
        {
            _renderer.GetPropertyBlock(_mpb);

            _mpb.SetFloat(SpeedID, duration);
            _mpb.SetFloat(Amount, power);

            _renderer.SetPropertyBlock(_mpb);
        }

        public override void Refresh()
        {

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

        public override void Wave(float duration, float power)
        {
            Flash(duration, power);
        }
    }
}
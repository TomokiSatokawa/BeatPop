using UnityEngine;

namespace InGame.Stage
{
    /// <summary>
    /// グラデーションライトパターン
    /// </summary>
    public class GradationLightPattern : LightPatternBase<GradationLightPatternData>
    {
        public override void InitializeCore(GradationLightPatternData data, StageLightBase[] lights)
        {
            base.InitializeCore(data, lights);

            foreach (var light in lights)
            {
                if (light is not GradientLightControl gradientLight) continue;

                gradientLight.SetColor(data.MainColor.GetColor(), data.SubColor.GetColor());

                gradientLight.Flash(data.Duration, data.Power);
            }
        }

        // 初期化時のみ設定するため更新不要
        public override void BeatUpdate(int division) { }
    }
    public class GradationLightPatternData : LightPatternBaseData
    {
        public ColorData SubColor = new ColorData(Color.green);

    }
}

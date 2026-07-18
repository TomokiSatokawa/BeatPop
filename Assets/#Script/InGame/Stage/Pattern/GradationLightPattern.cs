using InGame.Stage;
using UnityEngine;

public class GradationLightPattern : LightPatternBase<GradationLightPatternData>
{
    public override void InitializeCore(GradationLightPatternData data, LightControlBase[] lights)
    {
        base.InitializeCore(data, lights);

        foreach (var light in lights)
        {
            if (light is not GradientLightControl gradientLight) continue;

            gradientLight.SetColor(data.MainColor.GetColor(), data.SubColor.GetColor());

            gradientLight.Flash(data.Duration, data.Power);
        }
    }
    public override void BeatUpdate(int division){ }
}
public class GradationLightPatternData: LightPatternBaseData
{
    public ColorData SubColor = new ColorData(Color.green);

}

namespace InGame.Stage
{

    public class SingleLightPattern : LightPatternBase<LightPatternBaseData>
    {
        public override void Initialize(LightPatternBaseData data, StageLightBase[] lights)
        {
            base.InitializeCore(data, lights);
            foreach (var light in lights)
            {
                light.Flash(data.Duration, data.Power);
            }
        }
        public override void BeatUpdate(int division) { }
    }
}

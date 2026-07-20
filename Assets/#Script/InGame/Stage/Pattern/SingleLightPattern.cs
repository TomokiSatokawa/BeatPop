namespace InGame.Stage
{
    /// <summary>
    /// 1”­‚Ì‚ÝŒõ‚ç‚¹‚é
    /// </summary>
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

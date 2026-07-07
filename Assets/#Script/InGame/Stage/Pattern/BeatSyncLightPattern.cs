namespace InGame.Stage
{
    public class BeatSyncLightPattern : LightPatternBase<LightPatternBaseData>
    {
        public BeatSyncLightPattern(LightPatternBaseData data, LightControlBase[] lights)
            : base(data, lights) { }

        public override void BeatUpdate(int division)
        {
            if (division > _data.Division) return;
            foreach (var light in _lights)
            {
                light.Flash(_data.Duration, _data.Power);
            }
        }
    }
}
namespace InGame.Stage
{
    public class BeatSyncLightPattern : LightPatternBase<LightPatternBaseData>
    {
        public override void BeatUpdate(int division)
        {
            if (division > Data.Division) return;

            foreach (var light in _lights)
            {
                light.Flash(Data.Duration, Data.Power);
            }
        }
    }
}
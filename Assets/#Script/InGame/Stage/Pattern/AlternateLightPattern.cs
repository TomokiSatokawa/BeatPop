namespace InGame.Stage
{

    public class AlternateLightPattern : LightPatternBase<LightPatternBaseData>
    {
        private int count = 0;

        public AlternateLightPattern(LightPatternBaseData data, LightControlBase[] lights)
            : base(data, lights) { }


        public override void BeatUpdate(int division)
        {
            if (division > _data.Division) return;

            for (int i = 0; i < _lights.Length; i++)
            {
                if ((i % 2 == 0) == (count % 2 == 0))
                {
                    _lights[i].Flash(_data.Duration, _data.Power);
                }
            }
            count++;
        }
    }
}
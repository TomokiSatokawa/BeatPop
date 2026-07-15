namespace InGame.Stage
{

    public class WaveLightPattern : LightPatternBase<WaveLightPatternData>
    {
        public override void BeatUpdate(int division)
        {
            throw new System.NotImplementedException();
        }
    }
    public class WaveLightPatternData : LightPatternBaseData
    {
        public int A;
    }
}

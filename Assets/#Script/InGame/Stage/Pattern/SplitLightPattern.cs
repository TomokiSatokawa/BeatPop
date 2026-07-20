namespace InGame.Stage
{
    /// <summary>
    /// 左右交互パターン
    /// </summary>
    public class SplitLightPattern : LightPatternBase<LightPatternBaseData>
    {
        private int _flashCount = 0;

        public override void BeatUpdate(int division)
        {
            if (division > Data.Division) return;

            for (int i = 0; i < _lights.Length; i++)
            {
                if ((i > _lights.Length /2) == (_flashCount % 2 == 0))
                {
                    _lights[i].Flash(Data.Duration, Data.Power);
                }
            }
            _flashCount++;
        }
    }
}
namespace InGame.Stage
{
    /// <summary>
    /// ˆê’è‚Ì–¾‚é‚³‚ğˆÛ‚·‚é
    /// </summary>
    public class PersistentLightPattern : LightPatternBase<LightPatternBaseData>
    {
        private bool _isPowerApplied;
        public override void InitializeCore(LightPatternBaseData data, StageLightBase[] lights)
        {
            base.InitializeCore(data, lights);

            _isPowerApplied = false;
        }
        public override void BeatUpdate(int division)
        {
            if (_isPowerApplied || division > Data.Division) return;

            foreach (var light in _lights)
            {
                light.SetPower(Data.Power);
            }
            _isPowerApplied = true;
        }
    }
}
using InGame.Stage;

public class ManualLightPattern : LightPatternBase<LightPatternBaseData>
{
    private bool _isSetPower;
    public override void InitializeCore(LightPatternBaseData data, StageLightBase[] lights)
    {
        base.InitializeCore(data, lights);

        _isSetPower = false;
    }
    public override void BeatUpdate(int division)
    {
        if (_isSetPower) return;
        if (division > Data.Division) return;

        foreach (var light in _lights)
        {
            light.SetPower(Data.Power);
        }
        _isSetPower = true;
    }
}

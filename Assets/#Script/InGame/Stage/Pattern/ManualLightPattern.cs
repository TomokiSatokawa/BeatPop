using InGame.Stage;

public class ManualLightPattern : LightPatternBase<LightPatternBaseData>
{
    private bool _isSetPower;
    public override void Initialize(LightPatternBaseData data, LightControlBase[] lights)
    {
        base.Initialize(data, lights);

        _isSetPower = false;
    }
    public override void BeatUpdate(int division)
    {
        if (_isSetPower) return;
        if (division > _data.Division) return;

        foreach (var light in _lights)
        {
            light.SetPower(_data.Power);
        }
        _isSetPower = true;
    }
}

using UnityEngine;
namespace InGame.Stage { 
public abstract class LightPatternBase<T> where T : LightPatternBaseData
{
    protected T _data;
    protected LightControlBase[] _lights;
    public LightPatternBase(T data, LightControlBase[] lights)
    {
        _data = data;
        _lights = lights;

        foreach (var light in _lights)
        {
            light.SetColor(data.Color);
        }
    }
    public abstract void BeatUpdate(int division);
}
[System.Serializable]
public class LightPatternBaseData
{
    public int Division;
    public float Duration;
    public float Power;
    public Color Color;
}
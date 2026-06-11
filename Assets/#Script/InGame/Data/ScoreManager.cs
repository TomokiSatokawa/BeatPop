using R3;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    private ReactiveProperty<int> _combo = new();
    public ReadOnlyReactiveProperty<int> Combo => _combo;
    public void AddScore(JudgementData judgement)
    {
        _combo.Value++;
    }
    public void AddMiss()
    {
        _combo.Value = 0;
    }
}
using System.Collections.Generic;
using InGame.Node;
using R3;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    private ReactiveProperty<int> _combo = new();
    public ReadOnlyReactiveProperty<int> Combo => _combo;
    public bool IsAllPerfect { get; private set; }
    private Dictionary<IReadOnlyJudgementData, int> _judgeCount = new();
    public IReadOnlyDictionary<IReadOnlyJudgementData,int> JudgeData => _judgeCount;
    public Dictionary<PoolPrefabType, AverageData> _nodeAverage = new();
    public int MaxScore { get; private set; } 
    private float _sumDifference = 0;
    private void Start()
    {
        _sumDifference = 0;
        IsAllPerfect = true;
        DontDestroyOnLoad(this.gameObject);
    }
    public IReadOnlyJudgementData AddScore(PoolPrefabType type, float difference, NodeData nodeData)
    {
        var judgement = JudgementManager.I.JudgementDifference(type, difference);
        MaxScore += JudgementManager.I.JudgementDifference(type, 0).Score;
        if (!judgement.IsAllPerfectContinued)
            IsAllPerfect = false;

        if (judgement.IsComboContinued)
        {
            _sumDifference += difference;         
            _combo.Value++;
        }
        else
        {
            _combo.Value = 0;
        }
        AddJudgeCount(judgement);
        AddNodeCount(nodeData.PrefabType, judgement.IsComboContinued);
        return judgement;
    }
    public IReadOnlyJudgementData AddHoldScore(PoolPrefabType type, float difference)
    {
        var judgementData = JudgementManager.I.JudgementDifference(type, difference);
        MaxScore += JudgementManager.I.JudgementDifference(type, 0).Score;
        AddJudgeCount(judgementData);

        AddNodeCount(PoolPrefabType.HoldNoteFill, judgementData.IsComboContinued);
        if (judgementData.IsComboContinued)
        {
            _combo.Value++;
        }
        else
        {
            _combo.Value = 0;
            IsAllPerfect = false;
        }
        return judgementData;
    }
    public void GetSumScore(out int score)
    {
        score = 0;
        foreach(var kv in _judgeCount)
        {
            score += kv.Key.Score * kv.Value;
        }
    }
    private void AddJudgeCount(IReadOnlyJudgementData judgement)
    {
        if (!_judgeCount.ContainsKey(judgement))
        {
            _judgeCount.Add(judgement, 0);
        }
        _judgeCount[judgement]++;
    }
    private void AddNodeCount(PoolPrefabType type,bool isHit)
    {
        if (!_nodeAverage.ContainsKey(type))
        {
            _nodeAverage.Add(type, new());
        }

        if(isHit)
        {
            _nodeAverage[type].SuccessCount++;
        }

        _nodeAverage[type].TotalCount++;

    }
    public void Delete()
    {
        Destroy(gameObject);
    }
    public class AverageData
    {
        public int TotalCount;
        public int SuccessCount;
        public AverageData()
        {
            TotalCount = 0;
            SuccessCount = 0;
        }
    }
}
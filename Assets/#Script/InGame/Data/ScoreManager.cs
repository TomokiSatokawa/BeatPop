using System.Collections.Generic;
using InGame.Node;
using R3;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    private ReactiveProperty<int> _combo = new();
    public ReadOnlyReactiveProperty<int> Combo => _combo;
    public bool IsAllPerfect { get; private set; }
    public Dictionary<IReadOnlyJudgementData, int> _judgeCount = new();
    public Dictionary<PoolPrefabType, AverageData> _nodeAverage = new();
    private float _sumDifference = 0;
    private void Start()
    {
        _sumDifference = 0;
        IsAllPerfect = true;
        DontDestroyOnLoad(this.gameObject);
    }
    public void AddScore(IReadOnlyJudgementData judgement,NodeData nodeData,float difference)
    {
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
    }
    public void AddHoldScore(IReadOnlyJudgementData judgementData)
    {
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
using System.Collections.Generic;
using System.Diagnostics;
using InGame.Node;
using R3;
using UnityEngine;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    /// <summary>オールパーフェクトが続いているか</summary>
    public bool IsAllPerfect { get; private set; }
    /// <summary>最高スコア</summary>
    public int MaxPossibleScore { get; private set; }

    private ReactiveProperty<int> _combo = new();

    /// <summary>コンボ数</summary>
    public ReadOnlyReactiveProperty<int> Combo => _combo;
    private ReactiveProperty<int> _score = new();

    /// <summary>コンボ数</summary>
    public ReadOnlyReactiveProperty<int> Score => _score;

    private Dictionary<IReadOnlyJudgementData, int> _judgeDataCount = new();

    /// <summary>判定別個数</summary>
    public IReadOnlyDictionary<IReadOnlyJudgementData, int> JudgeDataCount => _judgeDataCount;

    /// <summary>ノード別ヒット数（打率）</summary>
    private Dictionary<PoolPrefabType, HitData> _nodeHitCount = new();

    /// <summary>Fast / Late の数値の合計</summary>
    private float _sumTimingOffset = 0;

    private void Start()
    {
        Initialize();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Initialize()
    {
        _combo.Value = 0;
        _sumTimingOffset = 0;
        MaxPossibleScore = 0;
        IsAllPerfect = true;

        _judgeDataCount.Clear();
        _nodeHitCount.Clear();
    }

    /// <summary>
    /// スコアを加算する
    /// </summary>
    public IReadOnlyJudgementData AddScore(PoolPrefabType type, float timing, NodeData nodeData)
    {
        var judgement = GetJudgement(type, timing);

        if (judgement.IsComboContinued)
            _sumTimingOffset += timing;

        UpdateJudgeState(judgement);
        AddJudgeCount(judgement);
        AddNodeHitCount(nodeData.PrefabType, judgement.IsComboContinued);
        return judgement;
    }

    /// <summary>
    /// ホールドノード専用スコア加算
    /// </summary>
    public IReadOnlyJudgementData AddHoldScore(PoolPrefabType type, float timing)
    {
        var judgement = GetJudgement(type, timing);

        UpdateJudgeState(judgement);
        AddJudgeCount(judgement);
        AddNodeHitCount(PoolPrefabType.HoldNoteFill, judgement.IsComboContinued);
        return judgement;
    }

    public void GetSumScore(out int score)
    {
        score = 0;
        foreach (var kv in _judgeDataCount)
        {
            score += kv.Key.Score * kv.Value;
        }
    }

    public void Release()
    {
        Destroy(gameObject);
    }

    private void UpdateJudgeState(IReadOnlyJudgementData judgement)
    {
        if (!judgement.IsAllPerfectContinued)
            IsAllPerfect = false;

        if (judgement.IsComboContinued)
        {
            _combo.Value++;
        }
        else
        {
            _combo.OnNext(0);
        }
    }

    private IReadOnlyJudgementData GetJudgement(PoolPrefabType type, float timing)
    {
        var judgement = JudgementManager.I.GetJudgement(type, timing);
        MaxPossibleScore += JudgementManager.I.GetJudgement(type, 0).Score;
        return judgement;
    }


    private void AddJudgeCount(IReadOnlyJudgementData judgement)
    {
        if (!_judgeDataCount.TryGetValue(judgement, out var count))
        {
            count = 0;
        }

        _judgeDataCount[judgement] = count + 1;

        _score.Value += judgement.Score;
    }

    private void AddNodeHitCount(PoolPrefabType type, bool isHit)
    {
        if (!_nodeHitCount.ContainsKey(type))
        {
            _nodeHitCount.Add(type, new());
        }

        if (isHit)
        {
            _nodeHitCount[type].AddHit();
        }
        else
        {
            _nodeHitCount[type].AddMiss();
        }
    }


    public class HitData
    {
        public int TotalCount { get; private set; }
        public int HitCount { get; private set; }

        public HitData()
        {
            HitCount = 0;
            TotalCount = 0;
        }

        public void AddHit()
        {
            HitCount++;
            TotalCount++;
        }

        public void AddMiss()
        {
            TotalCount++;
        }
    }
}
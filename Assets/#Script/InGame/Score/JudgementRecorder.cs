using System.Collections.Generic;

namespace InGame.Score
{
    /// <summary>
    /// ”»’è‚ğ‚·‚×‚Ä•Û‘¶‚·‚é
    /// </summary>
    public class JudgementRecorder : IReadOnlyJudgementRecorder
    {
        private readonly Dictionary<IReadOnlyJudgementData, int> _judgeDataCount = new();

        /// <summary>”»’è•ÊŒÂ”</summary>
        public IReadOnlyDictionary<IReadOnlyJudgementData, int> JudgeDataCount => _judgeDataCount;

        public void Initialize()
        {
            _judgeDataCount.Clear();
        }

        public void AddJudgeCount(IReadOnlyJudgementData judgement)
        {
            _judgeDataCount[judgement] = _judgeDataCount.GetValueOrDefault(judgement) + 1;
        }
    }
    public interface IReadOnlyJudgementRecorder
    {
        public IReadOnlyDictionary<IReadOnlyJudgementData, int> JudgeDataCount { get; }
    }
}

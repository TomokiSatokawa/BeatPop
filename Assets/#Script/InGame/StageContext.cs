using Common.PlaySystem;
using InGame.Node;
using Title.Custom;
using Title.SongSelect;
using UnityEngine;

namespace InGame
{
    /// <summary>
    ///  ステージに関する各種設定を提供する
    /// </summary>
    public class StageConfig : SingletonPersistent<StageConfig>
    {
        [SerializeField] private StageLayoutData _stageLayoutData;
        [SerializeField] private JudgementTable _judgementTable;
        [SerializeField] private ExperienceDatabase _experienceDatabase;
        [SerializeField] private RankDataBase _rankDataBase;
        [SerializeField] private CustomStageData _customStageData;
        [SerializeField] private int _longNoteDivisionInterval = 8;
        [SerializeField] private Transform[] _clonePos;

        public StageLayoutData StageLayout => _stageLayoutData;
        public JudgementTable JudgementTable => _judgementTable;
        public ExperienceDatabase ExperienceDatabase => _experienceDatabase;
        public RankDataBase RankDataBase => _rankDataBase;
        public int LongNoteDivisionInterval => _longNoteDivisionInterval;
        public float ArrivalSeconds => _arrivalSeconds;

        private float _arrivalSeconds;

        protected override void OnAwake()
        {
            if (SongPlayContext.I != null)
                _arrivalSeconds = _customStageData.GetSpeedSecond(SongPlayContext.I.PatternData.SpeedPattern.NodeSpeed);
        }
        public Vector3 GetClonePos(int lane)
        {
            if (_clonePos == null || 0 > lane || lane >= _clonePos.Length)
            {
                Debug.LogError($"[StageContext] Lane index out of range. Lane:{lane} Length:{_clonePos.Length}");
                return Vector3.zero;
            }

            return _clonePos[lane].position;
        }
        public void ChangeArrivalSeconds(float arrivalSeconds)
        {
            _arrivalSeconds = arrivalSeconds;
        }
    }
}
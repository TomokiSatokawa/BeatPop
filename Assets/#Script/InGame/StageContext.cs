using InGame.Node;
using UnityEngine;

namespace InGame
{
    /// <summary>
    ///  ステージに関する各種設定を提供する
    /// </summary>
    public class StageConfig : SingletonMonoBehaviour<StageConfig>
    {
        [SerializeField] private StageLayoutData _stageLayoutData;
        [SerializeField] private JudgementTable _judgementTable;
        [SerializeField] private int _longNoteDivisionInterval = 8;
        [SerializeField] private Transform[] _clonePos;
        [SerializeField] private float _arrivalSeconds;

        public StageLayoutData StageLayout => _stageLayoutData;
        public JudgementTable JudgementTable => _judgementTable;
        public int LongNoteDivisionInterval => _longNoteDivisionInterval;
        public float ArrivalSeconds => _arrivalSeconds;

        public Vector3 GetClonePos(int lane)
        {
            if (_clonePos == null || 0 > lane || lane >= _clonePos.Length)
            {
                Debug.LogError($"[StageContext] Lane index out of range. Lane:{lane} Length:{_clonePos.Length}");
                return Vector3.zero;
            }

            return _clonePos[lane].position;
        }
    }
}
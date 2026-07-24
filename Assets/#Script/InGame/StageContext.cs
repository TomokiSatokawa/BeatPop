using UnityEngine;

namespace InGame
{
    /// <summary>
    /// StageLayoutData‚ðŠÈ’P‚ÉŽæ“¾‚·‚é
    /// </summary>
    public class StageContext : SingletonMonoBehaviour<StageContext>
    {
        [SerializeField] private StageLayoutData _stageLayoutData;
        [SerializeField] private Transform[] _clonePos;
        [SerializeField] private float _arrivalSeconds;

        public StageLayoutData StageLayout => _stageLayoutData;
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
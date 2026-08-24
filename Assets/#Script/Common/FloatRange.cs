using UnityEngine;

namespace Common
{
    /// <summary>
    /// MinMax‚Ì’l
    /// </summary>
    [System.Serializable]
    public struct FloatRange
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public float Min => _min;
        public float Max => _max;

        public float GetRandom()
        {
            return Random.Range(_min, _max);
        }

        public bool Contains(float value)
        {
            return value >= _min && value <= _max;
        }
    }
}
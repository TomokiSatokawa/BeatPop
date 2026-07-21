using NUnit.Framework.Constraints;
using UnityEngine;

namespace Input
{
    /// <summary>
    /// タッチ入力の計算
    /// </summary>
    public class TouchManager : MonoBehaviour
    {
        [SerializeField] private float _flickMoveAmount;
        private FloatRange _laneY;

        //2レーン固定
        private FloatRange _lane0;
        private FloatRange _lane1;

        private void Start()
        {
            int width = Screen.width;
            int height = Screen.height;

            _laneY = new(0, height / 2);
            _lane0 = new(0, width / 2);
            _lane1 = new(width / 2, width);
        }

        public int TapLane(Vector2 pos)
        {
            if (!_laneY.Contains(pos.y)) return -1;

            if (_lane0.Contains(pos.x)) return 0;
            if (_lane1.Contains(pos.x)) return 1;

            return -1;
        }

        public bool IsFlick(Vector2 start, Vector2 end)
        {
            return end.y - start.y > _flickMoveAmount;
        }
    }

    [System.Serializable]
    public readonly struct FloatRange
    {
        public float Min { get; }
        public float Max { get; }
        public FloatRange(float min , float max)
        {
            if (min > max)
                (min, max) = (max, min);

            Min = min;
            Max = max;
        }
        public bool Contains(float value)
        {
            return value >= Min && value <= Max;
        }
    }
}
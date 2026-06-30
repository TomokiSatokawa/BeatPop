using UnityEngine;

namespace Input
{
    public class TouchManager : MonoBehaviour
    {
        private FloatRange _laneY;
        private FloatRange _lane0;
        private FloatRange _lane1;
        private FloatRange _lane2;
        private FloatRange _lane3;

        private void Start()
        {
            int width = Screen.width;
            int height = Screen.height;

            _laneY.Min = 0;
            _laneY.Max = height / 2;

            _lane0.Min = 0;
            _lane0.Max = width / 2;

            _lane1.Min = width / 2;
            _lane1.Max = width;

        }
        public int TapLane(Vector2 pos)
        {
            if (!_laneY.Contains(pos.y)) return -1;

            if (_lane0.Contains(pos.x)) return 0;
            if (_lane1.Contains(pos.x)) return 1;
            //if (_lane2.Contains(pos.x)) return 2;
            //if (_lane3.Contains(pos.x)) return 3;

            return -1;
        }
    }
    [System.Serializable]
    public struct FloatRange
    {
        public float Min;
        public float Max;
        public bool Contains(float value)
        {
            return value >= Min && value <= Max;
        }
    }
}
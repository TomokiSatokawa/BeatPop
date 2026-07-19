using InGame;
using System;
using UnityEngine;

namespace Common.BeatUpdate
{
    /// <summary>
    /// BeatUpdateに登録する1個単位のデータ
    /// </summary>
    public class BeatUpdateHandle
    {
        private  const int MaxDivision = 64;
        public int Division { get; }
        public float SecondOffset { get; }
        public Action<BeatData> Callback { get; }
        public float NextTime { get; private set; }

        private float _baseDivisionInterval;
        private float _interval;

        public BeatUpdateHandle(int division, float offset, Action<BeatData> callback)
        {
            Division = division;
            SecondOffset = offset;
            Callback = callback;

            Refresh();
        }

        public void Refresh()
        {
            _interval = (60f / StageTimeController.I.BPM) * (4f / Division);
            _baseDivisionInterval = (60f / StageTimeController.I.BPM) * (4f / MaxDivision);
            UpdateNextTime();
        }

        public void UpdateNextTime()
        {
            float beat = (StageTimeController.StageTime - SecondOffset) / _interval;
            NextTime = (Mathf.Floor(beat) + 1) * _interval + SecondOffset;
        }

        public void Tick()
        {
            if (NextTime <= StageTimeController.StageTime)
            {
                int beatIndex = Mathf.RoundToInt((NextTime - SecondOffset) / _baseDivisionInterval);
                Callback?.Invoke(new(NextTime, GetBeatDivision(beatIndex)));
                UpdateNextTime();
            }
        }

        /// <summary>
        /// beatIndexに対応する拍の分割数を取得する
        /// </summary>
        private static int GetBeatDivision(int beatIndex)
        {
            if (beatIndex == 0)
                return 1;

            int division = MaxDivision;

            while ((beatIndex & 1) == 0)
            {
                beatIndex >>= 1;
                division >>= 1;
            }

            return division;
        }
    }

    /// <summary>
    /// BeatUpdateコールバック時に渡すデータ
    /// </summary>
    public readonly struct BeatData
    {
        public float Time { get; }
        public int Division { get; }
        public BeatData(float time, int division)
        {
            Time = time;
            Division = division;
        }
    }
}
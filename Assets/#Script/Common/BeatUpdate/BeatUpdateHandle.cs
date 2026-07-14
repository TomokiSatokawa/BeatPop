using InGame.UI;
using System;
using UnityEngine;

namespace Common.BeatUpdate
{
    public class BeatUpdateHandle
    {
        public int Division { get; }
        public float SecondOffset { get; }
        public Action<BeatData> Callback { get; }

        public float NextTime { get; private set; }

        private float _sixtyFourthInterval;
        private float _interval;
        public BeatUpdateHandle(int division, float offset, Action<BeatData> callback)
        {
            Division = division;
            SecondOffset = offset;
            Callback = callback;
            NextTime = 0;
            UpdateConstants();
            UpdateNextTime();
        }

        public void UpdateConstants()
        {
            _interval = (60f / StageTimeController.I.BPM) * (4f / Division);
            _sixtyFourthInterval = (60f / StageTimeController.I.BPM) / 16f;
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
                int sixteenthIndex = Mathf.RoundToInt((NextTime - SecondOffset) / _sixtyFourthInterval);
                Callback?.Invoke(new(NextTime, GetBeatDivision(sixteenthIndex)));
                UpdateNextTime();
            }
        }
        public static int GetBeatDivision(int sixtyFourthIndex)
        {
            if (sixtyFourthIndex % 64 == 0)
                return 1;   // ‘S‰¹•„

            if (sixtyFourthIndex % 32 == 0)
                return 2;   // 2•ª‰¹•„

            if (sixtyFourthIndex % 16 == 0)
                return 4;   // 4•ª‰¹•„

            if (sixtyFourthIndex % 8 == 0)
                return 8;   // 8•ª‰¹•„

            if (sixtyFourthIndex % 4 == 0)
                return 16;  // 16•ª‰¹•„

            if (sixtyFourthIndex % 2 == 0)
                return 32;  // 32•ª‰¹•„

            return 64;      // 64•ª‰¹•„
        }
    }
    public struct BeatData
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

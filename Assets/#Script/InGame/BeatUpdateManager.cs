using System;
using System.Collections.Generic;
using InGame.UI;
using R3;
using UnityEngine;
namespace InGame
{
    public class BeatUpdateManager : SingletonMonoBehaviour<BeatUpdateManager>
    {
        private readonly List<BeatUpdateHandle> _activeFastBeatUpdate = new();
        private readonly List<BeatUpdateHandle> _activeBeatUpdate = new();
        private readonly List<BeatUpdateHandle> _activeLateBeatUpdate = new();
        private float _previousTime;
        public void Start()
        {
            StageTimeController.I.OnInitialized.Subscribe(_ => UpdateAllNextTime());
        }
        public void AddFastBeatUpdate(BeatUpdateHandle handle)
        {
            handle.UpdateNextTime();
            _activeFastBeatUpdate.Add(handle);
        }

        public void AddBeatUpdate(BeatUpdateHandle handle)
        {
            handle.UpdateNextTime();
            _activeBeatUpdate.Add(handle);
        }

        public void AddLateBeatUpdate(BeatUpdateHandle handle)
        {
            handle.UpdateNextTime();
            _activeLateBeatUpdate.Add(handle);
        }
        private void Update()
        {
            float current = StageTimeController.StageTime;

            if (current < _previousTime)
            {
                UpdateAllNextTime();
            }

            foreach (var handle in _activeFastBeatUpdate)
            {
                handle.Tick();
            }
            foreach (var handle in _activeBeatUpdate)
            {
                handle.Tick();
            }
            foreach (var handle in _activeLateBeatUpdate)
            {
                handle.Tick();
            }

            _previousTime = current;
        }

        private void UpdateAllNextTime()
        {
            foreach (var handle in _activeFastBeatUpdate)
            {
                handle.UpdateNextTime();
            }
            foreach (var handle in _activeBeatUpdate)
            {
                handle.UpdateNextTime();
            }
            foreach (var handle in _activeLateBeatUpdate)
            {
                handle.UpdateNextTime();
            }
        }
    }
    public class BeatUpdateHandle
    {
        public int Division { get; }
        public float SecondOffset { get; }
        public Action<float,int> Callback { get; }

        public float NextTime { get;private set; }

        private float _sixtyFourthInterval;
        public BeatUpdateHandle(int division, float offset, Action<float,int> callback)
        {
            Division = division;
            SecondOffset = offset;
            Callback = callback;
            NextTime = 0;
            _sixtyFourthInterval = (60f / StageTimeController.I.BPM) / 16f;
            UpdateNextTime();
        }

        public void UpdateNextTime()
        {
            float interval = (60f / StageTimeController.I.BPM) * (4f / Division);

            float beat = (StageTimeController.StageTime - SecondOffset) / interval;
            NextTime = (Mathf.Floor(beat) + 1) * interval + SecondOffset;
        }

        public void Tick()
        {
            if (NextTime <= StageTimeController.StageTime)
            {
                Debug.Log("aaa");
                int sixteenthIndex = Mathf.RoundToInt((NextTime - SecondOffset) / _sixtyFourthInterval);
                Callback?.Invoke(NextTime,GetBeatDivision(sixteenthIndex));
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
}

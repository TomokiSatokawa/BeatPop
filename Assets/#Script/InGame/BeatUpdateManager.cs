using System;
using System.Collections.Generic;
using InGame.UI;
using UnityEngine;
namespace InGame
{

    public class BeatUpdateManager : SingletonMonoBehaviour<BeatUpdateManager>
    {
        private readonly List<BeatUpdateHandle> _activeBeatUpdate = new();
        public void Register(BeatUpdateHandle handle)
        {
            handle.UpdateNextTime();
            _activeBeatUpdate.Add(handle);
        }

        private void Update()
        {
            foreach(var handle in _activeBeatUpdate)
            {
                handle.Tick();
            }
        }
    }
    public class BeatUpdateHandle
    {
        public int Division { get; }
        public float SecondOffset { get; }
        public Action<float> Callback { get; }

        public float NextTime { get;private set; }
        public BeatUpdateHandle(int division, float offset, Action<float> callback)
        {
            Division = division;
            SecondOffset = offset;
            Callback = callback;
            NextTime = 0;
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
                Callback?.Invoke(NextTime);
                UpdateNextTime();
            }
        }
    }
}

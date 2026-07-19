using System.Collections.Generic;
using System;
using UnityEngine;

namespace Common.BeatUpdate
{
    /// <summary>
    /// BeatUpdateでのBeatUpdateHandleのグループ
    /// </summary>
    public class BeatGroup
    {
        private readonly List<BeatUpdateHandle> _handles = new();

        public void Tick()
        {
            foreach (var handle in _handles)
            {
                handle.Tick();
            }
        }

        public void UpdateAllNextTime()
        {
            foreach (var handle in _handles)
            {
                handle.UpdateNextTime();
            }
        }

        public void RefreshAll()
        {
            foreach (var handle in _handles)
            {
                handle.Refresh();
                handle.UpdateNextTime();
            }
        }

        public void Subscribe(int division, float timeOffset, Action<BeatData> callback)
        {
            if (division <= 0)
            {
                Debug.LogError($"[BeatUpdate] Divisionが不正です : {division}");
                return;
            }

            if (callback == null)
            {
                Debug.LogError("[BeatUpdate] callbackが指定されていません");
                return;
            }

            var handle = new BeatUpdateHandle(division, timeOffset, callback);
            _handles.Add(handle);
        }

        public void Clear()
        {
            _handles.Clear();
        }
    }
}
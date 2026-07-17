using System.Collections.Generic;
using System;

namespace Common.BeatUpdate
{
    public class BeatGroup
    {
        private List<BeatUpdateHandle> _handles = new();

        public void Tick()
        {
            foreach (var handle in _handles)
            {
                handle.Tick();
            }
        }

        public void UpdateNextTime()
        {
            foreach (var handle in _handles)
            {
                handle.UpdateNextTime();
            }
        }

        public void UpdateConstants()
        {
            foreach (var handle in _handles)
            {
                handle.UpdateConstants();
                handle.UpdateNextTime();
            }
        }

        public void Subscribe(int division, float offset, Action<BeatData> callback)
        {
            BeatUpdateHandle handle = new(division, offset, callback);
            handle.UpdateNextTime();
            _handles.Add(handle);
        }

        public void Clear()
        {
            _handles.Clear();
        }
    }
}
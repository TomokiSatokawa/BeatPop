using System.Collections.Generic;
using System;
using UnityEngine;

namespace Common.BeatUpdate
{
    /// <summary>
    /// BeatUpdateでのBeatUpdateHandleのグループ
    /// </summary>
    [System.Serializable]
    public class BeatGroup
    {
        /// <summary> BeatUpdateデータ </summary>
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

        /// <summary>
        /// BeatUpdateに登録する
        /// </summary>
        public IDisposableBeat Subscribe(int division, float timeOffset, Action<BeatData> callback)
        {
            Debug.Log(callback.ToString());
            if (division <= 0)
            {
                Debug.LogError($"[BeatUpdate] Divisionが不正です : {division}");
                return null;
            }

            if (callback == null)
            {
                Debug.LogError("[BeatUpdate] callbackが指定されていません");
                return null;
            }

            var handle = new BeatUpdateHandle(division, timeOffset, callback,BeatUpdateManager.I.UpdateRefresh);
            var disposable = new DisposableBeat(() => Unsubscribe(handle));

            _handles.Add(handle);
            //callback?.Invoke(default);
            return disposable;
        }

        private void Unsubscribe(BeatUpdateHandle handle)
        {
            _handles.Remove(handle);
        }

        public void Clear()
        {
            _handles.Clear();
        }
    }
}
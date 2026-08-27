using System;

namespace Common.BeatUpdate
{
    /// <summary>
    /// BeatUpdateの消すインターフェース
    /// </summary>
    public interface IDisposableBeat
    {
        public void Dispose();
    }
    public class DisposableBeat : IDisposableBeat
    {
        public DisposableBeat(Action onDispose)
        {
            _onDispose = onDispose;
        }

        private Action _onDispose;
        public void Dispose()
        {
            _onDispose?.Invoke();
        }
    }
}

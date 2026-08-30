using Common.PlaySystem;
using Title.Custom;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// InGame内でその他のカスタムを簡単に取得する
    /// </summary>
    public class InGameCustomOtherData : MonoBehaviour
    {
        private static OtherCustomFlags _customFlags;

        private void Start()
        {
            if (SongPlayContext.I == null) return;

            _customFlags = OtherCustomFlags.Create(SongPlayContext.I.PatternData.OtherPattern.Flags);
         }

        public static bool HasFlag(CustomOtherType otherType)
        {
            return _customFlags.Has(otherType);
        }
    }
}
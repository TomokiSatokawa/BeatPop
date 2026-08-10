using Title.Custom;
using Title.SongSelect;

namespace Common.PlaySystem
{
    /// <summary>
    /// 再生する曲データ、設定を保存するクラス
    /// </summary>
    public class SongPlayContext : SingletonPersistent<SongPlayContext>
    {
        /// <summary> 曲データ </summary>
        public SongSelectData SongData { get; private set; } 
        /// <summary> カスタムパターンデータ </summary>
        public PatternJsonData PatternData { get; private set; }
        /// <summary> 開始セクション </summary>
        public int StartSection { get; private set; }
        /// <summary> オートプレイ </summary>
        public bool IsAutoPlay { get; private set; } = false;
        /// <summary> オートプレイ設定 </summary>
        public AutoPlaySetting AutoPlaySetting { get; private set; }

        public void SetData(SongSelectData songData,PatternJsonData patternData,int section)
        {
            SongData = songData;
            PatternData = patternData;
            StartSection = section;
        }

        public void SetAutoPlay(bool isAutoPlay, AutoPlaySetting setting= default)
        {
            IsAutoPlay = isAutoPlay;
            AutoPlaySetting　=setting;
        }
    }

    [System.Serializable]
    public struct AutoPlaySetting
    {
        public float AutoPlayOffset;

        public AutoPlaySetting(float offset = 0)
        {
            AutoPlayOffset = offset;
        }
    }
}

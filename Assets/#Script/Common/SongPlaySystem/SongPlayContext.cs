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

        public void SetData(SongSelectData songData,PatternJsonData patternData,int section)
        {
            SongData = songData;
            PatternData = patternData;
            StartSection = section;
        }
    }
}

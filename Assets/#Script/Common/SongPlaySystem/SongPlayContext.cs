namespace Common.PlaySystem
{
    /// <summary>
    /// 再生する曲データ、設定を保存するクラス
    /// </summary>
    public class SongPlayContext : SingletonPersistent<SongPlayContext>
    {
        public SongSelectData SongData { get; private set; } 
        public PatternJsonData PatternData { get; private set; }
        public int StartSection { get; private set; }

        public void SetData(SongSelectData songData,PatternJsonData patternData,int section)
        {
            SongData = songData;
            PatternData = patternData;
            StartSection = section;
        }
    }
}

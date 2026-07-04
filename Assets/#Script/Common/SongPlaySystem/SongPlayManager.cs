namespace Common.PlaySystem
{
    public class SongPlayManager : SingletonMonoBehaviour<SongPlayManager>
    {
        public SongSelectData SongData { get; private set; } 
        public PatternJsonData PatternData { get; private set; }
        public int StartSection { get; private set; }
        public void SetData(SongSelectData song,PatternJsonData patternData,int section)
        {
            SongData = song;
            PatternData = patternData;
            StartSection = section;
        }
        public void Delete()
        {
            Destroy(this.gameObject);
        }
    }
}

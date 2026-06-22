namespace Common.PlaySystem
{
    public class SongPlayManager : SingletonMonoBehaviour<SongPlayManager>
    {
        public SongSelectData SongData { get; private set; } 
        public PatternJsonData PatternData { get; private set; }
        public void SetData(SongSelectData song,PatternJsonData patternData)
        {
            SongData = song;
            PatternData = patternData;
        }
        public void Delete()
        {
            Destroy(this.gameObject);
        }
    }
}

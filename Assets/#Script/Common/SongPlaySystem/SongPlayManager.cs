namespace Common.PlaySystem
{
    public class SongPlayManager : SingletonMonoBehaviour<SongPlayManager>
    {
        public IReadOnlySongData SongData { get; private set; } 
        public void SetData(IReadOnlySongData song)
        {
            SongData = song;
        }
    }
}

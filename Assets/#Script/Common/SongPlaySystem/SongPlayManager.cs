namespace Common.PlaySystem
{
    public class SongPlayManager : SingletonMonoBehaviour<SongPlayManager>
    {
        public SongSelectData SongData { get; private set; } 
        public void SetData(SongSelectData song)
        {
            SongData = song;
        }
        public void Delete()
        {
            Destroy(this.gameObject);
        }
    }
}

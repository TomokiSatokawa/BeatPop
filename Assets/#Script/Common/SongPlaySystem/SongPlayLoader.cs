using UnityEngine;
namespace Common.PlaySystem
{
    public class SongPlayLoader : MonoBehaviour
    {
        [SerializeField] private SongPlayManager _managerPrefab;

        public void OnLoad(IReadOnlySongData songData)
        {
            if(SongPlayManager.I != null)
            {
                Debug.LogError("SongPlayManager‚ªŠù‚É‘¶İ‚µ‚Ä‚¢‚Ü‚·");
                return;
            }

            var playManager = Instantiate(_managerPrefab);
            playManager.SetData(songData);
            DontDestroyOnLoad(playManager);
        }
    }
}

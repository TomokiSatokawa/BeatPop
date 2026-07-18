using UnityEngine;
namespace Common.PlaySystem
{
    public class SongPlayLoader : MonoBehaviour
    {
        [SerializeField] private SongPlayManager _managerPrefab;

        public void OnLoad(SongSelectData songData,PatternJsonData patternJsonData)
        {
            var playManager = Instantiate(_managerPrefab);
            playManager.SetData(songData,patternJsonData,0);
        }
    }
}

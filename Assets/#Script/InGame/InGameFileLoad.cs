using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using R3;

namespace InGame
{
    /// <summary>
    /// InGame専用のファイルローダー
    /// </summary>
    public class InGameFileLoad : SingletonMonoBehaviour<InGameFileLoad>
    {
        private Subject<NodeSaveData> _onFileLoaded = new();
        public Observable<NodeSaveData> OnFileLoaded => _onFileLoaded;

        public async UniTask FileLoad()
        {
            var data = await NodeDataSerializer.AutoDeserialize(SongPlayManager.I.SongData.GetNodeJson().text);
            _onFileLoaded.OnNext(data);
        }
    }
}

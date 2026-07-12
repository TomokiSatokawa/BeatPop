using Common;
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
        private Subject<NodeSaveData> _onNodeFileLoaded = new();
        public Observable<NodeSaveData> OnNodeFileLoaded => _onNodeFileLoaded;
        private Subject<StageSaveData> _onStageFileLoaded = new();
        public Observable<StageSaveData> OnStageFileLoaded => _onStageFileLoaded;

        public async UniTask FileLoad()
        {
            var nodeData = await NodeDataSerializer.AutoDeserialize(SongPlayManager.I.SongData.GetNodeJson().text);
            var stageData = StageDataSerializer.DeserializeJson(SongPlayManager.I.SongData.SongData.StageEffectData.text);
            _onNodeFileLoaded.OnNext(nodeData);
            _onStageFileLoaded.OnNext(stageData);
        }
    }
}

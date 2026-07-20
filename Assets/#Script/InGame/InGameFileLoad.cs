using Common;
using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using R3;
using Debug = UnityEngine.Debug;

namespace InGame
{
    /// <summary>
    /// InGame専用のファイルローダー
    /// </summary>
    public class InGameFileLoad : SingletonMonoBehaviour<InGameFileLoad>
    {
        private readonly ReactiveProperty<NodeSaveData> _onNodeFileLoaded = new();
        public ReadOnlyReactiveProperty<NodeSaveData> OnNodeFileLoaded => _onNodeFileLoaded;
        private readonly ReactiveProperty<StageSaveData> _onStageFileLoaded = new();
        public ReadOnlyReactiveProperty<StageSaveData> OnStageFileLoaded => _onStageFileLoaded;

        public async UniTask FileLoad()
        {
            var nodeData = await NodeDataSerializer.AutoDeserialize(SongPlayContext.I.SongData.GetNodeJson().text);
            var stageData = StageDataSerializer.DeserializeJson(SongPlayContext.I.SongData.SongData.StageEffectData.text);

            if (nodeData == null)
                Debug.LogError("[InGameFileLoad] NodeDataがありません");

            if (stageData == null)
                Debug.LogError("[InGameFileLoad] StageDataがありません");

            _onNodeFileLoaded.OnNext(nodeData);
            _onStageFileLoaded.OnNext(stageData);
        }
    }
}

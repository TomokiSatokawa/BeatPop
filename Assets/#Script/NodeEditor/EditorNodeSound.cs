using System.Collections.Generic;
using System.Linq;
using InGame;
using InGame.Node;
using R3;
using Sound;
using UnityEngine;

namespace JsonEditor
{
    /// <summary>
    /// エディターでノーツの音を出す
    /// </summary>
    public class EditorNodeSound : MonoBehaviour
    {
        private Queue<NodeData> _soundTimeData;

        public void Start()
        {
            StageTimeController.I.IsPlaying
                .Where(x => x)
                .Subscribe(_ => UpdateNodeData())
                .AddTo(this);
        }
        public void UpdateNodeData()
        {
            _soundTimeData = new Queue<NodeData>(
                EditorNodeData.I.Nodes
                .Where(x => x.Time >= StageTimeController.StageTime)
                .OrderBy(x => x.Time));
        }
        void Update()
        {
            if (!StageTimeController.I.IsPlaying.CurrentValue) return;

            while (_soundTimeData.Count > 0 && _soundTimeData.Peek().Time <= StageTimeController.StageTime)
            {
                var nodeData = _soundTimeData.Dequeue();

                var seData = InGameCustomSoundData.I.NodeSE[nodeData.PrefabType];
                SoundManager.SE.PlaySE(seData.Clip, seData.Volume);
            }
        }
    }
}
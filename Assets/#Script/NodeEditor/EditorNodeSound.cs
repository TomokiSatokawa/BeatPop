using System.Collections.Generic;
using System.Linq;
using InGame.Node;
using InGame;
using R3;
using Sound;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// エディターでノーツの音を出す
    /// </summary>
    public class EditorNodeSound : MonoBehaviour
    {
        private List<NodeData> _soundTimeData;

        public void Start()
        {
            StageTimeController.I.IsPlaying
                .Where(x => x)
                .Subscribe(_ => UpdateNodeData())
                .AddTo(this);
        }
        public void UpdateNodeData()
        {
            _soundTimeData = new();
            foreach (var nodeData in EditorNodeData.I.Nodes)
            {
                if (nodeData.Time < StageTimeController.StageTime)
                {
                    continue;
                }
                _soundTimeData.Add(nodeData);
            }
            _soundTimeData = _soundTimeData.OrderBy(x => x.Time).ToList();
        }
        void Update()
        {
            if (!StageTimeController.I.IsPlaying.CurrentValue) return;
            List<NodeData> removeList = new();

            foreach (var nodeData in _soundTimeData)
            {
                if (nodeData.Time <= StageTimeController.StageTime)
                {
                    SoundManager.SE.PlaySE(InGameCustomSoundData.I.NodeSE[nodeData.PrefabType]);
                    removeList.Add(nodeData);
                    continue;
                }
                break;
            }

            foreach (var nodeData in removeList)
            {
                _soundTimeData.Remove(nodeData);
            }
        }
    }
}
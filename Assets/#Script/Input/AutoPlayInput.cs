using System;
using System.Collections.Generic;
using Common.BeatUpdate;
using InGame;
using InGame.Node;
using R3;
using UnityEngine;

namespace Input
{
    /// <summary>
    /// オートプレイ入力
    /// </summary>
    public class AutoPlayInput
    {
        private readonly List<NodeData> _nodeDatas;
        private int _nextNode;

        public event Action<bool> LeftMain;
        public event Action<bool> RightMain;
        public event Action<bool> LeftFlick;
        public event Action<bool> RightFlick;

        public AutoPlayInput(List<NodeData> nodeDatas)
        {
            _nodeDatas = nodeDatas;
            _nextNode = 0;

            StageTimeController.I.OnInitialized.Subscribe(_ =>
            {
                BeatUpdateManager.LateBeatUpdate.Subscribe(16, 0, _ => BeatUpdate());
            });
        }

        public void BeatUpdate()
        {
            while (_nextNode < _nodeDatas.Count)
            {
                NodeData node = _nodeDatas[_nextNode];

                // まだ入力タイミングではない
                if (node.Time > StageTimeController.StageTime)
                    break;

                Debug.Log(node.PrefabType);
                OnInput(node);
                _nextNode++;
            }
        }

        private void OnInput(NodeData node)
        {
            switch (node.PrefabType)
            {
                case PoolPrefabType.NormalNote:
                case PoolPrefabType.HighScoreNote:
                    InvokeMain(node.Lane, true);
                    InvokeMain(node.Lane, false);
                    break;

                case PoolPrefabType.FlickNote:
                    InvokeMain(node.Lane, true);
                    InvokeFlick(node.Lane,true);
                    InvokeMain(node.Lane, false);
                    break;

                case PoolPrefabType.HoldNoteStart:
                    InvokeMain(node.Lane, true);
                    break;

                case PoolPrefabType.HoldNoteEnd:
                    InvokeMain(node.Lane, false);
                    break;

                case PoolPrefabType.HoldFlickEnd:
                    InvokeMain(node.Lane, false);
                    InvokeFlick(node.Lane,false);
                    break;
            }
        }

        private void InvokeMain(int lane, bool isPressed)
        {
            GetMainAction(lane)?.Invoke(isPressed);
        }

        private void InvokeFlick(int lane, bool isPressed)
        {
            (lane == 0 ? LeftFlick : RightFlick)?.Invoke(isPressed);
        }

        private Action<bool> GetMainAction(int lane)
        {
            return lane == 0 ? LeftMain : RightMain;
        }
    }
}

using InGame;
using InGame.Stage;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace Editor
{
    public class EditorRecording : MonoBehaviour
    {
        [SerializeField] private Slider _channel;
        private RecordingMode _mode;
        public void Update()
        {
            switch (_mode)
            {
                case RecordingMode.NormalNode:
                    NormalNode();
                    break;
                case RecordingMode.LongNode:
                    HoldNode();
                    break;
                case RecordingMode.LightEditor:
                    LightMode();
                    break;
            }
        }

        public void NoneMode()
        {
            _mode = RecordingMode.None;
        }

        public void NormalNodeMode()
        {
            _mode = RecordingMode.NormalNode;
        }

        public void LongNodeMode()
        {
            _mode = RecordingMode.LongNode;
        }

        public void LightEditorMode()
        {
            _mode |= RecordingMode.LightEditor;
        }

        private void NormalNode()
        {
            if (EditorNodeData.I == null) return;

            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                EditorNodeData.I.AddNode(PoolPrefabType.NormalNote, GetNodeTime(), 0);
            }

            if (Keyboard.current.jKey.wasPressedThisFrame)
            {
                EditorNodeData.I.AddNode(PoolPrefabType.NormalNote, GetNodeTime(), 1);
            }
        }

        private void HoldNode()
        {
            if (EditorNodeData.I == null) return;

            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                EditorNodeData.I.AddNode(PoolPrefabType.HoldNoteStart, GetNodeTime(), 0);
            }
            else if (Keyboard.current.fKey.wasReleasedThisFrame)
            {
                EditorNodeData.I.AddNode(PoolPrefabType.HoldNoteEnd, GetNodeTime(), 0);
            }

            if (Keyboard.current.jKey.wasPressedThisFrame)
            {
                EditorNodeData.I.AddNode(PoolPrefabType.HoldNoteStart, GetNodeTime(), 1);
            }
            else if (Keyboard.current.jKey.wasReleasedThisFrame)
            {
                EditorNodeData.I.AddNode(PoolPrefabType.HoldNoteEnd, GetNodeTime(), 1);
            }
        }

        private void LightMode()
        {
            if (EditorLightData.I == null
                || _channel == null) return;

            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                var data = new LightPatternBaseData();
                data.Time = (float)GetNodeTime();
                data.Channel = (int)_channel.value;
                EditorLightData.I.AddNode(data);
            }
        }

        private static float GetNodeTime()
        {
            return (float)SnapNode.SnapNodeTime((double)StageTimeController.StageTime);
        }
        public enum RecordingMode
        {
            None, NormalNode, LongNode, LightEditor
        }
    }
}
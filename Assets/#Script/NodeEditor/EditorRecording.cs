using InGame.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditorRecording : MonoBehaviour
{
    private RecordingMode _mode;
    public void Update()
    {
        switch (_mode)
        {
            case RecordingMode.NormalNode:
                NormalNode();
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
            float nodeTime = (float)SnapNode.SnapNodeTime((double)StageTimeController.StageTime);
            EditorNodeData.I.AddNode(PoolPrefabType.NormalNote, nodeTime, 0);
        }

        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            float nodeTime = (float)SnapNode.SnapNodeTime((double)StageTimeController.StageTime);
            EditorNodeData.I.AddNode(PoolPrefabType.NormalNote, nodeTime, 1);
        }
    }

    public enum RecordingMode
    {
        None,NormalNode, LongNode, LightEditor
    }
}

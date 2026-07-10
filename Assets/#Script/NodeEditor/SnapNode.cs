using InGame.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnapNode : MonoBehaviour
{
    [SerializeField] private RectTransform _snapObjectParent;
    [SerializeField] private RectTransform _sectionPointer;
    [SerializeField] private RectTransform _lightPointer;
    [SerializeField] private RectTransform _createPointer;
    [SerializeField] private RectTransform _pointerRect;
    [SerializeField] private RectTransform[] _laneRect;

    private EditMode _editMode = EditMode.None;
    private PoolPrefabType _prefabType;
    public void Start()
    {
        //if (_snapObject != null) return;
        //_snapObject = PoolManager.I.Get<EditorNode>(PoolPrefabType.EditorNote, _snapObjectParent);
    }
    public void OnNewNode(int prefab)
    {
        _editMode = EditMode.Create;
        _prefabType = (PoolPrefabType)prefab;
    }


    public void OnSection()
    {
        _editMode = EditMode.Section;
    }
    public void Update()
    {
        if (_editMode == EditMode.None) return;
        Vector2 mousePos = Mouse.current.position.ReadValue();

        RectTransform parent = _snapObjectParent;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent,
            mousePos,
            null,
            out Vector2 localMousePos);

        _createPointer.gameObject.SetActive(_editMode == EditMode.Create);
        _sectionPointer.gameObject.SetActive(_editMode == EditMode.Section);
        _lightPointer.gameObject.SetActive(_editMode == EditMode.Light);

        Vector2 pos = localMousePos;

        int laneIndex = 0;
        foreach (var lane in _laneRect)
        {
            float minY = lane.anchoredPosition.y + lane.rect.yMin;
            float maxY = lane.anchoredPosition.y + lane.rect.yMax;

            if (localMousePos.y >= minY && localMousePos.y <= maxY)
            {
                pos.y = lane.anchoredPosition.y;
                break;
            }
            laneIndex++;
        }

        if (laneIndex == _laneRect.Length)
        {
            _createPointer.gameObject.SetActive(false);
            return;
        }


        float beatInterval = 60f / StageTimeController.I.BPM;

        float barInterval = beatInterval * 4f;
        float divisionInterval = barInterval / EditorManager.I.Division;

        // X → Time
        double noteTime =
            StageTimeController.StageTime +
            (localMousePos.x / EditorManager.I.Magnification);

        // Timeをスナップ
        noteTime =
            Mathf.Round((float)(noteTime / divisionInterval))
            * divisionInterval;

        // Time → X
        pos.x = (float)(noteTime - StageTimeController.StageTime)
            * EditorManager.I.Magnification;

        _sectionPointer.anchoredPosition = pos;
        _createPointer.anchoredPosition = pos;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            switch (_editMode)
            {
                case EditMode.Create:
                    EditorNodeData.I.AddNode(_prefabType, noteTime, laneIndex);
                    break;

                case EditMode.Section:
                    EditorNodeData.I.AddSection((float)noteTime);
                    break;
            }
        }

        if(Mouse.current.rightButton.wasPressedThisFrame)
        {
            switch (_editMode)
            {
                case EditMode.Create:
                    EditorNodeData.I.DeleteNode(noteTime, laneIndex);
                    break;

                case EditMode.Section:
                    EditorNodeData.I.RemoveSection((float)noteTime);
                    break;
            }
        }
    }
    public enum EditMode
    {
        None, Create, Section,Light
    }
}
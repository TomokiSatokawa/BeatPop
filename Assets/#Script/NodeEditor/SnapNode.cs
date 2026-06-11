using UnityEngine;
using UnityEngine.InputSystem;

public class SnapNode : MonoBehaviour
{
    [SerializeField] private RectTransform _snapObjectParent;
    [SerializeField] private RectTransform _deletePointer;
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


    public void OnDelete()
    {
        _editMode = EditMode.Delete;
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
        _deletePointer.gameObject.SetActive(_editMode == EditMode.Delete);

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


        float beatInterval = 60f / EditorManager.I.BPM;

        float barInterval = beatInterval * 4f;
        float divisionInterval = barInterval / EditorManager.I.Divition;

        // X → Time
        double noteTime =
            EditorManager.I.EditorTime.CurrentValue +
            (localMousePos.x / EditorManager.I.Magnification);

        // Timeをスナップ
        noteTime =
            Mathf.Round((float)(noteTime / divisionInterval))
            * divisionInterval;

        // Time → X
        pos.x = (float)(noteTime - EditorManager.I.EditorTime.CurrentValue)
            * EditorManager.I.Magnification;

        _deletePointer.anchoredPosition = pos;
        _createPointer.anchoredPosition = pos;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_editMode == EditMode.Create)
            {
                EditorNodeData.I.AddNode(_prefabType,noteTime, laneIndex);
            }
            else if (_editMode == EditMode.Delete)
            {
                EditorNodeData.I.DeleteNode(noteTime, laneIndex);
            }
        }
    }
    public enum EditMode
    {
        None, Create, Delete
    }
}
using Editor.UI;
using InGame;
using InGame.Stage;
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
    [SerializeField] private UIPointerHover _fieldHover;
    [SerializeField] private PatternSettingsControl _patternSettingsControl;

    private LightPatternBaseData _snapPattern = new LightPatternBaseData();
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
    public void OnLight()
    {
        _editMode = EditMode.Light;
        _patternSettingsControl.ShowSettings(_snapPattern);
    }
    public void Update()
    {
        if (_editMode == EditMode.None) return;
        if (!_fieldHover.IsPointerOver.CurrentValue) return;
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



        int laneIndex = GetLaneIndex();

        if (laneIndex == -1)
        {
            _createPointer.gameObject.SetActive(false);
            return;
        }
        Vector3 worldPos = _laneRect[laneIndex].TransformPoint(_laneRect[laneIndex].rect.center);
        Vector3 localPos = _snapObjectParent.InverseTransformPoint(worldPos);

        pos.y = localPos.y;



        // X Å® Time
        double noteTime =
            StageTimeController.StageTime +
            (localMousePos.x / EditorManager.I.Magnification);

        noteTime = SnapNodeTime(noteTime);

        // Time Å® X
        pos.x = (float)(noteTime - StageTimeController.StageTime)
            * EditorManager.I.Magnification;

        _sectionPointer.anchoredPosition = pos;
        _createPointer.anchoredPosition = pos;
        _lightPointer.anchoredPosition = pos;

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
                case EditMode.Light:
                    var data = _snapPattern.Clone();
                    data.Time = (float)noteTime;
                    data.Channel = laneIndex;
                    EditorLightData.I.AddNode(data);
                    break;
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            switch (_editMode)
            {
                case EditMode.Create:
                    EditorNodeData.I.DeleteNode(noteTime, laneIndex);
                    break;

                case EditMode.Section:
                    EditorNodeData.I.RemoveSection((float)noteTime);
                    break;
                case EditMode.Light:
                    EditorLightData.I.DeleteNode((float)noteTime, laneIndex);
                    break;
            }
        }
    }

    public static double SnapNodeTime(double noteTime)
    {
        float beatInterval = 60f / StageTimeController.I.BPM;

        float barInterval = beatInterval * 4f;
        float divisionInterval = barInterval / EditorManager.I.Division;


        noteTime =
            Mathf.Round((float)(noteTime / divisionInterval))
            * divisionInterval;
        return noteTime;
    }

    private int GetLaneIndex()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        for (int i = 0; i < _laneRect.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(
                    _laneRect[i],
                    mousePos,
                    null))
            {
                return i;
            }
        }

        return -1;
    }
    public enum EditMode
    {
        None, Create, Section, Light
    }
}
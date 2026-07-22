using Editor.UI;
using InGame;
using InGame.Stage;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Editor
{
    /// <summary>
    /// クリックによるノーツ配置
    /// </summary>
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

        private LightPatternBaseData _previewPattern = new LightPatternBaseData();
        private EditMode _editMode = EditMode.None;
        private PoolPrefabType _prefabType;

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
            _patternSettingsControl.ShowSettings(_previewPattern);
        }

        public void Update()
        {
            if (_editMode == EditMode.None) return;
            if (!_fieldHover.IsPointerOver.CurrentValue) return;


            if (!UpdatePointer(out var laneIndex, out var noteTime)) return;
            UpdatePointerVisible();

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                CreateObject(laneIndex, noteTime);
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                DeleteObject(laneIndex, noteTime);
            }
        }

        private void DeleteObject(int laneIndex, double noteTime)
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
                    EditorLightData.I.RemoveLightPattern((float)noteTime, laneIndex);
                    break;
            }
        }

        private void CreateObject(int laneIndex, double noteTime)
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
                    var data = _previewPattern.Clone();
                    data.Time = (float)noteTime;
                    data.Channel = laneIndex;
                    EditorLightData.I.AddLightPattern(data);
                    break;
            }
        }

        private bool UpdatePointer(out int laneIndex, out double noteTime)
        {
            laneIndex = 0;
            noteTime = 0;

            Vector2 mousePos = Mouse.current.position.ReadValue();

            RectTransform parent = _snapObjectParent;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                mousePos,
                null,
                out Vector2 localMousePos);

            Vector2 pos = localMousePos;

            laneIndex = GetLaneIndex();
            if (laneIndex == -1)
            {
                _createPointer.gameObject.SetActive(false);
                return false;
            }
            Vector3 worldPos = _laneRect[laneIndex].TransformPoint(_laneRect[laneIndex].rect.center);
            Vector3 localPos = _snapObjectParent.InverseTransformPoint(worldPos);

            pos.y = localPos.y;

            // X → Time
            noteTime = StageTimeController.StageTime +
                (localMousePos.x / EditorManager.I.Magnification);
            noteTime = SnapNodeTime(noteTime);

            // Time → X
            pos.x = (float)(noteTime - StageTimeController.StageTime)
                * EditorManager.I.Magnification;
            return true;
        }

        private void UpdatePointerVisible()
        {
            _createPointer.gameObject.SetActive(_editMode == EditMode.Create);
            _sectionPointer.gameObject.SetActive(_editMode == EditMode.Section);
            _lightPointer.gameObject.SetActive(_editMode == EditMode.Light);
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
}
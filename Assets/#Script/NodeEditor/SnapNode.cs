using InGame;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Editor
{
    /// <summary>
    /// クリックによるノーツ配置
    /// </summary>
    public class SnapNode : MonoBehaviour
    {
        [Header("TimeLine")]
        [SerializeField] private RectTransform _snapObjectParent;
        [SerializeField] private Image _pointerRect;
        [SerializeField] private RectTransform[] _laneRect;
        [SerializeField] private UIPointerHover _fieldHover;
        [Header("State")]
        [SerializeField] private NodeSnapState _nodeSnapState;
        [SerializeField] private SectionSnapState _sectionSnapState;
        [SerializeField] private LightPatternSnapState _lightPatternSnapState;

        private EditorSnapStateBase _currentSnapState;
        public void OnNewNode(int prefab)
        {
            _nodeSnapState.SetPrefabType((PoolPrefabType)prefab);
            ChangeState(_nodeSnapState);
        }

        public void OnSection()
        {
            ChangeState(_sectionSnapState);
        }

        public void OnLight()
        {
            ChangeState(_lightPatternSnapState);
        }

        private void ChangeState(EditorSnapStateBase target)
        {
            _currentSnapState?.OnExit();
            target?.OnExit();
            _currentSnapState = target;
            UpdatePointer(target);
        }

        private void UpdatePointer(EditorSnapStateBase target)
        {
            _pointerRect.color = target.PointerColor;
            _pointerRect.sprite = target.PointerImage;
        }

        private void Update()
        {
            if (!_fieldHover.IsPointerOver.CurrentValue) return;


            if (!UpdatePointer(out var laneIndex, out var noteTime)) return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _currentSnapState?.OnCreate(laneIndex, noteTime);
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _currentSnapState?.OnDelete(laneIndex, noteTime);
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

            _pointerRect.rectTransform.anchoredPosition = pos;
            return true;
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
    }
}
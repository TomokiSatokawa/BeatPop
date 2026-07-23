using InGame.Effect;
using Input;
using R3;
using Sound;
using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// ÉmÅ[ÉcÇÃè¡Ç∑îªíËÇ∆éwé¶
    /// </summary>
    public class NodeInputHandler : MonoBehaviour
    {
        [SerializeField] private NodeController _nodeController;
        [SerializeField] private HoldNodeFillManager _holdNodeFillManager;
        [SerializeField] private LaneClickEffect _laneClick;

        private void Start()
        {
            SubscribeInput();
        }

        private void SubscribeInput()
        {
            InputManager.LeftLane.Where(_ => !InputManager.FlickLeftLane.CurrentValue).Subscribe(b => HandleClickLaneInput(0, b, false)).AddTo(this);
            InputManager.RightLane.Where(_ => !InputManager.FlickRightLane.CurrentValue).Subscribe(b => HandleClickLaneInput(1, b, false)).AddTo(this);
            InputManager.OnFlick.Subscribe(x => HandleClickLaneInput(x, false, true)).AddTo(this);
        }

        private void HandleClickLaneInput(int lane, bool isClick, bool isFlick)
        {
            var node = _nodeController.GetClickNode(lane);

            if (IsClickNode(isClick, isFlick, node))
            {
                _nodeController.ClickNode(node);
                return;
            }

            if (_holdNodeFillManager.HasFill(lane)) return;

            bool isNextFlick = node?.NodeObjData.InputType == InputType.Flick;

            if (isClick && !isNextFlick)
                EmptyClick(lane);
        }

        private bool IsClickNode(bool isClick, bool isFlick, NodeObject node)
        {
            if (node == null) return false;

            InputType inputType = GetInputType(isClick, isFlick);
            if (node.NodeObjData.InputType == inputType)
            {
                return true;
            }
            return false;
        }

        private static InputType GetInputType(bool isClick, bool isFlick)
        {
            if (isFlick)
                return InputType.Flick;

            return isClick ? InputType.Down : InputType.Up;
        }

        private void EmptyClick(int lane)
        {
            SoundManager.SE.PlaySE(SESoundType.EmptyHit);
            _laneClick.PlayLaneHighlight(lane);
        }
    }
}

public enum InputType
{
    None, Down, Up, Flick, Hold
}


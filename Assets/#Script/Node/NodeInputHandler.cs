using Common.PlaySystem;
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
        [SerializeField] private LaneClick _laneClick;
       
        void Start()
        {
            InputManager.LeftLane.Where(_ => !InputManager.FlickLeftLane.CurrentValue).Subscribe(b => ClickLane(0, b, false)).AddTo(this);
            InputManager.RightLane.Where(_ => !InputManager.FlickRightLane.CurrentValue).Subscribe(b => ClickLane(1, b, false)).AddTo(this);
            InputManager.OnFlick.Subscribe(x => ClickLane(x, false, true)).AddTo(this);
        }
        public void ClickLane(int lane, bool isClick, bool isFlick)
        {
            var node = _nodeController.GetClickNode(lane);

            if (node != null)
            {
                InputType inputType = GetInputType(isClick, isFlick);

                if (node.NodeObjData.InputType == inputType)
                {
                    var se = InGameCustomSoundData.I.NodeSE[node.Type];
                    _nodeController.ClickNode(se, node);
                    return;
                }
            }
            if (_holdNodeFillManager.HasFill(lane)) return;

            if (isClick && node.NodeObjData.InputType != InputType.Flick)
                EmptyClick(lane);
        }

        private InputType GetInputType(bool isClick, bool isFlick)
        {
            InputType inputType = InputType.None;

            if (isFlick)
            {
                inputType = InputType.Flick;
            }
            else
            {
                inputType = isClick ? InputType.Down : InputType.Up;
            }

            return inputType;
        }

        private void EmptyClick(int lane)
        {
            SoundManager.I.PlaySESound(SESoundType.EmptyHit);
            _laneClick.PlayLaneHighlight(lane);
        }
    }
}

public enum InputType
{
    None, Down, Up, Flick, Hold
}

